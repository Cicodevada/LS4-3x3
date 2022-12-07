using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using System.Collections.Generic;
using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using System;

namespace Spells
{
    public class RivenFengShuiEngine : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        string pcastname;
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
        }

        public void OnSpellPostCast(ISpell spell)
        {
			spell.SetCooldown(0.5f, true);	
            var owner = spell.CastInfo.Owner;
			AddBuff("RivenR2", 15f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);
            AddBuff("RivenFengShuiEngine", 15f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);			
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
    public class RivenIzunaBlade : ISpellScript
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
            //PlayAnimation(owner, "Spell1");
        }

        public void OnSpellCast(ISpell spell)
        {          
        }

        public void OnSpellPostCast(ISpell spell)
        {
             var owner = spell.CastInfo.Owner as IChampion;               		 
			 var truePos = GetPointFromUnit(owner, 1000f);
			 var targetPos = GetPointFromUnit(owner, 900f, -13f);
			 var Pos = GetPointFromUnit(owner, 900f, 13f);
			 SpellCast(owner, 5, SpellSlotType.ExtraSlots, truePos, truePos, true, Vector2.Zero);
			 SpellCast(owner, 5, SpellSlotType.ExtraSlots, targetPos, targetPos, true, Vector2.Zero);
             SpellCast(owner, 5, SpellSlotType.ExtraSlots, Pos, Pos, true, Vector2.Zero);		
			 //SpellCast(owner, 3, SpellSlotType.ExtraSlots, targetPos, targetPos, true, Vector2.Zero);
             //SpellCast(owner, 3, SpellSlotType.ExtraSlots, Pos, Pos, true, Vector2.Zero);				 
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

    public class RivenLightsaberMissile : ISpellScript
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
			string particles;
			string particles2;
			string particles3;
            string particles4;			
			switch ((owner as IObjAiBase).SkinID)
            {
                case 3:
                    particles = "exile_ult_mis_tar.troy";
					particles2 = "exile_ult_mis_tar_minion.troy";
                    break;

                case 4:
                    particles = "exile_ult_mis_tar.troy";
					particles2 = "exile_ult_mis_tar_minion.troy";
                    break;
				case 5:
                    particles = "Riven_Skin05_R_Tar.troy";
					particles2 = "Riven_Skin05_R_Tar_Minion.troy";
                    break;

                default:
                    particles = "Riven_Base_R_Tar.troy";
					particles2 = "Riven_Base_R_Tar_Minion.troy";
                    break;
		    }
            var spellLevel = owner.GetSpell("RivenMartyr").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.6f;
            var damage = 30 + 25f*(spellLevel - 1) + ADratio;         
            if (!UnitsHit.Contains(target))
            {
                UnitsHit.Add(target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(owner, target, "Riven_Base_R_Tar.troy", target, 1f);
				AddParticleTarget(owner, target, "Riven_Base_R_Tar_Minion.troy", target, 1f);
            }
        }

        public void OnMissileEnd(ISpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
            //SpellCast(owner, 2, SpellSlotType.ExtraSlots, owner.Position, owner.Position, true, missile.Position);
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
	public class RivenLightsaberMissileSide : ISpellScript
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
            var spellLevel = owner.GetSpell("RivenMartyr").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.6f;
            var damage = 30 + 25f*(spellLevel - 1) + ADratio;         
            if (!UnitsHit.Contains(target))
            {
                UnitsHit.Add(target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddParticleTarget(owner, target, "Riven_Base_R_Tar.troy", target, 1f);
				AddParticleTarget(owner, target, "Riven_Base_R_Tar_Minion.troy", target, 1f);
            }
        }

        public void OnMissileEnd(ISpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
            //SpellCast(owner, 2, SpellSlotType.ExtraSlots, owner.Position, owner.Position, true, missile.Position);
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
