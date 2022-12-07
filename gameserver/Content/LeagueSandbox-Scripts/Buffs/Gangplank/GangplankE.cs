using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using System.Collections.Generic;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class GangplankE : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_DEHANCER
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private List<IParticle> Particles => new List<IParticle>();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            var ADbuff = 12f + 7f * (ownerSpell.CastInfo.SpellLevel - 1);
            var MSbuff = 0.08f + 0.03f * (ownerSpell.CastInfo.SpellLevel - 1);

            if (unit == ownerSpell.CastInfo.Owner)
            {
                StatsModifier.MoveSpeed.PercentBonus = MSbuff;
                StatsModifier.AttackDamage.FlatBonus = ADbuff;
            }
            else
            {
                StatsModifier.MoveSpeed.PercentBonus = MSbuff / 2f;
                StatsModifier.AttackDamage.FlatBonus = ADbuff / 2f;
            }

            //_hudvisual = AddBuffHUDVisual("RaiseMorale", time, 1, unit);

            Particles.Add(AddParticleTarget(owner, null, "pirate_raiseMorale_cas", unit));
            Particles.Add(AddParticleTarget(owner, null, "pirate_raiseMorale_mis", unit));
            Particles.Add(AddParticleTarget(owner, null, "pirate_raiseMorale_tar", unit));
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //RemoveBuffHudVisual(_hudvisual);
            Particles.ForEach(particle => RemoveParticle(particle));
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
