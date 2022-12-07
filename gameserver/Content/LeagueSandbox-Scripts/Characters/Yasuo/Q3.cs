using System.Linq;
using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class YasuoQ3W : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
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
			   FaceDirection(end, owner,true);
               owner.CancelAutoAttack(false, false);
               owner.UpdateMoveOrder(OrderType.AttackTo, true);			   
        }

        public void OnSpellCast(ISpell spell)
        {
		   var owner = spell.CastInfo.Owner;
		   RemoveBuff(owner, "YasuoQ3W");
        }

        public void OnSpellPostCast(ISpell spell)
        {
		   var owner = spell.CastInfo.Owner;
		   //RemoveBuff(owner, "YasuoQ3W");
		   if (!owner.HasBuff("YasuoE"))
           {
			   RemoveBuff(owner, "YasuoQ3W");
		       SpellCast(owner, 3, SpellSlotType.ExtraSlots, false, owner, Vector2.Zero);
		   }
		   else
			  {
				  RemoveBuff(owner, "YasuoQ3W");
				  owner.PlayAnimation("Spell1_Dash", 0.5f, 0, 1);
				  //owner.RemoveBuffsWithName("YasuoE");
				  AddParticle(owner, owner, "Yasuo_Base_EQ_cas.troy", owner.Position);
				  spell.CreateSpellSector(new SectorParameters
                  {
                  BindObject = owner,
                  Length = 250f,
				  SingleTick = true,
                  //OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                  Type = SectorType.Area
                  });
			  }
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;

            var ADratio = owner.Stats.AttackDamage.Total;
            var Damage = ADratio + (spell.CastInfo.SpellLevel * 20);      
            AddParticleTarget(owner, owner, "Yasuo_Base_Q_hit_tar", target);
            target.TakeDamage(owner, Damage,DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            ForceMovement(target, "RUN", new Vector2(target.Position.X + 10f, target.Position.Y + 10f), 13f, 0, 16.5f, 0);
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
	public class YasuoQ3 : ISpellScript
    {
        IObjAiBase Owner;
		Vector2 A;
		Vector2 B;
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
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
			RemoveBuff(owner, "YasuoQ3W");
			AddBuff("DontM", 0.5f, 1, spell, spell.CastInfo.Owner , spell.CastInfo.Owner,false);		
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
			//owner.PlayAnimation("idle_out",3f);
			RemoveBuff(owner, "YasuoQ3W");
			A = GetPointFromUnit(owner, 75.0f);
            B = GetPointFromUnit(owner, 1150.0f);
            var ownerSkinID = owner.SkinID;
			SpellCast(owner, 2, SpellSlotType.ExtraSlots, B, Vector2.Zero, true, A);    		
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

    public class YasuoQ3Mis : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true
            // TODO
        };

        //Vector2 direction;
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
			var ADratio = owner.Stats.AttackDamage.Total;
            var Damage = ADratio + (spell.CastInfo.SpellLevel * 20);
            target.TakeDamage(owner, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
			ForceMovement(target, "RUN", new Vector2(target.Position.X + 5f, target.Position.Y + 5f), 13f, 0, 16.5f, 0);	
            AddParticleTarget(owner, target, "Yasuo_Base_Q_hit_tar.troy", target); 
            AddParticleTarget(owner, target, "Yasuo_Base_Q_wind_hit_tar.troy", target);
            AddParticleTarget(owner, target, ".troy", target);					
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
}