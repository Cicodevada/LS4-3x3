using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    class Feast : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
			MaxStacks = 6
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			if (unit is IObjAiBase ai)
            {
		    var HealthPoints = 70 + 40 * (ai.GetSpell(3).CastInfo.SpellLevel - 1);
            StatsModifier.HealthPoints.FlatBonus += HealthPoints;
			StatsModifier.Size.FlatBonus += 0.1f;
            ai.AddStatModifier(StatsModifier);
			}
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
