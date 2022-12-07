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
    public class BlindMonkEOne : ISpellScript
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
        }

        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
            var sector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 450f,
                SingleTick = true,
                Type = SectorType.Area
            });
			AddParticleTarget(owner, owner, "blindMonk_E_cas.troy", owner, 10f,1f);
			AddParticleTarget(owner, owner, "blindMonk_thunderCrash_impact_02.troy", owner, 10f,1f);
			AddParticleTarget(owner, owner, "blindMonk_thunderCrash_impact_cas.troy", owner, 10f,1f);
			AddParticleTarget(owner, owner, "blindMonk_E_mis_01.troy", owner, 10f,1f,"L_HAND");
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            if (!(target is IBaseTurret || target is ILaneTurret || target.Team == owner.Team || target == owner))
            {
                var AP = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.3f;
                var AD = spell.CastInfo.Owner.Stats.AttackDamage.Total * 0.6f;
                var damage = 40 + spell.CastInfo.SpellLevel * 30 + AP + AD;           
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
				AddBuff("BlindMonkEOne", 3f, 1, spell, target, owner);
				if (target is IObjAiBase ai)
                        {
                            AddBuff("BlindMonkEManager", 3f, 1, spell, owner, ai);
                        }             
                AddParticleTarget(owner, target, "blindMonk_E_thunderCrash_tar.troy", target, 10f);
				AddParticleTarget(owner, target, "blindMonk_thunderCrash_impact_unit_tar.troy", target, 10f,1f);
				AddParticleTarget(owner, target, "blindmonk_resonatingstrike_tar_sound.troy", target, 10f);
				AddParticleTarget(owner, target, "blindMonk_E_thunderCrash_unit_tar_blood.troy", target, 10f);
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
	public class BlindMonkETwo : ISpellScript
    {
		string checkBuffName = "BlindMonkEOne";
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
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
			string buffName = "BlindMonkEOne";
			var owner = spell.CastInfo.Owner;
			foreach (IAttackableUnit Unit in GetUnitsInRange(owner.Position, 250000f, true))
            {
            var buff = Unit.GetBuffWithName(buffName);
                    if (buff != null && buff.SourceUnit == owner)
                    {
                        if (Unit is IObjAiBase ai)
                        {
							AddBuff("BlindMonkETwo", 1f, 1, spell, owner, ai);
                        }
                        RemoveBuff(owner, "BlindMonkEManager");
                    }	
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
	public class BlindMonkETwoMissile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
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

        public void OnSpellCast(ISpell spell)
        {	 
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
			AddBuff("BlindMonkETwoMissile", 4f, 1, spell, target, owner);	
            missile.SetToRemove();
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
