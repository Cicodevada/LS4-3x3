using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;


namespace Buffs
{
    internal class YellowTriket : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.INTERNAL,
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IAttackableUnit Unit;
        ISpell spell;
        float timeSinceLastTick = 0f;
        float counter;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Unit = unit;
            spell = ownerSpell;
            Unit.Stats.ManaRegeneration.PercentBonus = -1;
            Unit.Stats.CurrentMana = 60f;
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;

            if(timeSinceLastTick >= 60000.0f)
            {
                Unit.TakeDamage(spell.CastInfo.Owner, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
            }
            //This would be used if the ward's ManaPoints were being properly read
            /*if (timeSinceLastTick >= 1000.0f)
            {
                Unit.Stats.ManaPoints.FlatBonus -= 1;
                if(Unit.Stats.CurrentMana == 0)
                {
                  Unit.Die(Unit);
                }
            }*/

        }
    }
}
