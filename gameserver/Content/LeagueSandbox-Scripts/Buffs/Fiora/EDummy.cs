using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;

namespace Buffs
{
    internal class FioraFlurryDummy : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IBuff thisBuff;
        IParticle highlander;
        string particle;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			thisBuff = buff;
            if (unit is IObjAiBase owner)
            {         	
            StatsModifier.AttackSpeed.PercentBonus = StatsModifier.MoveSpeed.PercentBonus += 15f/100f;
            unit.AddStatModifier(StatsModifier);
			}
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(highlander);
        }

        private void OnAutoAttack(IAttackableUnit target, bool isCrit)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}