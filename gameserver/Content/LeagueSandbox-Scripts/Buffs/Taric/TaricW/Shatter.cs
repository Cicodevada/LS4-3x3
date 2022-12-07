using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;

namespace Buffs
{
    internal class Shatter : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IParticle p;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            var ARratio = owner.Stats.Armor.Total * 0.05f;
            var Armor = 5f + 5f * (ownerSpell.CastInfo.SpellLevel - 1) + ARratio;

            if (unit != owner)
            {
              p = AddParticleTarget(owner, unit, "Shatter_tar.troy", unit, buff.Duration);
            }

            StatsModifier.Armor.FlatBonus = -(Armor);
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
