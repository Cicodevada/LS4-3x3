using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;


namespace Buffs
{
    internal class ZhonyasHourglass : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IParticle Gold;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			if (ownerSpell.CastInfo.Owner is IChampion c)
            {
				 c.PauseAnimation(true);
				 c.StopMovement(); 
                 c.SetTargetUnit(null, true);	
				 SetStatus(c, StatusFlags.Stunned, true);
                 SetStatus(c, StatusFlags.Invulnerable, true);
				 c.Stats.SetActionState(ActionState.CAN_MOVE, false);
				 c.Stats.SetActionState(ActionState.CAN_ATTACK, false);
				 buff.SetStatusEffect(StatusFlags.Targetable, false);
				 Gold = AddParticleTarget(c, c, "zhonyas_ring", c, buff.Duration);
				 Gold = AddParticleTarget(c, c, "zhonyas_cylinder", c, buff.Duration);
				 Gold = AddParticleTarget(c, c, "zhonya_ring_self_skin", c, buff.Duration);
				 Gold = AddParticleTarget(c, c, "zhonyas_ring_activate", c, buff.Duration);
            }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveParticle(Gold);
			if (ownerSpell.CastInfo.Owner is IChampion c)
            {
				 c.PauseAnimation(false);
				 SetStatus(c, StatusFlags.Stunned, false);
                 SetStatus(c, StatusFlags.Invulnerable, false);
				 c.Stats.SetActionState(ActionState.CAN_MOVE, true);
				 c.Stats.SetActionState(ActionState.CAN_ATTACK, true);
				 buff.SetStatusEffect(StatusFlags.Targetable, true);
            }
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
