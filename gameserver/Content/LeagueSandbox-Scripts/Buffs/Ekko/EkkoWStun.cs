using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;

namespace Buffs
{
    public class EkkoWStun : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; }

        IParticle stun;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //Change this back to buff.SetStatusEffect when it's removal get's fixed
			unit.PauseAnimation(true);
			unit.StopMovement(); 
			(unit as IObjAiBase).SetTargetUnit(null, true);	
            SetStatus(unit, StatusFlags.Stunned, true);
            stun = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "LOC_Stun", unit, buff.Duration);
			stun = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Ekko_Base_W_Stun_Tar", unit, buff.Duration);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			unit.PauseAnimation(false);
            SetStatus(unit, StatusFlags.Stunned, false);
            RemoveParticle(stun);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}