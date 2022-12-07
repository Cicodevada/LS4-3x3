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
using GameServerCore;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System.Linq;

namespace Spells
{
    public class BlindMonkWOne : ISpellScript
    {
        public static IAttackableUnit Target = null;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            //TriggersSpellCasts = true,
            //IsDamagingSpell = true
        };
        IObjAiBase owner;
        public void OnActivate(IObjAiBase owner, ISpell spell)
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
		   if (owner != Target)
            {
		    AddBuff("BlindMonkWOne", 3f, 1, spell, owner, owner);
            AddBuff("BlindMonkWOneDash", 3f, 1, spell, owner, owner);			
            AddBuff("BlindMonkWOneShield", 3f, 1, spell, owner, owner);		
			AddBuff("BlindMonkWOneShield", 3f, 1, spell, Target, owner);
			}
			else
			{
		    AddBuff("BlindMonkWOne", 3f, 1, spell, owner, owner);			
            AddBuff("BlindMonkWOneShield", 3f, 1, spell, owner, owner);
			}
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
	public class BlindMonkWTwo : ISpellScript
    {
        IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
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
			AddBuff("BlindMonkWTwo", 4f, 1, spell, owner, owner);
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