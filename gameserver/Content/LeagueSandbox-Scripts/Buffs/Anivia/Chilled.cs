using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class Chilled : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData();

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            // TODO: Implement global variables which can be assigned values outside of the buff script.
            StatsModifier.MoveSpeed.PercentBonus = StatsModifier.MoveSpeed.PercentBonus - 0.2f;
            if (ownerSpell.SpellName == "GlacialStorm")
            {
                StatsModifier.AttackSpeed.PercentBonus = StatsModifier.AttackSpeed.PercentBonus - 0.2f;
            }
            unit.AddStatModifier(StatsModifier);

            // ApplyAssistMarker
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {

        }
    }
}

