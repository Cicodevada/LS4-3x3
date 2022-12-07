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
    public class BurningAgony : ISpellScript
    {
        IObjAiBase Owner;
        float timeSinceLastTick = 1000f;
        ISpell Spell;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            Owner = owner;
            Spell = spell;
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
            if (!Owner.HasBuff("BurningAgony"))
            {
                AddBuff("BurningAgony", 1, 1, spell, Owner, Owner, true);
                timeSinceLastTick = 1000f;
                spell.SetCooldown(0.5f, true);
            }
            else
            {
                RemoveBuff(Owner, "BurningAgony");
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
            timeSinceLastTick += diff;
            if (timeSinceLastTick >= 1000.0f && Owner != null && Spell != null && Owner.HasBuff("BurningAgony"))
            {
                float selfDMG = 5f + (5f * Spell.CastInfo.SpellLevel);
                if (Owner.Stats.CurrentHealth > selfDMG)
                {
                    Owner.Stats.CurrentHealth -= selfDMG;
                }
                else
                {
                    Owner.Stats.CurrentHealth = 1f;
                }
                timeSinceLastTick = 0f;
            }
        }
    }
}
