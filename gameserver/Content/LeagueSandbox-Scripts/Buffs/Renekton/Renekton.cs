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
    class RenektonReignOfTheTyrant : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IParticle p;
		IParticle p2;
		IObjAiBase Owner;
		public ISpellSector AOE;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            if (unit is IChampion c)
            {   
		        Owner = c;
				ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, TargetExecute, false);
		        AOE = ownerSpell.CreateSpellSector(new SectorParameters
                {
                BindObject = c,
                Length = 270f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
                });
                AddParticleTarget(c, c, "Renekton_Base_R_cas", c);        
                p = AddParticleTarget(c, c, "Renekton_Base_R_buf", c);
                p.SetToRemove();
				p2 = AddParticleTarget(c, c, "Renekton_Base_R_weapon", c, buff.Duration, 1, "WEAPON");
				//OverrideAnimation(unit, "Run_ULT", "RUN");
				var HealthBuff = 200f + 200f * ownerSpell.CastInfo.SpellLevel;
				StatsModifier.Size.BaseBonus = StatsModifier.Size.BaseBonus + 0.4f;
                StatsModifier.HealthPoints.BaseBonus += HealthBuff;        
                unit.AddStatModifier(StatsModifier);
				unit.Stats.CurrentHealth += HealthBuff;
            }
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var AP = Owner.Stats.AbilityPower.Total * 0.12f;
            var damage = 11f + (8f * Owner.GetSpell(3).CastInfo.SpellLevel - 1) + AP;

            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL,DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			//OverrideAnimation(unit, "RUN", "Run_ULT");
			AOE.SetToRemove();
            RemoveParticle(p);
			RemoveParticle(p2);
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
