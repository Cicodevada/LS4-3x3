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
using static GameServerCore.Domain.GameObjects.IGameObject;
using GameServerCore;
using LeagueSandbox.GameServer.GameObjects.Stats;

namespace Spells
{
    public class NinanR : ISpellScript
    {
		float timeSinceLastTick = 1000f;
		public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
		IBuff HandlerBuff;
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
            var owner = spell.CastInfo.Owner as IChampion;
			PlaySound("Play_vo_Talon_TalonShadowAssault_OnCast", owner);
			if (owner.SkinID == 5)
			{
			PlaySound("Play_sfx_TalonSkin05_TalonR_OnCast", owner);
			}
			else
			{
			PlaySound("Play_sfx_Talon_TalonR_OnCast", owner);
			}
            PlayAnimation(owner, "Spell4");
			AddBuff("NinanR", 2.5f, 1, spell, owner, owner, false);
			AddBuff("NinanRDummy", 3f, 1, spell, owner, owner, false);
            for (int bladeCount = 0; bladeCount <= 11; bladeCount++)
            {				           
                var start = GetPointFromUnit(owner, 25f, bladeCount * 30f);
				var end = GetPointFromUnit(owner, 615f, bladeCount * 30f);
				SpellCast(owner, 3, SpellSlotType.ExtraSlots, end, end, true, Vector2.Zero);
			    SpellCast(owner, 5, SpellSlotType.ExtraSlots, end, end, true, Vector2.Zero);
				//SpellCast(owner, 3, SpellSlotType.ExtraSlots, end, Vector2.Zero, true, start);
				//SpellCast(owner, 5, SpellSlotType.ExtraSlots, end, Vector2.Zero, true, start);			            			
            }			
            //AddParticleTarget(owner, owner, "Talon_Base_R_Cas.troy", owner, 10f);
        }
		public void SetSpellToggle(bool toggle)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            spell.SetCooldown(0.8f, true);
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

    public class NinanRMisOne : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true
        };
		ISpell Spell;
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
			Spell = spell;
            UnitsHit.Clear();
			var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });

            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }

        public void OnMissileEnd(ISpellMissile missile)
        {
			var owner = missile.CastInfo.Owner;
            IMinion Blade = AddMinion(owner, "TestCubeRender10Vision", "TestCubeRender10Vision", missile.Position, owner.Team, owner.SkinID, true, false);
			AddBuff("NinanRMisBuff", 2.24f, 1, Spell, Blade, Blade, false);
			//AddBuff("TalonShadowAssault", 2.24f, 1, Spell, Blade, Blade, false);			
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ADratio = (owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue) * 0.8f;
            var damage = 90 + 45f * (owner.GetSpell(3).CastInfo.SpellLevel - 1) + ADratio;
            if (!UnitsHit.Contains(target) && target != spell.CastInfo.Owner)
            {
				if (target.Team != owner.Team && !(target is IObjBuilding || target is IBaseTurret))
				{
                UnitsHit.Add(target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
				if (!target.HasBuff("NinanPassiveCooldown")&&!target.HasBuff("NinanPassiveBleed"))
                {     
			    AddBuff("NinanPassiveStack", 6f, 1, spell, target, owner,false);
			    }
                else
			    {
			    }	
                AddParticleTarget(owner, target, "Talon_Base_R_Tar.troy", target, 1f);
				}
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
    public class NinanRMisTwo : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true
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
        }
     
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
			if (target == missile.CastInfo.Owner)
            {
				//missile.SetToRemove();            
            }
            var owner = spell.CastInfo.Owner;         
            var ADratio = (owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue) * 0.8f;
            var damage = 90 + 45f * (owner.GetSpell(3).CastInfo.SpellLevel - 1) + ADratio;

            if (!UnitsHit.Contains(target) && target != spell.CastInfo.Owner)
            {
				if (target.Team != owner.Team && !(target is IObjBuilding || target is IBaseTurret))
				{
                UnitsHit.Add(target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
				if (!target.HasBuff("NinanPassiveCooldown")&&!target.HasBuff("NinanPassiveBleed"))
                {     
			    AddBuff("NinanPassiveStack", 6f, 1, spell, target, owner,false);
			    }
                else
			    {
			    }	
                AddParticleTarget(owner, target, "Talon_Base_R_Tar_return.troy", target, 1f);
				}
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
	public class NinanRToggle : ISpellScript
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

