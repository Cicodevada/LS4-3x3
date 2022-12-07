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
    public class NinanBasicAttack : ISpellScript
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
			PlaySound("Play_vo_Talon_TalonBasicAttack_OnCast", owner);
			PlaySound("Play_sfx_Talon_TalonBasicAttack_OnCast", owner);
			if (Target.HasBuff("NinanPassiveBleed"))
			{
				OverrideAnimation(owner, "Crit", "Attack1");
			}
			else
			{
				OverrideAnimation(owner, "Attack1", "Crit");
			}
        }

        public void OnLaunchAttack(ISpell spell)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
        }

        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			PlaySound("Play_sfx_Talon_TalonBasicAttack_OnHit", Target);
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

    public class NinanBasicAttack2 : ISpellScript
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
			PlaySound("Play_vo_Talon_TalonBasicAttack2_OnCast", owner);
			PlaySound("Play_sfx_Talon_TalonBasicAttack2_OnCast", owner);
			if (Target.HasBuff("NinanPassiveBleed"))
			{
				OverrideAnimation(owner, "Crit", "Attack2");
			}
			else
			{
				OverrideAnimation(owner, "Attack2", "Crit");
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
			PlaySound("Play_sfx_Talon_TalonBasicAttack2_OnHit", Target);
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
	public class NinanCritAttack : ISpellScript
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
			PlaySound("Play_vo_Talon_TalonCritAttack_OnCast", owner);
			PlaySound("Play_sfx_Talon_TalonCritAttack_OnCast", owner);
        }

        public void OnLaunchAttack(ISpell spell)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
			PlaySound("Play_sfx_Talon_TalonCritAttack_OnHit", Target);
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
