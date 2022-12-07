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
    class FizzSeastoneTridentActive : IBuffGameScript
    {
		public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.POISON,
			BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        float damage;
        float timeSinceLastTick = 900f;
        IAttackableUnit Unit;
        IObjAiBase owner;
        IParticle p;
        IParticle p2;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            owner = ownerSpell.CastInfo.Owner as IChampion;
            Unit = unit;
            var AP = owner.Stats.AbilityPower.Total * 0.4f;
			var WLevel = owner.GetSpell("FizzSeastonePassive").CastInfo.SpellLevel;
            damage = 20 + ((ownerSpell.CastInfo.SpellLevel - 1)*10) + AP;
            p = AddParticleTarget(owner, unit, "Fizz_SeastoneTrident", unit, 1, buff.Duration);
            p2 = AddParticle(owner, unit, "", unit.Position, 1, buff.Duration);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
            RemoveParticle(p2);
        }
        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;

            if (timeSinceLastTick >= 1000.0f && Unit != null)
            {
                Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                timeSinceLastTick = 0f;
            }
        }
    }
}
