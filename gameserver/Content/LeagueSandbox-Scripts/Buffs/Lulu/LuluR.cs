using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using System.Collections.Generic;
using GameServerCore.Domain.GameObjects.Spell.Missile;

namespace Buffs
{
    internal class LuluR : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private float _healthBefore;
        private float _meantimeDamage;
        private float _healthNow;
        private float _healthBonus;
        IObjAiBase Owner;
        ISpell OwnerSpell;
        IParticle cast;
		IParticle p;
        public ISpellSector AOE;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Owner = ownerSpell.CastInfo.Owner;
            var owner = ownerSpell.CastInfo.Owner;
            OwnerSpell = ownerSpell;
			ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, TargetExecute, false);
            PlayAnimation(owner, "Spell4", 1f);
			//AddParticleTarget(OwnerSpell.CastInfo.Owner, unit, "Lulu_R_knock_up_impact.troy", unit);
			AddParticleTarget(OwnerSpell.CastInfo.Owner, unit, "Lulu_R_reverse.troy", unit);
			AddParticleTarget(OwnerSpell.CastInfo.Owner, unit, "Lulu_R_forward.troy", unit);
            p = AddParticleTarget(OwnerSpell.CastInfo.Owner, unit, "Lulu_R_cas.troy", unit,buff.Duration,1);
            StatsModifier.Size.PercentBonus = StatsModifier.Size.PercentBonus + 1;
            _healthBefore = unit.Stats.CurrentHealth;
            _healthBonus = 150 + 150 * ownerSpell.CastInfo.SpellLevel;
            StatsModifier.HealthPoints.BaseBonus = StatsModifier.HealthPoints.BaseBonus + 150 + 150 * ownerSpell.CastInfo.SpellLevel;
            unit.Stats.CurrentHealth = unit.Stats.CurrentHealth + 150 + 150 * ownerSpell.CastInfo.SpellLevel;
            unit.AddStatModifier(StatsModifier);
			AOE = ownerSpell.CreateSpellSector(new SectorParameters
            {
                BindObject = owner,
                Length = 450f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
        }
		public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            AddBuff("TalonSlow", 1f, 1, spell, target, Owner);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveParticle(p);
			ApiEventManager.OnSpellHit.RemoveListener(this);
            AOE.SetToRemove();
            AddParticleTarget(OwnerSpell.CastInfo.Owner, unit, "Lulu_R_expire", unit);
            _healthNow = unit.Stats.CurrentHealth - _healthBonus;
            _meantimeDamage = _healthBefore - _healthNow;
            var bonusDamage = _healthBonus - _meantimeDamage;
            if (unit.Stats.CurrentHealth > unit.Stats.HealthPoints.Total)
            {
                unit.Stats.CurrentHealth = unit.Stats.CurrentHealth - bonusDamage;
            }
        }

        public void OnUpdate(float diff)
        {

        }
    }
}

