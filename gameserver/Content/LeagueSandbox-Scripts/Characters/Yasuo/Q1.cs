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
    public class YasuoQW : ISpellScript
    {
        IObjAiBase Owner;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {         
            TriggersSpellCasts = true,
            IsDamagingSpell = true
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
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var ownerSkinID = owner.SkinID;
            if (!owner.HasBuff("YasuoE"))
            {
		     SpellCast(owner, 0, SpellSlotType.ExtraSlots, false, owner, Vector2.Zero);
		    }
		  else
			  {
				  owner.PlayAnimation("Spell1_Dash", 0.5f, 0, 1);
				  AddParticle(owner, owner, "Yasuo_Base_EQ_cas.troy", owner.Position);
				  owner.RemoveBuffsWithName("YasuoE");
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
            target.TakeDamage(owner, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddParticleTarget(owner, target, "Yasuo_Base_Q_hit_tar.troy", target);
            AddBuff("YasuoQ", 10.0f, 1, spell, owner, owner);
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

    public class YasuoQ : ISpellScript
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
        }
        public void OnSpellCast(ISpell spell)
        {      
		   var owner = spell.CastInfo.Owner;
		   AddBuff("DontM", 0.5f, 1, spell, spell.CastInfo.Owner , spell.CastInfo.Owner,false);		
           AddParticle(owner, owner, "Yasuo_Base_Q_WindStrike.troy", owner.Position); 
        }

        public void OnSpellPostCast(ISpell spell)
        {	
		   var owner = spell.CastInfo.Owner;    
           spell.CreateSpellSector(new SectorParameters
                    {
                        BindObject = owner,
                        Length = 450f,
                        Width = 80f,
                        PolygonVertices = new Vector2[]
                    {
                    // Basic square, however the values will be scaled by Length/Width, which will make it a rectangle
                    new Vector2(-1, 0),
                    new Vector2(-1, 1),
                    new Vector2(1, 1),
                    new Vector2(1, 0)
                    },
                        SingleTick = true,
                        Type = SectorType.Polygon
                    });				          
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ADratio = owner.Stats.AttackDamage.Total;
            var Damage = ADratio + (spell.CastInfo.SpellLevel * 20);
            target.TakeDamage(owner, Damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddParticleTarget(owner, target, "Yasuo_Base_Q_hit_tar.troy", target);
			AddBuff("YasuoQ", 10.0f, 1, spell, owner, owner);
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
