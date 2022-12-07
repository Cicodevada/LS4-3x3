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

namespace Spells
{
    public class JaxRelentlessAssault : ISpellScript
    {
        IObjAiBase Owner;
        ISpell Spell;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            Owner = owner;
            Spell = spell;

            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUpSpell, true);
        }
        public void OnLevelUpSpell(ISpell spell)

        {
            var Owner = spell.CastInfo.Owner;
             ApiEventManager.OnLaunchAttack.AddListener(this, Owner, OnLaunchAttack, false);
        }
        public void OnLaunchAttack(ISpell spell)
        {
           AddBuff("JaxRelentlessAttack", 3f, 1, Spell, Owner, Owner);
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

            AddBuff("JaxRelentlessAssaultAS", 8f, 1, spell, Owner, Owner, false);
            //AddParticleTarget(owner, owner, "JaxRelentlessAssault_buf.troy", owner, 8f, 1f);
            
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
	public class JaxRelentlessAttack : ISpellScript
    {
        IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
			TriggersSpellCasts = true,
            NotSingleTargetSpell = true,
			IsDamagingSpell = true,
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
			Target = target;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnLaunchAttack(ISpell spell)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
            var AP = owner.Stats.AbilityPower.Total * 0.7f;
            var target = spell.CastInfo.Targets[0].Unit;
            float damage = 60 + 40 * owner.GetSpell(3).CastInfo.SpellLevel + AP;          
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
			AddParticleTarget(owner, target, "RelentlessAssault_tar.troy", target, 1f);
            AddParticleTarget(owner, target, ".troy", target, 1f);
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