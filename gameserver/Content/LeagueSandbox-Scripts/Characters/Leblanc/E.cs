using System;
using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class LeblancSoulShackle : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            }
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
			//if (!owner.HasBuff("LeblancSlideM") && owner.GetSpell("LeblancMimic").CastInfo.SpellLevel >= 1 )
            //{
			if (!owner.HasBuff("LeblancSlideM")&&owner.HasBuff("LeblancMimic"))
             {
			owner.SetSpell("LeblancSoulShackleM", 3, true);
             }			
			AddParticleTarget(owner, owner, "LeBlanc_Base_E_cas", owner, bone:"L_HAND");
			AddParticleTarget(owner, owner, "LeBlanc_Base_E_cas_02", owner, bone:"L_HAND");
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            if (missile is ISpellCircleMissile skillshot)
            {            
			var spellLevel = owner.GetSpell("LeblancSoulShackle").CastInfo.SpellLevel;
            var AP = owner.Stats.AbilityPower.Total * 0.5f;
			var MAXAP = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.65f;
		    var QLevel = owner.GetSpell(0).CastInfo.SpellLevel;
			var RQLevel = owner.GetSpell(3).CastInfo.SpellLevel;
            var damage = 40 + 25f*(spellLevel - 1) + AP;
			var damagemax=55 + 25f*(QLevel - 1) + AP;
			var QMarkdamage = damage + damagemax;
			var damagemaxx=100 + 100f*(spellLevel - 1)+ MAXAP;
			var RQMarkdamage = damage + damagemaxx;
                if (target.HasBuff("LeblancChaosOrb"))
                {							
				    target.RemoveBuffsWithName("LeblancChaosOrb");
					target.TakeDamage(owner, QMarkdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
                }
			    else if (target.HasBuff("LeblancChaosOrbM"))
                {
					target.RemoveBuffsWithName("LeblancChaosOrbM");
					target.TakeDamage(owner, RQMarkdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
                }
				else
				{
				    target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                }
				AddParticleTarget(owner, target, "LeBlanc_Base_Q_tar", target);
				AddParticleTarget(owner, owner, "LeBlanc_Base_E_chain", target, lifetime: 1.5f,1,"L_BUFFBONE_GLB_HAND_LOC","C_BuffBone_Glb_Center_Loc");
			    //AddParticleTarget(owner, target, "LeBlanc_Base_E_indicator", target,10f,1,"C_BuffBone_Glb_Center_Loc");
				AddParticleTarget(owner, target, "", target);
				AddParticleTarget(owner, target, "LeBlanc_Base_E_tar_02", target);
                AddBuff("LeblancSoulShackle", 1.5f, 1, spell, target, owner);
				missile.SetToRemove();
            }
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}