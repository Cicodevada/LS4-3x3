using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using System.Numerics;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class Taunt : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; }

        IParticle p;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			if (unit is IChampion Taunter)
            {
			FaceDirection(ownerSpell.CastInfo.Owner.Position, Taunter);
			Taunter.SetTargetUnit(ownerSpell.CastInfo.Owner, true);
            Taunter.UpdateMoveOrder(OrderType.AttackTo, true);
			}
            p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "LOC_Taunt", unit, buff.Duration, bone: "head");
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			if (unit is IChampion Taunter)
            {
			Taunter.SetTargetUnit(null, true);
			}
            RemoveParticle(p);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}