using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;



namespace Buffs
{
    internal class MordekaiserChildrenOfTheGrave : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.SLOW,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        float timeSinceLastTick;
        IAttackableUnit Unit;
        float TickingDamage;
        IObjAiBase Owner;
        ISpell spell;
        bool limiter = false;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            
            TickingDamage = unit.Stats.HealthPoints.Total * (0.012f + (0.0025f * (ownerSpell.CastInfo.SpellLevel - 1)) + (owner.Stats.AbilityPower.Total * 0.00002f));
            var damage = unit.Stats.HealthPoints.Total * (0.12f + (0.025f * (ownerSpell.CastInfo.SpellLevel - 1)) + (owner.Stats.AbilityPower.Total * 0.0002f));
            Unit = unit;
            Owner = owner;
            spell = ownerSpell;
            limiter = true;


            AddParticleTarget(owner, unit, "mordekeiser_cotg_tar.troy", unit, buff.Duration);
            unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);
            owner.Stats.CurrentHealth += damage;
            unit.Stats.HealthRegeneration.PercentBonus = -1;
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {

        }

        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;

            if (timeSinceLastTick >= 1000.0f)
            {
                Unit.TakeDamage(Unit, TickingDamage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
                Owner.Stats.CurrentHealth = Owner.Stats.CurrentHealth + TickingDamage;
                timeSinceLastTick = 0;
            }
            if (Unit != null)
            {
                if (Unit.IsDead && limiter == true)
                {
                    var ghost = AddMinion(Owner, Unit.Model, Unit.Model, Unit.Position);
                    AddParticleTarget(Owner, ghost, "mordekeiser_cotg_skin.troy", ghost, lifetime: 30f);
                    AddBuff("MordekaiserChildrenOfTheGraveGhost", 40f, 1, spell, ghost, ghost);
                    limiter = false;
                }
            }
        }
    }
}
//TODO: Make healing for POST MITIGATION damage
