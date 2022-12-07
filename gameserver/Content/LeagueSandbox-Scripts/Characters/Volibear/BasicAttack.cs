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
    public class VolibearBasicAttack : ISpellScript
    {
		private IAttackableUnit Target = null;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
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
			if (owner.HasBuff("VolibearQ"))
            {
				OverrideAnimation(owner, "spell1_attack", "Attack1");
			}
			else
			{
				OverrideAnimation(owner, "Attack1", "spell1_attack");
			}
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnLaunchAttack(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			var spellLevel = owner.GetSpell("VolibearQ").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.3f;
            var damage =(30 * spellLevel) + ADratio;
			if (owner.HasBuff("VolibearQ"))
            {
			    Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
				AddParticleTarget(owner, Target, "Volibear_Q_tar", Target, 10f,1,"");
				if (Target.Team != owner.Team && !(Target is IObjBuilding || Target is IBaseTurret))
				{
				ForceMovement(Target, "RUN", GetPointFromUnit(owner, -125f), 400f, 0, 25f, 0);
				}
			}
			else
			{
			}
			//spell.CastInfo.Owner.SetAutoAttackSpell("TalonBasicAttack2", false);
        }

        public void OnSpellCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
        }

        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
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

    public class VolibearBasicAttack2 : ISpellScript
    {
		private IAttackableUnit Target = null;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
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
			if (owner.HasBuff("VolibearQ"))
            {
				OverrideAnimation(owner, "spell1_attack", "Attack2");
			}
			else
			{
				OverrideAnimation(owner, "Attack2", "spell1_attack");
			}
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnLaunchAttack(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			var spellLevel = owner.GetSpell("VolibearQ").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.3f;
            var damage =(30 * spellLevel) + ADratio;
			if (owner.HasBuff("VolibearQ"))
            {
			    Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
				AddParticleTarget(owner, Target, "Volibear_Q_tar", Target, 10f,1,"");
			    if (Target.Team != owner.Team && !(Target is IObjBuilding || Target is IBaseTurret))
				{
				ForceMovement(Target, "RUN", GetPointFromUnit(owner, -125f), 400f, 0, 25f, 0);
				}
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
	public class VolibearCritAttack : ISpellScript
    {
		private IAttackableUnit Target = null;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
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
			if (owner.HasBuff("VolibearQ"))
            {
				OverrideAnimation(owner, "spell1_attack", "Crit");
			}
			else
			{
				OverrideAnimation(owner, "Crit", "spell1_attack");
			}
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnLaunchAttack(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			var spellLevel = owner.GetSpell("VolibearQ").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.3f;
            var damage =(30 * spellLevel) + ADratio;
			var damager =damage * 2f;
			if (owner.HasBuff("VolibearQ"))
            {
			    Target.TakeDamage(owner, damager, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, true);
				AddParticleTarget(owner, Target, "Volibear_Q_tar", Target, 10f,1,"");
			    if (Target.Team != owner.Team && !(Target is IObjBuilding || Target is IBaseTurret))
				{
				ForceMovement(Target, "RUN", GetPointFromUnit(owner, -125f), 400f, 0, 25f, 0);
				}
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
