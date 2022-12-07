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
    class NasusE : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IObjAiBase Owner;
		IMinion NA; 
        public ISpellSector AOE;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Owner = ownerSpell.CastInfo.Owner;
            ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, TargetExecute, false);
			AddParticle(Owner, null, "Nasus_Base_E_SpiritFire.troy", unit.Position,5f,1);
			AddParticle(Owner, null, "Nasus_E_Green_Ring.troy", unit.Position,5f,1);
            AOE = ownerSpell.CreateSpellSector(new SectorParameters
            {
                BindObject = NA,
                Length = 270f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var AP = Owner.Stats.AbilityPower.Total * 0.12f;
            var damage = 11f + (8f * Owner.GetSpell(2).CastInfo.SpellLevel - 1) + AP;

            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL,DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			unit.TakeDamage(unit, 100000, DamageType.DAMAGE_TYPE_TRUE,DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
            ApiEventManager.OnSpellHit.RemoveListener(this);
            AOE.SetToRemove();
        }
        public void OnUpdate(float diff)
        {

        }
    }
}