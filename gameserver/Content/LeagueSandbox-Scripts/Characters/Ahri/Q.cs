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
    public class AhriOrbofDeception : ISpellScript
    {
        IObjAiBase Owner;
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
        }
        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var ownerSkinID = owner.SkinID;
            var targetPos = GetPointFromUnit(owner, 900f);
            FaceDirection(targetPos, owner);
            SpellCast(owner, 0, SpellSlotType.ExtraSlots, targetPos, targetPos, true, Vector2.Zero);
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

    public class AhriOrbMissile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
        };
		ISpell Spell;
		IAttackableUnit T;
		IObjAiBase I;
        public static List<IAttackableUnit> UnitsHit = new List<IAttackableUnit>();

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			I = owner;
			T = target;
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
            var owner = spell.CastInfo.Owner;
			var ownerSkinID = owner.SkinID;
            float ad = owner.Stats.AttackDamage.Total;
            float damage = 75 + (spell.CastInfo.SpellLevel - 1) * 40 + ad;         
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            AddParticleTarget(owner, target, ".troy", target);
        }
		public void OnMissileEnd(ISpellMissile missile)
        {
			var owner = missile.CastInfo.Owner;
            SpellCast(owner, 1, SpellSlotType.ExtraSlots, true, owner, missile.Position);
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

    public class AhriOrbReturn : ISpellScript
    {
		float T;
		ISpell S;
		ISpellMissile m;
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
			S = spell;
            UnitsHit.Clear();
			var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,              
            });	
            m = missile;			
			AddParticle(owner, missile, "Ekko_Base_Q_Aoe_Resolve", missile.Position);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
			var ownerSkinID = owner.SkinID;
            float ad = owner.Stats.AttackDamage.Total;
            float damage = 75 + (spell.CastInfo.SpellLevel - 1) * 40 + ad;        
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
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
