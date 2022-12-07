using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;


namespace Buffs
{
    internal class PotionOfGiantStrength : IBuffGameScript
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
            var extraHP = 120f + (6.39f * owner.Stats.Level);

            StatsModifier.HealthPoints.FlatBonus = extraHP;
            owner.Stats.CurrentHealth = owner.Stats.CurrentHealth + extraHP;
            StatsModifier.AttackDamage.FlatBonus = 15f;

            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
