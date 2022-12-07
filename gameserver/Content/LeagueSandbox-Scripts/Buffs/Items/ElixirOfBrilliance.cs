using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;


namespace Buffs
{
    internal class PotionOfBrilliance : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.HEAL
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IParticle potion;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            StatsModifier.AbilityPower.FlatBonus = 25f + (8.3f * owner.Stats.Level);
            StatsModifier.CooldownReduction.FlatBonus = 0.1f;

            unit.AddStatModifier(StatsModifier);
            //TODO: CooldownReduction
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
