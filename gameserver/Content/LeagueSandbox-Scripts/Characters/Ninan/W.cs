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
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class NinanW : ISpellScript
    {
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
			string Sound;
            //PlayAnimation(owner, "Spell1");
			switch (owner.SkinID)
            {
				case 5:
                    Sound = "Play_sfx_TalonSkin05_TalonW_OnCast";
                    break;
					
                case 12:
                    Sound = "Play_sfx_TalonSkin12_TalonW_OnCast";
                    break;

                case 20:
                    Sound = "Play_sfx_TalonSkin20_TalonW_OnCast";
                    break;
				case 29:
                    Sound = "Play_sfx_TalonSkin29_TalonW_OnCast";
                    break;
				case 38:
                    Sound = "Play_sfx_TalonSkin38_TalonW_OnCast";
                    break;
				case 39:
                    Sound = "Play_sfx_TalonSkin39_TalonW_OnCast";
                    break;

                default:
                    Sound = "Play_sfx_Talon_TalonW_OnCast";
                    break;
            }
			PlaySound("Play_vo_Talon_TalonRake_OnCast", owner);		
			PlaySound(Sound, owner);	
        }

        public void OnSpellCast(ISpell spell)
        {	
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner as IChampion;                  
            var start = GetPointFromUnit(owner, 25f);
			var start1 = GetPointFromUnit(owner, 25f,-11f);
			var start2 = GetPointFromUnit(owner, 25f,11f);
		    var end = GetPointFromUnit(owner, 900f);
		    var end1 = GetPointFromUnit(owner, 850f, -11f );
			var end2 = GetPointFromUnit(owner, 850f, 11f );
			SpellCast(owner, 1, SpellSlotType.ExtraSlots, end, end, true, Vector2.Zero);
			SpellCast(owner, 1, SpellSlotType.ExtraSlots, end1, end1, true, Vector2.Zero);
			SpellCast(owner, 1, SpellSlotType.ExtraSlots, end2, end2, true, Vector2.Zero);
            //PlaySound("Play_sfx_Cassiopeia_CassiopeiaPetrifyingGazeStun_OnBuffActivate", owner);
		    //SpellCast(owner, 1, SpellSlotType.ExtraSlots, end, Vector2.Zero, true, start);
            //SpellCast(owner, 1, SpellSlotType.ExtraSlots, end1, Vector2.Zero, true, start1); 
            //SpellCast(owner, 1, SpellSlotType.ExtraSlots, end2, Vector2.Zero, true, start2); 		
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

    public class NinanWMissileOne : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {        
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
			string Sound;
            //PlayAnimation(owner, "Spell1");
			switch (owner.SkinID)
            {
				case 5:
                    Sound = "Play_sfx_TalonSkin05_TalonWMissileOne_OnMissileLaunch";
                    break;
					
                case 12:
                    Sound = "Play_sfx_TalonSkin12_TalonWMissileOne_OnMissileLaunch";
                    break;

                case 20:
                    Sound = "Play_sfx_TalonSkin20_TalonWMissileOne_OnMissileLaunch";
                    break;
				case 29:
                    Sound = "Play_sfx_TalonSkin29_TalonWMissileOne_OnMissileLaunch";
                    break;
				case 38:
                    Sound = "Play_sfx_TalonSkin38_TalonWMissileOne_OnMissileLaunch";
                    break;
				case 39:
                    Sound = "Play_sfx_TalonSkin39_TalonWMissileOne_OnMissileLaunch";
                    break;

                default:
                    Sound = "Play_sfx_Talon_TalonWMissileOne_OnMissileLaunch";
                    break;
            }	
			PlaySound(Sound, owner);
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });

            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("NinanW").CastInfo.SpellLevel;
            var ADratio = (owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue) * 0.4f;
            var damage = 50 + 15f*(spellLevel - 1) + ADratio;
            if (!UnitsHit.Contains(target))
            {
                UnitsHit.Add(target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(owner, target, "Talon_Base_W_Tar.troy", target, 1f);
				PlaySound("Play_sfx_Talon_TalonWMissileOne_OnHit", owner);
				if (!target.HasBuff("NinanPassiveCooldown")&&!target.HasBuff("NinanPassiveBleed"))
                {     
			    AddBuff("NinanPassiveStack", 6f, 1, spell, target, owner,false);
				//AddBuff("TestAi_Talon", 6f, 1, spell, target, owner,false);
			    }
                else
			    {
			    }	
            }
        }

        public void OnMissileEnd(ISpellMissile missile)
        {
			string particles; 
            var owner = missile.CastInfo.Owner;
			string Sound;
            //PlayAnimation(owner, "Spell1");
			switch (owner.SkinID)
            {
				case 5:
                    Sound = "Stop_sfx_TalonSkin05_TalonWMissileOne_OnMissileLaunch";
                    break;
					
                case 12:
                    Sound = "Stop_sfx_TalonSkin12_TalonWMissileOne_OnMissileLaunch";
                    break;

                case 20:
                    Sound = "Stop_sfx_TalonSkin20_TalonWMissileOne_OnMissileLaunch";
                    break;
				case 29:
                    Sound = "Stop_sfx_TalonSkin29_TalonWMissileOne_OnMissileLaunch";
                    break;
				case 38:
                    Sound = "Stop_sfx_TalonSkin38_TalonWMissileOne_OnMissileLaunch";
                    break;
				case 39:
                    Sound = "Stop_sfx_TalonSkin39_TalonWMissileOne_OnMissileLaunch";
                    break;

                default:
                    Sound = "Stop_sfx_Talon_TalonWMissileOne_OnMissileLaunch";
                    break;
            }	
			PlaySound(Sound, owner);
			AddParticle(owner, null, "Talon_Base_W_End.troy", missile.Position, 0.7f,1f);
			CreateTimer((float) 0.7 , () =>
            {
            SpellCast(owner, 2, SpellSlotType.ExtraSlots, true, owner, missile.Position);
            //SpellCast(owner, 2, SpellSlotType.ExtraSlots, owner.Position, owner.Position, true, missile.Position);
	    	});		
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
    public class NinanWMissileTwo : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
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
			string Sound;
            //PlayAnimation(owner, "Spell1");
			switch (owner.SkinID)
            {
				case 5:
                    Sound = "Play_sfx_TalonSkin05_TalonWMissileTwo_OnMissileLaunch";
                    break;
					
                case 12:
                    Sound = "Play_sfx_TalonSkin12_TalonWMissileTwo_OnMissileLaunch";
                    break;

                case 20:
                    Sound = "Play_sfx_TalonSkin20_TalonWMissileTwo_OnMissileLaunch";
                    break;
				case 29:
                    Sound = "Play_sfx_TalonSkin29_TalonWMissileTwo_OnMissileLaunch";
                    break;
				case 38:
                    Sound = "Play_sfx_TalonSkin38_TalonWMissileTwo_OnMissileLaunch";
                    break;
				case 39:
                    Sound = "Play_sfx_TalonSkin39_TalonWMissileTwo_OnMissileLaunch";
                    break;

                default:
                    Sound = "Play_sfx_Talon_TalonWMissileTwo_OnMissileLaunch";
                    break;
            }	
			PlaySound(Sound, owner);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            if (target == missile.CastInfo.Owner)
            {
				missile.SetToRemove();
            }
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("NinanW").CastInfo.SpellLevel;
            var ADratio = (owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue) * 0.6f;
            var damage = 70 + 15f * (spellLevel - 1) + ADratio;
            if (!UnitsHit.Contains(target))
            {
				if (target.Team != owner.Team && !(target is IObjBuilding || target is IBaseTurret))
				{
                UnitsHit.Add(target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddBuff("TalonWSlow", 2f, 1, spell, target, owner);
				PlaySound("Play_sfx_Talon_TalonWMissileTwo_OnHit", owner);
                if (!target.HasBuff("NinanPassiveCooldown")&&!target.HasBuff("NinanPassiveBleed"))
                {     
			    AddBuff("NinanPassiveStack", 6f, 1, spell, target, owner,false);
			    }
                else
			    {
			    }	
                AddParticleTarget(owner, target, "Talon_Base_W_Tar_return.troy", target, 1f);
				}
            }
        }
        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			PlaySound("Stop_sfx_Talon_TalonWMissileTwo_OnMissileLaunch", owner);
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
