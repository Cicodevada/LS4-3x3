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
    public class MasterYiBasicAttack : ISpellScript
    {
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
			if (owner.HasBuff("MasterYiDoubleStrike"))
            {
				OverrideAnimation(owner, "Passive", "Attack1");
			}
			else
			{
				OverrideAnimation(owner, "Attack1", "Passive");
			}
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnLaunchAttack(ISpell spell)
        {
            //spell.CastInfo.Owner.SetAutoAttackSpell("MasterYiBasicAttack2", false);
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
    public class MasterYiBasicAttack2 : ISpellScript
    {
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
			if (owner.HasBuff("MasterYiDoubleStrike"))
            {
				OverrideAnimation(owner, "Passive", "Attack2");
			}
			else
			{
				OverrideAnimation(owner, "Attack2", "Passive");
			}
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, true);
        }

        public void OnLaunchAttack(ISpell spell)
        {
            //spell.CastInfo.Owner.SetAutoAttackSpell("MasterYiBasicAttack", false);
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
	public class MasterYiDoubleStrike : ISpellScript
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
            //spell.CastInfo.Owner.SetAutoAttackSpell("MasterYiBasicAttack", false); 
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {	
            var owner = spell.CastInfo.Owner;			
			var ADratio = owner.Stats.AttackDamage.Total * 0.5f;
			CreateTimer((float) 0.25 , () =>
            {
			Target.TakeDamage(owner, ADratio, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
			});	
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
	public class MasterYiCritAttack : ISpellScript
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
			if (owner.HasBuff("MasterYiDoubleStrike"))
            {
				OverrideAnimation(owner, "Passive", "Crit");
			}
			else
			{
				OverrideAnimation(owner, "Crit", "Passive");
			}
        }

        public void OnLaunchAttack(ISpell spell)
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

