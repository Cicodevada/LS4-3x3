using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using System;
using GameServerCore.Enums;

namespace Spells
{
    public class PantheonRJump : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            NotSingleTargetSpell = true,
            TriggersSpellCasts = true,
            ChannelDuration = 2f,
        };

        IObjAiBase Owner;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Owner = owner;
        }

        public void OnSpellCast(ISpell spell)
        {
            AddParticleTarget(Owner, Owner, "Pantheon_Base_R_cas.troy", Owner, flags: 0);

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
            var spellpos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            SpellCast(Owner, 1, SpellSlotType.ExtraSlots,spellpos, spellpos, false, Vector2.Zero);
            AddParticle(Owner, null, "Pantheon_Base_R_indicator_green.troy", spellpos, 2.7f);
        }

        public void OnUpdate(float diff)
        {
        }
    }



    public class PantheonRFall : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            NotSingleTargetSpell = true,
            TriggersSpellCasts = true,
            ChannelDuration = 2f,
        };

        IObjAiBase Owner;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Owner = owner;
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
            var spellpos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
            AddParticle(Owner, null, "Pantheon_Base_R_aoe_explosion.troy", spellpos);
            Owner.TeleportTo(spellpos.X, spellpos.Y);
			if (Owner is IObjAiBase c)
            {
                var units = GetUnitsInRange(c.Position, 550f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
					        var RLevel = c.GetSpell(3).CastInfo.SpellLevel;
							var damage = 200 + (100 * (RLevel - 1)) + (c.Stats.AbilityPower.Total * 1f);
                            units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
						    AddParticleTarget(c, units[i], "Malphite_Base_UnstoppableForce_stun.troy", units[i], 1f);
				            AddParticleTarget(c, units[i], ".troy", units[i], 1f);
                    }
                }      
            }

        }

        public void OnUpdate(float diff)
        {
        }
    }
}