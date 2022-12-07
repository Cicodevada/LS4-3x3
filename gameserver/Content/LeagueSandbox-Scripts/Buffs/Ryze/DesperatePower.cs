using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Collections.Generic;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class DesperatePower : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IParticle pmodel;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            if (unit is IChampion c)
            {           
                pmodel = AddParticleTarget(c, c, "Ryze_Dark_DesperatePower.troy", c);
				//OverrideAnimation(unit, "Run_ULT", "RUN");
                StatsModifier.AttackSpeed.PercentBonus = (0.4f + (0.1f * (ownerSpell.CastInfo.SpellLevel - 1))) * buff.StackCount; // StackCount included here as an example
                StatsModifier.Range.FlatBonus = 175f * buff.StackCount;             
                unit.AddStatModifier(StatsModifier);
            }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			//OverrideAnimation(unit, "RUN", "Run_ULT");
            RemoveParticle(pmodel);
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
