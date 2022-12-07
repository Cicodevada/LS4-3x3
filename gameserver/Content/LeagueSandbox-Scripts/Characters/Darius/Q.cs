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
    public class DariusCleave : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            NotSingleTargetSpell = true,
            IsDamagingSpell = true,
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
			spell.CreateSpellSector(new SectorParameters
            {
                Length = 400f,
                SingleTick = true,
                Type = SectorType.Area,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes
            });

        }

        public void OnSpellCast(ISpell spell)
        {
			
        }

        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			PlayAnimation(owner, "Spell1", 0.5f);
            AddParticleTarget(owner, owner, "darius_Base_Q_aoe_cast.troy", owner, bone:"C_BuffBone_Glb_Center_Loc");
			AddParticleTarget(owner, owner, "darius_Base_Q_aoe_cast_mist.troy", owner, bone:"C_BuffBone_Glb_Center_Loc");
            
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("DariusCleave").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.75f;
            var damage = 90 + 20f*(spellLevel - 1) + ADratio;
			var Blood = owner.Stats.CurrentHealth;
			var Health = owner.Stats.HealthPoints.Total;
			var Live = (Health - Blood) * 0.15f;
			if (target.Team != owner.Team && !(target is IObjBuilding || target is IBaseTurret))
			{
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
			owner.Stats.CurrentHealth += Live;
			if (owner.HasBuff("DariusHemoVisual"))
			{
		    AddBuff("DariusHemo", 6.0f, 5, spell, target, owner);
			}
			else
			{
			AddBuff("DariusHemo", 6.0f, 1, spell, target, owner);
			}
            AddParticleTarget(owner, target, "darius_Base_Q_impact_spray.troy", target, 1f);
			AddParticleTarget(owner, target, "darius_Base_Q_tar.troy", target, 1f);
			AddParticleTarget(owner, target, "darius_Base_Q_tar_inner.troy", target, 1f);
			AddParticleTarget(owner, target,"darius_Base_Q_impact_spray.troy", target);
			AddParticleTarget(owner, target, "", target, 1f);
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
}