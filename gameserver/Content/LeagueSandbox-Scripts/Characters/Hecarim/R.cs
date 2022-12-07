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
    public class HecarimUlt: ISpellScript
    {
		ISpell spell;

        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };
        IMinion a;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
			 ApiEventManager.OnMoveSuccess.AddListener(this, owner, OnMoveEnd, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {       
        }

        public void OnSpellCast(ISpell spell)
        {	
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
			var current = new Vector2(owner.Position.X, owner.Position.Y);
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var dist = Vector2.Distance(current, spellPos);
            if (dist > 1200.0f)
            {
                dist = 1200.0f;
            }
			if (dist < 250.0f)
            {
                dist = 250.0f;
            }
            FaceDirection(spellPos, owner, true);
            var trueCoords = GetPointFromUnit(owner, dist);
			var End1 = GetPointFromUnit(owner, 1200f,5f);
			var End2 = GetPointFromUnit(owner, 1200f,-5f);
			var End3 = GetPointFromUnit(owner, 1200f,10f);
			var End4 = GetPointFromUnit(owner, 1200f,-10f);
			var Start1 = GetPointFromUnit(owner, -125f,45f);
			var Start2 = GetPointFromUnit(owner, -125f,-45f);
			var Start3 = GetPointFromUnit(owner, -250f,45f);
			var Start4 = GetPointFromUnit(owner, -250f,-45f);
			PlayAnimation(owner, "Spell4");
			AddParticleTarget(owner, owner, ".troy", owner, 0.5f);
			AddParticle(owner, null, ".troy", owner.Position);
            ForceMovement(owner, null, trueCoords, 1100, 0, 0, 0);
            if (owner.SkinID == 4)
			{
				a = AddMinion((IChampion)owner, "TestCubeRender", "TestCubeRender", owner.Position, owner.Team, owner.SkinID, true, false);
				AddParticleTarget(owner, a, "Hecarim_Skn4_R.troy", a);
				AddParticleTarget(owner, a, "Become_Transparent.troy", a,1000000f);
				ForceMovement(a, null, GetPointFromUnit(owner, 1200f), 1100, 0, 0, 0);
				CreateTimer((float) 2 , () =>{a.Die(CreateDeathData(false, 0, a, a, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, 0.0f));});
			}				
			SpellCast(owner, 4, SpellSlotType.ExtraSlots, GetPointFromUnit(owner, 1200f), Vector2.Zero, true, owner.Position);
			SpellCast(owner, 4, SpellSlotType.ExtraSlots, End2, Vector2.Zero, true, Start1);
			SpellCast(owner, 4, SpellSlotType.ExtraSlots, End1, Vector2.Zero, true, Start2);
			SpellCast(owner, 4, SpellSlotType.ExtraSlots, End4, Vector2.Zero, true, Start3);
            SpellCast(owner, 4, SpellSlotType.ExtraSlots, End3, Vector2.Zero, true, Start4);		
        }
		public void OnMoveEnd(IAttackableUnit owner)
        {
            if (owner is IObjAiBase c)
            {
                StopAnimation(c, "Spell4",true,true,true);
                if (c.SkinID == 4)
				{
					AddParticle(c, null, "Hecarim_SoundUltImpact.troy", c.Position);
					//a.Die(CreateDeathData(false, 0, a, a, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, 0.0f));
				}				
			    else {AddParticle(c, null, "Hecarim_R_impact.troy", c.Position);}
				AddParticle(c, null, ".troy", c.Position);            
            }
        }
		public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
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
	public class HecarimUltMissile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true
        };
		public List<IAttackableUnit> UnitsHit = new List<IAttackableUnit>();
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			UnitsHit.Clear();
			var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle
            });
				
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
			var ownerSkinID = owner.SkinID;
            float ad = owner.Stats.AttackDamage.Total;
            float damage = 75 + (spell.CastInfo.SpellLevel - 1) * 40 + ad;
			if (!UnitsHit.Contains(target))
            {
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            AddParticleTarget(owner, target, "Hecarim_R_tar.troy", target);
			}
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
	public class HecarimUltMissileGrab : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
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
			var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle
            });
			//var direction = new Vector3(owner.Position.X, 500, owner.Position.Y);
			//if (owner.SkinID == 4){AddParticleTarget(owner, missile, "Hecarim_Skn4_R.troy", missile,10,1,"","",new Vector3(owner.Position.X, 500, owner.Position.Y));}
				
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {       
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