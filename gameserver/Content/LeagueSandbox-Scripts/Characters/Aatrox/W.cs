using System.Collections.Generic;
using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class AatroxW : ISpellScript
    {
        ISpell Spell;
        IObjAiBase Owner;
		int counter;
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            Spell = spell;
            Owner = spell.CastInfo.Owner;
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, true);
			ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnWLevelUp, true);
        }
        public void OnLevelUp (ISpell spell)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, spell.CastInfo.Owner, OnLaunchAttack, false);
        }
		public void OnWLevelUp (ISpell spell)
        {
			AddBuff("AatroxWLife", 25000f, 1, Spell, Owner, Owner);
			CreateTimer(0.1f, () =>
            {
			ApiEventManager.OnLevelUpSpell.RemoveListener(this);
			});
        }
        public void OnLaunchAttack(ISpell spell)
        {
			if (!Owner.HasBuff("AatroxWPower"))
            {
				counter++;      
                if (counter == 2)
                {
                AddBuff("AatroxWONHLifeBuff", 25000f, 1, Spell, Owner, Owner);
                }
				if (counter == 3)
                {              
                counter = 0;
                }
            }
			else
			{
				counter++;      
                if (counter == 2)
                {
                AddBuff("AatroxWONHPowerBuff", 25000f, 1, Spell, Owner, Owner);
                }
				if (counter == 3)
                {              
                counter = 0;
                }
			}
        }
        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
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
	public class AatroxW2 : ISpellScript
    {
        ISpell Spell;
        IObjAiBase Owner;
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
		}
        public void OnLevelUp (ISpell spell)
        {
        }
        public void OnHitUnit(IAttackableUnit target, bool isCrit)
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
	public class AatroxWONHAttackLife : ISpellScript
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
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
        }
        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			Target = target;
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
	public class AatroxWONHAttackPower : ISpellScript
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
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
        }
        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			Target = target;
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