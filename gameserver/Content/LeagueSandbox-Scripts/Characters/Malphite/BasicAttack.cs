using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Collections.Generic;


namespace Spells
{
    public class MalphiteBasicAttack : ISpellScript
    {
		private IAttackableUnit Target = null;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
			ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
          Target = target;			
		  if (owner.HasBuff("ObduracyAttack"))
			{
				OverrideAnimation(owner, "Spell2", "Attack1");
			}
			else
			{
				OverrideAnimation(owner, "Attack1", "Spell2");
			}
        }

        public void OnLaunchAttack(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			float ad = owner.Stats.AttackDamage.Total * 0.6f;
            float damage = 15 + 15 * owner.GetSpell(1).CastInfo.SpellLevel + ad;
			if (owner.HasBuff("ObduracyAttack"))
            {
			    spell.CastInfo.Owner.TargetUnit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
			    AddParticleTarget(owner, spell.CastInfo.Owner.TargetUnit, "Malphite_Base_CleaveEnragedHit.troy", owner);
				AddParticleTarget(owner, spell.CastInfo.Owner.TargetUnit, "Malphite_Base_CleaveHit.troy", owner);
			}
			else
			{
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

    public class MalphiteBasicAttack2 : ISpellScript
    {
		private IAttackableUnit Target = null;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true

            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
			ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
		  Target = target;
          if (owner.HasBuff("ObduracyAttack"))
			{
				OverrideAnimation(owner, "Spell2", "Attack2");
			}
			else
			{
				OverrideAnimation(owner, "Attack2", "Spell2");
			}
        }

        public void OnLaunchAttack(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
			float ad = owner.Stats.AttackDamage.Total * 0.6f;
            float damage = 15 + 15 * owner.GetSpell(1).CastInfo.SpellLevel + ad;
			if (owner.HasBuff("ObduracyAttack"))
            {
			    spell.CastInfo.Owner.TargetUnit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
			    AddParticleTarget(owner, spell.CastInfo.Owner.TargetUnit, "Malphite_Base_CleaveEnragedHit.troy", owner);
				AddParticleTarget(owner, spell.CastInfo.Owner.TargetUnit, "Malphite_Base_CleaveHit.troy", owner);
			}
			else
			{
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
	public class MalphiteCritAttack : ISpellScript
    {
		private IAttackableUnit Target = null;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
			ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        { 
          Target = target;		
		  if (owner.HasBuff("ObduracyAttack"))
			{
				OverrideAnimation(owner, "Spell2", "Crit");
			}
			else
			{
				OverrideAnimation(owner, "Crit", "Spell2");
			}
        }

        public void OnLaunchAttack(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			float ad = owner.Stats.AttackDamage.Total * 0.6f;
            float damag = 15 + 15 * owner.GetSpell(1).CastInfo.SpellLevel + ad;
			float damage = damag * 2f;
			if (owner.HasBuff("ObduracyAttack"))
            {
			    spell.CastInfo.Owner.TargetUnit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
			    AddParticleTarget(owner, spell.CastInfo.Owner.TargetUnit, "Malphite_Base_CleaveEnragedHit.troy", owner);
				AddParticleTarget(owner, spell.CastInfo.Owner.TargetUnit, "Malphite_Base_CleaveHit.troy", owner);
			}
			else
			{
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
}

