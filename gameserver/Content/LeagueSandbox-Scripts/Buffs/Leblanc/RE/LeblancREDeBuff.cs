using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;

namespace Buffs
{
    internal class LeblancREDeBuff : IBuffGameScript
    {
       public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
			BuffType = BuffType.COMBAT_DEHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
		};

        public IStatsModifier StatsModifier { get; private set; }

        private IParticle buff1;
        private IParticle buff2;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var caster = ownerSpell.CastInfo.Owner;
            unit.StopMovement();
            if (unit is IObjAiBase ai)
            {
                ai.SetTargetUnit(null, true);
            }

            buff1 = AddParticleTarget(caster, unit, "LeBlanc_Base_RE_tar", unit, 1.5f);
            buff2 = AddParticleTarget(caster, unit, "", unit);
            unit.Stats.SetActionState(ActionState.CAN_MOVE, false);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(buff1);
            RemoveParticle(buff2);
            unit.Stats.SetActionState(ActionState.CAN_MOVE, true);
            if (unit is IMonster ai && buff.SourceUnit is IChampion ch)
            {
                ai.SetTargetUnit(ch, true);
            }
        }

        public void OnUpdate(float diff)
        {
        }
    }
}