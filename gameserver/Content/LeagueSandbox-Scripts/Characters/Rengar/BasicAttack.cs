using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using System.Collections.Generic;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace Spells
{
    public class RengarBasicAttack : ISpellScript
    {
		IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
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
			Target = target;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
			if (owner.HasBuff("RengarQBuff"))
            {
				OverrideAnimation(owner, "Spell1", "Attack1");
			}
			else
			{
				OverrideAnimation(owner, "Attack1", "Spell1");
			}
        }

        public void OnLaunchAttack(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			float QLevel = (owner.GetSpell(0).CastInfo.SpellLevel-1) * 0.05f;
			float damage = ((30 * owner.GetSpell(0).CastInfo.SpellLevel) + owner.Stats.AttackDamage.Total * QLevel);
            if (owner.HasBuff("RengarQBuff"))
            {
				Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
				if (!owner.HasBuff("RengarFerocityManager"))
                {
				    AddBuff("RengarManager", 8.0f, 1, spell, owner, owner);
                }
				AddParticleTarget(owner, Target, "Rengar_Base_Q_Tar.troy", owner); 
			}
			else if (owner.HasBuff("RengarQEmp"))
			{
                Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
				AddParticleTarget(owner, Target, "Rengar_Base_Q_Tar.troy", owner); 				
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
	public class RengarCritAttack : ISpellScript
    {
		IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
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
			Target = target;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
			if (owner.HasBuff("RengarQBuff"))
            {
				OverrideAnimation(owner, "Spell1", "Crit");
			}
			else
			{
				OverrideAnimation(owner, "Crit", "Spell1");
			}
        }

        public void OnLaunchAttack(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			float QLevel = (owner.GetSpell(0).CastInfo.SpellLevel-1) * 0.05f;
			float damage = ((30 * owner.GetSpell(0).CastInfo.SpellLevel) + owner.Stats.AttackDamage.Total * QLevel) * 2;
            if (owner.HasBuff("RengarQBuff"))
            {
				Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
				AddParticleTarget(owner, Target, "Rengar_Base_Q_Tar.troy", owner);
			    if (!owner.HasBuff("RengarFerocityManager"))
                {
				    AddBuff("RengarManager", 8.0f, 1, spell, owner, owner);
                }
			}
			else if (owner.HasBuff("RengarQEmp"))
			{
                Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
				AddParticleTarget(owner, Target, "Rengar_Base_Q_Tar.troy", owner); 				
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

