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
    public class EkkoBasicAttack : ISpellScript
    {
		IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
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
			if (target.HasBuff("EkkoPassiveSpellShieldCheck"))
			    {
                   OverrideAnimation(owner, "Attack_P_1", "Attack1");     
                }
           else
                {  
                   OverrideAnimation(owner, "Attack1", "Attack_P_1"); 			
                }				
        }

        public void OnLaunchAttack(ISpell spell)
        {
            //spell.CastInfo.Owner.SetAutoAttackSpell("NasusBasicAttack2", false);
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

    public class EkkoBasicAttack2 : ISpellScript
    {
        IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
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
			if (target.HasBuff("EkkoPassiveSpellShieldCheck"))
			    {
                   OverrideAnimation(owner, "Attack_P_2", "Attack2");     
                }
           else
                {  
                   OverrideAnimation(owner, "Attack2", "Attack_P_2"); 			
                }	
        }

        public void OnLaunchAttack(ISpell spell)
        {
            //spell.CastInfo.Owner.SetAutoAttackSpell("NasusBasicAttack", false);
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
	public class EkkoBasicAttack3 : ISpellScript
    {
        IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
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
			if (target.HasBuff("EkkoPassiveSpellShieldCheck"))
			    {
                   OverrideAnimation(owner, "Attack_P_3", "Attack3");     
                }
           else
                {  
                   OverrideAnimation(owner, "Attack3", "Attack_P_3"); 			
                }	
        }

        public void OnLaunchAttack(ISpell spell)
        {
            //spell.CastInfo.Owner.SetAutoAttackSpell("NasusBasicAttack", false);
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

