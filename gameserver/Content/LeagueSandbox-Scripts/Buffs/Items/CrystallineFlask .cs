using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;


namespace Buffs
{
    internal class ItemCrystalFlask : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.HEAL,
            BuffAddType = BuffAddType.STACKS_AND_CONTINUE,
            MaxStacks = 3
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IParticle potion;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            StatsModifier.ManaRegeneration.FlatBonus = 5f;
            StatsModifier.HealthRegeneration.FlatBonus = 10f;

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
