using System.Collections.Generic;
using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class EkkoQ : ISpellScript
    {
        IObjAiBase Owner;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {      
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
		public static Vector2 end;
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

        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var ownerSkinID = owner.SkinID;          
            var start = GetPointFromUnit(owner, 25f);
		    var end = GetPointFromUnit(owner, 700f);
			FaceDirection(end, owner);
			//SpellCast(owner, 0, SpellSlotType.ExtraSlots, end, Vector2.Zero, true, start);
			SpellCast(owner, 0, SpellSlotType.ExtraSlots, end, end, true, Vector2.Zero);
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

    public class EkkoQMis : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
        };
		ISpellMissile m;
		ISpell Spell;
        public static List<IAttackableUnit> UnitsHit = new List<IAttackableUnit>();

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
			Spell = spell;
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });
			//CreateTimer((float) 0.25f , () =>{AddParticleTarget(owner, missile, "Ekko_Base_Q_Aoe_Dilation", missile);	;});			  
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, false);
			ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, true);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
			m = missile;
			if (!UnitsHit.Contains(target))
            {
            UnitsHit.Add(target);
            var owner = spell.CastInfo.Owner;
            float ap = owner.Stats.AbilityPower.Total;
            float damage = 40 + (spell.CastInfo.SpellLevel - 1) * 20 + ap;
			if (!target.HasBuff("EkkoPassiveSlow"))
			{
                AddBuff("EkkoPassive", 6f, 1, spell, target, owner); 					
			}
			else
			{
			}
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            AddParticleTarget(owner, target, "Ekko_Base_Q_Mis_Tar", target);
			if(target is IChampion)
			{		
                //ApiEventManager.OnSpellMissileEnd.RemoveListener(this);				
				missile.SetToRemove();
			}
			}
        }
        public void OnSpellCast(ISpell spell)
        {
        }
		public void OnMissileEnd(ISpellMissile missile)
        {
			var owner = missile.CastInfo.Owner;
            SpellCast(owner, 5, SpellSlotType.ExtraSlots, GetPointFromUnit(missile, 375), Vector2.Zero, true, missile.Position);   
        }
		public void Slow(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
			//AddBuff("", 2.24f, 1, spell, Time, Time, false);
			//AddParticlePos(owner, "Ekko_Base_Q_Aoe_Dilation.troy",missile.Position, missile.Position, 1);
			//AddParticle(owner, missile, "Ekko_Base_Q_Aoe_Resolve", missile.Position);
			SpellCast(owner, 5, SpellSlotType.ExtraSlots, GetPointFromUnit(m, 375), Vector2.Zero, true, m.Position);         			
            //SpellCast(owner, 2, SpellSlotType.ExtraSlots, owner.Position, owner.Position, true, missile.Position);
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
	public class EkkoQReturnDead : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {     
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
        };
		IParticle P;
        public List<IAttackableUnit> UnitsHit = Spells.EkkoQMis.UnitsHit;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });
			if (missile != null )
			{
				AddParticleTarget(owner, missile, "Ekko_Base_Q_Aoe_Dilation", missile,25000,0.85f);
				//P = AddParticle(owner, owner, "Ekko_Base_Q_Mis_Throw.troy", owner.Position);
				//P = AddParticle(owner, missile, "Ekko_Base_Q_Mis_02.troy", missile.Position,lifetime : 0f);
			}
			//if (owner.SkinID == 11 ){AddParticleTarget(owner, missile, "Ekko_Skin11_Q_Mis_03", missile);}			
            CreateTimer((float) 1.9f , () =>{ if (missile != null ){AddParticleTarget(owner, missile, "Ekko_Base_Q_Aoe_Resolve", missile);}});			
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
			if (!UnitsHit.Contains(target))
            {
                UnitsHit.Add(target);
            var owner = spell.CastInfo.Owner;
			var ownerSkinID = owner.SkinID;
            float ap = owner.Stats.AbilityPower.Total;
            float damage = 40 + (spell.CastInfo.SpellLevel - 1) * 20 + ap;
			if (!target.HasBuff("EkkoPassiveSlow"))
			{
                AddBuff("EkkoPassive", 6f, 1, spell, target, owner); 					
			}
			else
			{
			}
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            AddParticleTarget(owner, target, "Ekko_Base_Q_Mis_Tar", target);
			}
        }
        public void OnSpellCast(ISpell spell)
        {
        }
		public void OnMissileEnd(ISpellMissile missile)
        {
			//RemoveParticle(P);
            var owner = missile.CastInfo.Owner;
			IParticle M = AddParticlePos(owner, "",missile.Position,missile.Position);
			//AddBuff("", 2.24f, 1, spell, Time, Time, false);
			//AddParticlePos(owner, "Ekko_Base_Q_Aoe_Dilation.troy",missile.Position, missile.Position, 1);
            SpellCast(owner, 1, SpellSlotType.ExtraSlots, true, owner, M.Position);
            //SpellCast(owner, 2, SpellSlotType.ExtraSlots, owner.Position, owner.Position, true, missile.Position);
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

    public class EkkoQReturn : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
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
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });		
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
			//AddParticle(owner, missile, "Ekko_Base_Q_Aoe_Resolve", missile.Position);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            if (target == missile.CastInfo.Owner)
            {
				missile.SetToRemove();
            }
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("EkkoQ").CastInfo.SpellLevel;
            var APratio = owner.Stats.AbilityPower.Total * 0.6f;
            var damage = 50 + 25f * (spellLevel - 1) + APratio;			
            if (!UnitsHit.Contains(target))
            {
				if (target.Team != owner.Team && !(target is IObjBuilding || target is IBaseTurret))
				{
                   UnitsHit.Add(target);
                   target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                      if (!target.HasBuff("EkkoPassiveSlow"))
			          {
                           AddBuff("EkkoPassive", 6f, 1, spell, target, owner); 					
			          }
			          else
			          {
			          }
                   AddParticleTarget(owner, target, "Ekko_Base_Q_Mis_Return_Tar.troy", target, 1f);
				   AddParticleTarget(owner, target, "Ekko_Base_Q_Mis_Return_Hit_Sound.troy", target, 1f);
				}
            }
        }
		public void OnMissileEnd(ISpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
			PlayAnimation(owner, "Spell1_Catch",0.8f);
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
