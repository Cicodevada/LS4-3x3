using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace Buffs
{
    internal class MoveQuick : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.HASTE
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IParticle p;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            //p = AddParticleTarget(owner, unit, "MoveQuick_buf2.troy", unit, 1, buff.Duration); 
            p = AddParticleTarget(owner, unit, "MoveQuick_buf.troy", unit, buff.Duration); //Take a look at whi the particles disapear later

            StatsModifier.MoveSpeed.PercentBonus += 0.06f + 0.04f * ownerSpell.CastInfo.SpellLevel;
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

