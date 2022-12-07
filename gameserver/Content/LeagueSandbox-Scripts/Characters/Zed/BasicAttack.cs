using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using System.Collections.Generic;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace Spells
{
    public class ZedBasicAttack : ISpellScript
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
			//ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
			float BBlood = target.Stats.HealthPoints.Total * 0.5f;
			float XBlood = target.Stats.CurrentHealth;
			if (BBlood >= XBlood && !Target.HasBuff("ZedPassiveToolTip") && Target.Team != owner.Team && !(Target is IObjBuilding || Target is IBaseTurret))
			{
				OverrideAnimation(owner, "attack_passive", "Attack1");
			}
			else
			{
				OverrideAnimation(owner, "Attack1", "attack_passive");
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
    public class ZedBasicAttack2 : ISpellScript
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
			//ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
			float BBlood = target.Stats.HealthPoints.Total * 0.5f;
			float XBlood = target.Stats.CurrentHealth;
			if (BBlood >= XBlood && !Target.HasBuff("ZedPassiveToolTip") && Target.Team != owner.Team && !(Target is IObjBuilding || Target is IBaseTurret))
			{
				OverrideAnimation(owner, "attack_passive", "Attack2");
			}
			else
			{
				OverrideAnimation(owner, "Attack2", "attack_passive");
			}
        }
        public void OnLaunchAttack(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
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
	public class ZedCritAttack : ISpellScript
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
			//ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
			float BBlood = target.Stats.HealthPoints.Total * 0.5f;
			float XBlood = target.Stats.CurrentHealth;
			if (BBlood >= XBlood && !Target.HasBuff("ZedPassiveToolTip") && Target.Team != owner.Team && !(Target is IObjBuilding || Target is IBaseTurret))
			{
				OverrideAnimation(owner, "attack_passive", "Crit");
			}
			else
			{
				OverrideAnimation(owner, "Crit", "attack_passive");
			}
        }
        public void OnLaunchAttack(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			float BBlood = Target.Stats.HealthPoints.Total * 0.5f;
			float XBlood = Target.Stats.CurrentHealth;
			if (BBlood >= XBlood && !Target.HasBuff("ZedPassiveToolTip") && Target.Team != owner.Team && !(Target is IObjBuilding || Target is IBaseTurret))
			{		
				AddBuff("ZedPassiveToolTip", 10f, 1, spell, Target, owner);
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

