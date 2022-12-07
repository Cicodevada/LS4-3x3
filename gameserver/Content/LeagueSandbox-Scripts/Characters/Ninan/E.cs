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
    public class NinanE : ISpellScript
    {
        IAttackableUnit Target;
		IParticle p;
		IParticle p2;
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
			owner.SetTargetUnit(null, true);
			owner.CancelAutoAttack(false, false);
			SetStatus(owner, StatusFlags.Ghosted, true);
        }

        public void OnSpellCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			SetStatus(owner, StatusFlags.Ghosted, true);
			var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
			var distt = dist + 225;
			var targetPos = GetPointFromUnit(owner,distt);
            if (!(dist >=350f))
            {
            PlayAnimation(owner, "Spell3");
			PlaySound("Play_sfx_Talon_TalonE_cast", owner);
			PlaySound("Play_sfx_Talon_TalonE_buffactivate_short", owner);
            ApiEventManager.OnMoveEnd.AddListener(this, owner, OnMoveEnd, true);			
            ForceMovement(owner, null, targetPos, 800, 0, 0, 0);
            p = AddParticleTarget(owner, owner, "Talon_Base_E_cas_trail.troy", owner, 10f);
			p2 = AddParticleTarget(owner, owner, "Talon_Base_E_land.troy", owner, 10f);
			}
			else if (dist > 350f && !(dist >= 800f))
			{
				SpellCast(owner, 6, SpellSlotType.ExtraSlots, false, Target, Vector2.Zero);	
				PlaySound("Play_sfx_Talon_TalonQ_cast", owner);
			}
			else if (dist >= 800f)
			{
                OnSpellPostCast(spell);
			}
        }

        public void OnSpellPostCast(ISpell spell)
        {         
        }
        public void OnMoveEnd(IAttackableUnit owner)
        {
			RemoveParticle(p);
			RemoveParticle(p2);
			StopAnimation(owner, "Spell3",true,true,true);  	
            ForceMovement(owner, "Spell3_long_torun", GetPointFromUnit(owner, 250f), 500, 0, 0, 0);			
			SetStatus(owner, StatusFlags.Ghosted, false);
            PlaySound("Play_sfx_Talon_TalonE_land", owner);			
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
	public class NinanEMissile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            },
            TriggersSpellCasts = true

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

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
			var ownerSkinID = owner.SkinID;
            float ad = (owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue) * 0.5f;
            float damage = 35 + (owner.GetSpell(2).CastInfo.SpellLevel-1 - 1) * 20 + ad;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            AddParticleTarget(owner, target, "Talon_Base_W_Tar.troy", target);
			AddBuff("NinanE2", 4f, 1, spell, target, owner);
			//AddParticleTarget(Owner, Unit, "Talon_Base_Q2_tar.troy", Unit);	
			PlaySound("Play_sfx_Talon_TalonQ_hit", target);	
			if (!target.HasBuff("NinanPassiveCooldown")&&!target.HasBuff("NinanPassiveBleed"))
                {     
			    AddBuff("NinanPassiveStack", 6f, 1, spell, target, owner,false);
			    }
                else
			    {
			    }	        
				missile.SetToRemove();
        }
        public void OnSpellCast(ISpell spell)
        {
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
	public class NinanE2 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true

        };
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			PlaySound("Play_sfx_Talon_TalonE_cast", owner);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {         
        }
        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
			PlaySound("Play_sfx_Talon_TalonE_buffactivate_short", spell.CastInfo.Owner);
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
