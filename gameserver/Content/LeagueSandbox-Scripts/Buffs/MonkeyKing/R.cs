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
    class MonkeyKingSpinToWin : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IObjAiBase Owner;
		IParticle p;
		IParticle p2;
		IBuff thisBuff;
        public ISpellSector AOE;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			thisBuff = buff;
            Owner = ownerSpell.CastInfo.Owner;
			Owner.SetSpell("MonkeyKingSpinToWinLeave", 3, true);
			PlayAnimation(Owner, "spell4",0.3f);     
            ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, TargetExecute, false);
			ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("MonkeyKingSpinToWinLeave"), R2OnSpellCast);
			p = AddParticleTarget(Owner, unit, "MonkeyKing_Base_R_Cas.troy", unit,buff.Duration,1);
			p2 = AddParticleTarget(Owner, unit, "MonkeyKing_Base_R_Cas_Glow.troy", unit,buff.Duration,1);
            AOE = ownerSpell.CreateSpellSector(new SectorParameters
            {
                BindObject = Owner,
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
            var damage = 11f + (8f * Owner.GetSpell(3).CastInfo.SpellLevel - 1) + AP;
            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL,DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
			AddParticleTarget(Owner, target, "MonkeyKing_Base_R_Tar.troy", target);
			AddParticleTarget(Owner, target, "MonkeyKing_Base_R_Tar_Audio.troy", target);
        }
		public void R2OnSpellCast(ISpell spell)
        {
            if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {			
            thisBuff.DeactivateBuff();
			}
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Owner.SetSpell("MonkeyKingSpinToWin", 3, true);
			StopAnimation(unit, "spell4",true,true,true);    
            ApiEventManager.OnSpellHit.RemoveListener(this);
			RemoveParticle(p);
			RemoveBuff(thisBuff);
            RemoveParticle(p2);
            AOE.SetToRemove();
        }
        public void OnUpdate(float diff)
        {

        }
    }
}