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
    class Tremors2 : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
		float Speed;
		IAttackableUnit Target;
        private ISpell spell;
		IBuff ibuff;
		IObjAiBase Owner;
		public ISpellSector AOE;
		IParticle p;
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Owner = ownerSpell.CastInfo.Owner;
			ibuff = buff;
			spell = ownerSpell;
			ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, TargetExecute, false);
			AOE = ownerSpell.CreateSpellSector(new SectorParameters
            {
                BindObject = Owner,
                Length = 450f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
			p = AddParticleTarget(unit, unit, "tremors_cas.troy", unit, 10f,1,"");	
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var AP = Owner.Stats.AbilityPower.Total * 0.6f;
            var damage = 65f * Owner.GetSpell(3).CastInfo.SpellLevel + AP;

            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL,DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }		
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveParticle(p);
			ApiEventManager.OnSpellHit.RemoveListener(this);
            AOE.SetToRemove();
        }
        public void OnUpdate(float diff)
        {        
        }
    }
}