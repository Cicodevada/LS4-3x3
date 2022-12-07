using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Domain.GameObjects.Spell.Missile;


namespace Buffs
{
    class GladiatorDravenLeftAxeReturn : IBuffGameScript
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
		IObjAiBase owner;
		IAttackableUnit Unit;
		IParticle p;
		public ISpellSector CC;
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Unit = unit;
			owner = ownerSpell.CastInfo.Owner;
			ibuff = buff;
			spell = ownerSpell;
            ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, TargetExecute, false);
		    CC = ownerSpell.CreateSpellSector(new SectorParameters
            {
                BindObject = Unit,
                Length = 125f,
                Tickrate = 2,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveParticle(p);
			CC.SetToRemove();
		    unit.TakeDamage(unit, 1000000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);
            ApiEventManager.OnPreAttack.RemoveListener(this, unit as IObjAiBase);
            ApiEventManager.OnCollision.RemoveListener(this, unit as IObjAiBase);						
        }
		public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            if (!(target != owner))
			{
			AOE(spell);
			ibuff.DeactivateBuff();
			}
        }
		public void AOE(ISpell spell)
        {
             var owner = spell.CastInfo.Owner;
			 AddBuff("DravenSpinningAttack", 8f, 1, spell, owner, owner);
			 AddParticle(owner, null, "Draven_Base_Q_reticle_self", owner.Position, 10f,1,"");
			 AddParticle(owner, null, "Draven_Base_Q_ReticleCatchSuccess", owner.Position, 10f,1,"");
        } 
       
        
        public void OnUpdate(float diff)
        {         			
        }
    }
}