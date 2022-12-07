using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace Buffs //TODO: Find proper Buff Name
{
    internal class NoxiousTrap : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_DEHANCER
        };

        public BuffType BuffType => BuffType.COMBAT_ENCHANCER;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IObjAiBase owner;
        IAttackableUnit Unit;
        IParticle p;
        float damage;
        float timeSinceLastTick = 1000f;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            owner = ownerSpell.CastInfo.Owner;
            Unit = unit;
            float APratio = owner.Stats.AbilityPower.Total * 0.125f;
            damage = 18.75f + 31.25f * ownerSpell.CastInfo.SpellLevel + APratio;

            StatsModifier.MoveSpeed.PercentBonus -= 0.2f + 0.1f * ownerSpell.CastInfo.SpellLevel;
            unit.AddStatModifier(StatsModifier);
            p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Global_Slow.troy", unit, buff.Duration);

        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
        }

        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;
            if (timeSinceLastTick >= 1000f && !Unit.IsDead && Unit != null)
            {
                Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                timeSinceLastTick = 0;
            }
        }
    }
}

