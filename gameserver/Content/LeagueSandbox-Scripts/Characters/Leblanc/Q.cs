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

namespace Spells
{
    public class LeblancChaosOrb : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            },
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
			if (!owner.HasBuff("LeblancSlideM")&&owner.HasBuff("LeblancMimic"))
             {
             owner.SetSpell("LeblancChaosOrbM", 3, true);
             }			 
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
			var QLevel = owner.GetSpell(0).CastInfo.SpellLevel;
			var RLevel = owner.GetSpell(3).CastInfo.SpellLevel;
            var AP = owner.Stats.AbilityPower.Total * 0.4f;
			var MAXAP = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.65f;
			var damagemax=100 + 100f*(RLevel - 1)+ MAXAP;
            var damage = 55 + 25f*(QLevel - 1) + AP;
			var QMarkdamage = damage * 2f;
			var RQMarkdamage = damage + damagemax;
            if (target.HasBuff("LeblancChaosOrb"))
            {
				target.TakeDamage(owner, QMarkdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
				target.RemoveBuffsWithName("LeblancChaosOrb");
				AddBuff("LeblancChaosOrb", 3.5f, 1, spell, target, owner);
            }
			else if (target.HasBuff("LeblancChaosOrbM"))
            {
				target.TakeDamage(owner, RQMarkdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
			    target.RemoveBuffsWithName("LeblancChaosOrbM");
				AddBuff("LeblancChaosOrb", 3.5f, 1, spell, target, owner);
            }
			else
			{
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
			}
			AddParticleTarget(owner, target, "LeBlanc_Base_Q_tar", target);
            AddBuff("LeblancChaosOrb", 3.5f, 1, spell, target, owner);
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
