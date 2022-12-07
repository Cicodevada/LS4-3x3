using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;

namespace Buffs
{
    class AzirR : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IBuff ThisBuff;
		IObjAiBase Owner;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ThisBuff = buff;
			Owner = ownerSpell.CastInfo.Owner;
			PlayAnimation(unit, "Spawn");
			//unit.UpdateMoveOrder(OrderType.AttackTo, true);						
			AddParticleTarget(Owner, unit, "", Owner);
			AddParticleTarget(Owner, unit, "", Owner);
			AddParticleTarget(Owner, unit, "", Owner);
			AddParticleTarget(Owner, unit, "", Owner);
			AddParticleTarget(Owner, unit, "", Owner,10,1,"head","head");			
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			AddParticle(Owner, null, "", unit.Position);
            if (unit != null && !unit.IsDead)
            {
				unit.TakeDamage(unit, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
            }
        }
        public void OnUpdate(float diff)
        {          
        }
    }
}