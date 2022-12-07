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
    public class MaokaiTrunkLine : ISpellScript
    {
        IObjAiBase Owner;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
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

        }

        public void OnSpellPostCast(ISpell spell)
        {
            if (spell.CastInfo.Owner is IChampion c)
            {
				var ownerSkinID = c.SkinID;
				var start = GetPointFromUnit(c, 125f);
			    var end = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
			    FaceDirection(end, c);
			    SpellCast(c, 0, SpellSlotType.ExtraSlots, end, Vector2.Zero, true, start);
                AddParticleTarget(c, c, "Maokai_Base_Q_Cas.troy", c);
                var AD = c.Stats.AttackDamage.Total * 0.6f;
                var damage = 40 + spell.CastInfo.SpellLevel * 30 + AD; 

                var units = GetUnitsInRange(c.Position, 250f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
                        units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                        AddParticleTarget(c, units[i], "Maokai_Base_Q_Tar_AoE.troy", units[i]);    
                    }
                }
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

    public class MaokaiTrunkLineMissile : ISpellScript
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
			var ownerSkinID = owner.SkinID;
            float ad = owner.Stats.AttackDamage.Total;
            float damage = 75 + (spell.CastInfo.SpellLevel - 1) * 40 + ad;
            if(missile is ISpellCircleMissile circleMissle && circleMissle.ObjectsHit.Count > 1)
            {
                damage *= 0.6f;
            }

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            AddParticleTarget(owner, target, "Maokai_Base_Q_Tar.troy", target);            
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
