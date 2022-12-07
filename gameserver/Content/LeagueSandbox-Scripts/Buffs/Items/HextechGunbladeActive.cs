using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;


namespace Buffs
{
    internal class HextechGunblade : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.SLOW
        };
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IParticle p;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            StatsModifier.MoveSpeed.PercentBonus = -0.4f;
            p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Global_Slow.troy", unit, lifetime: buff.Duration);
          //StatsModifier.CooldownReduction.FlatBonus = 10f;

            unit.AddStatModifier(StatsModifier);
            //TODO: CooldownReduction
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
