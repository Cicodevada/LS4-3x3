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
using GameServerCore;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System.Linq;

namespace Spells
{
    public class NinanQ : ISpellScript
    {
        IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
			owner.CancelAutoAttack(false);
			PlaySound("Play_sfx_Talon_TalonQAttack_OnCast", owner);
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
			var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
            if (dist > 225f && !(dist >= 600f))
            {
				SpellCast(owner, 5, SpellSlotType.ExtraSlots, false, Target, Vector2.Zero);	
            }
            else 
            {
				SpellCast(owner, 0, SpellSlotType.ExtraSlots, false, Target, Vector2.Zero);	
            }          		
        }

        public void OnSpellPostCast(ISpell spell)
        {         	
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
    public class NinanQAttack : ISpellScript
    {
		IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
			TriggersSpellCasts = true,
            NotSingleTargetSpell = true,
			IsDamagingSpell = true,
            // TODO
        };
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
        }
        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			Target = target;
			PlaySound("Play_vo_Talon_TalonNoxianDiplomacyAttack_OnCast", Target);
        }
        public void OnSpellCast(ISpell spell)
        {
        }
        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			var ad = (owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue) * 1.2f;
            var damage = 80 + 25* (owner.GetSpell(0).CastInfo.SpellLevel-1) + ad;
			var Damage = damage * 1.5f;
			AddParticleTarget(owner, owner, "Talon_Base_Q1_cas", owner,10f, 1f,"C_buffbone_GLB_Layout_Loc","C_buffbone_GLB_Layout_Loc");
			AddParticleTarget(owner, Target, "Talon_Base_Q1_tar", Target,10f, 1f,"C_buffbone_GLB_Layout_Loc","C_buffbone_GLB_Layout_Loc");
			Target.TakeDamage(owner, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, true);
			PlaySound("Play_sfx_Talon_TalonQAttack_OnHit", Target);
			if (!Target.HasBuff("NinanPassiveCooldown")&&!Target.HasBuff("NinanPassiveBleed"))
                {     
			    AddBuff("NinanPassiveStack", 6f, 1, spell, Target, owner,false);
			    }
                else
			    {
			    }	
            owner.CancelAutoAttack(true);			
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
	public class NinanQDashAttack : ISpellScript
    {
        IAttackableUnit Target;
		ISpell Spell;
		private IObjAiBase owner;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			Spell = spell;
            Target = target;
			owner = spell.CastInfo.Owner;
			SetStatus(owner, StatusFlags.Ghosted, true);
			ApiEventManager.OnMoveEnd.AddListener(this, owner, OnMoveEnd, true);
            //ApiEventManager.OnMoveSuccess.AddListener(this, owner, OnMoveSuccess, true);
			owner.CancelAutoAttack(true,true);
        }

        public void OnSpellCast(ISpell spell)
        {       
		    var owner = spell.CastInfo.Owner;		      
            var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
			var distt = dist - 125;
			var time = distt/2200f;
			var targetPos = GetPointFromUnit(owner,distt);
			var ad = (owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue) * 1.2f;
            var damage = 80 + 25* (owner.GetSpell(0).CastInfo.SpellLevel-1) + ad;
			PlayAnimation(owner, "Spell1_leap");                         
			FaceDirection(targetPos, owner, true);
            ForceMovement(owner, null, targetPos, 2200, 0, 120, 0);
			CreateTimer((float) time , () =>
                {
				Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
				if (owner.Team != Target.Team && Target is IChampion)
                {
                 owner.SetTargetUnit(Target, true);
                 owner.UpdateMoveOrder(OrderType.AttackTo, true);
                }
				if (!Target.HasBuff("NinanPassiveCooldown")&&!Target.HasBuff("NinanPassiveBleed"))
                {     
			    AddBuff("NinanPassiveStack", 6f, 1, Spell, Target, owner,false);
			    }
                else
			    {
			    }	
				AddParticleTarget(owner, Target, "Talon_Base_Q2_tar", Target,10f, 1f,"C_buffbone_GLB_Layout_Loc","C_buffbone_GLB_Layout_Loc");
			    //AddParticleTarget(owner, Target, "Talon_Base_Q2_tar.troy", Target); 
				});
			AddParticleTarget(owner, owner, "Talon_Base_Q2_cas.troy", owner);
			AddParticleTarget(owner, owner, ".troy", owner); 		
        }

        public void OnSpellPostCast(ISpell spell)
        {      		   
        }
        public void OnMoveSuccess(IAttackableUnit unit)
        {
			if(unit is IObjAiBase owner)
            {             
			if (owner.Team != Target.Team && Target is IChampion)
            {
               owner.SetTargetUnit(Target, true);
               owner.UpdateMoveOrder(OrderType.AttackTo, true);
            }
			if (!Target.HasBuff("NinanPassiveCooldown")&&!Target.HasBuff("NinanPassiveBleed"))
                {     
			    AddBuff("NinanPassiveStack", 6f, 1, Spell, Target, owner,false);
			    }
                else
			    {
			    }	
			}
        }
		public void OnMoveEnd(IAttackableUnit owner)
        {
			SetStatus(owner, StatusFlags.Ghosted, false);			
			//StopAnimation(owner, "spell1", true, true, true);
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


