using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class NinanPassiveCooldown : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        float damage;
        float timeSinceLastTick = 200f;
        IAttackableUnit Unit;
        IObjAiBase owner;
        IParticle p;
		IBuff thisBuff;
		bool isVisible = true;
        IParticle p2;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			thisBuff = buff;
            owner = ownerSpell.CastInfo.Owner as IChampion;
            Unit = unit;
            var ADratio = (owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue) * 2f;  
            damage = (10 * owner.Stats.Level + ADratio + 65f) / 20f;			
            //p = AddParticleTarget(owner, unit, "talon_Q_bleed", unit, buff.Duration, 1f);
            p2 = AddParticleTarget(owner, unit, "Talon_Base_P_tar_tick", unit, buff.Duration, 1f);
			if (unit.IsDead)
            {
			RemoveParticle(p);
			RemoveBuff(thisBuff);
            RemoveParticle(p2);
			}
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
			RemoveBuff(thisBuff);
            RemoveParticle(p2);
        }
        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;

            if (timeSinceLastTick >= 100.0f && Unit != null)
            {
                Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                timeSinceLastTick = 0f;
            }
        }
    }
}
