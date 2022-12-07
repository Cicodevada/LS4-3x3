using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Collections.Generic;

namespace Buffs
{
    internal class Highlander : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IParticle highlander;
        string particle;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            var RLevel = owner.GetSpell("Highlander").CastInfo.SpellLevel;
            switch (RLevel)
            {
                case 1:
                    particle = "MasterYi_Base_R_Buf.troy";
                        break;
                case 2:
                    particle = "MasterYi_Base_R_Buf_Lvl2.troy";
                        break;
                case 3:
                    particle = "MasterYi_Base_R_Buf_Lvl3.troy";
                        break;
            }
            highlander = AddParticleTarget(owner, unit, particle, unit, buff.Duration);
			AddParticleTarget(owner, unit, "Highlander_buf", unit,buff.Duration);
            OverrideAnimation(owner, "run_haste", "RUN");
            StatsModifier.MoveSpeed.PercentBonus = StatsModifier.MoveSpeed.PercentBonus + (15f + ownerSpell.CastInfo.SpellLevel * 10) / 100f;
            StatsModifier.AttackSpeed.PercentBonus = StatsModifier.AttackSpeed.PercentBonus + (5f + ownerSpell.CastInfo.SpellLevel * 25) / 100f;
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(highlander);
			OverrideAnimation(unit, "RUN", "run_haste");
        }

        private void OnAutoAttack(IAttackableUnit target, bool isCrit)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
