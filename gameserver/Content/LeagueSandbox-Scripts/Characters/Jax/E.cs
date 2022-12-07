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
    public class JaxCounterStrike: ISpellScript
    {
        IObjAiBase Owner;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            Owner = owner;
			if (!owner.HasBuff("JaxCounterStrike"))
		    {
			spell.SetCooldown(1f, true);
			}
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			if (!owner.HasBuff("JaxCounterStrike"))
		    {
			spell.SetCooldown(1f, true);
			}
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            if (!owner.HasBuff("JaxCounterStrike"))
		    {
            AddBuff("JaxCounterStrike", 2f, 1, spell, Owner, Owner, false);
            }
			else
			{
			RemoveBuff(Owner, "JaxCounterStrike");			
			}
        }

        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			if (!owner.HasBuff("JaxCounterStrike"))
		    {
			spell.SetCooldown(1f, true);
			}
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