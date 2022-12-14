using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class NautilusAnchorDrag : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
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
            FaceDirection(end, owner);
        }

        public void OnSpellCast(ISpell spell)
        {
			var endPos = GetPointFromUnit(spell.CastInfo.Owner, 925);
            SpellCast(spell.CastInfo.Owner, 0, SpellSlotType.ExtraSlots, endPos, endPos, false, Vector2.Zero);
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

    public class NautilusAnchorDragMissile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            }
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
            // Prevent flash during cast.
            SealSpellSlot(spell.CastInfo.Owner, SpellSlotType.SummonerSpellSlots, 0, SpellbookType.SPELLBOOK_SUMMONER, true);
            SealSpellSlot(spell.CastInfo.Owner, SpellSlotType.SummonerSpellSlots, 1, SpellbookType.SPELLBOOK_SUMMONER, true);
        }

        public void OnSpellPostCast(ISpell spell)
        {
            SealSpellSlot(spell.CastInfo.Owner, SpellSlotType.SummonerSpellSlots, 0, SpellbookType.SPELLBOOK_SUMMONER, false);
            SealSpellSlot(spell.CastInfo.Owner, SpellSlotType.SummonerSpellSlots, 1, SpellbookType.SPELLBOOK_SUMMONER, false);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("NautilusAnchorDrag").CastInfo.SpellLevel;
            var ap = owner.Stats.AbilityPower.Total;
            var damage = 80 + (spellLevel - 1) * 55 + ap;

            var dist = System.Math.Abs(Vector2.Distance(target.Position, owner.Position));
            var time = dist / 1350f;
            if (target is IObjBuilding || target is IBaseTurret){AddBuff("Stun", 0.6f, 1, spell, owner, owner);}
            // Grab particle
            AddBuff("RocketGrab", time, 1, spell, target, owner);

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

            AddBuff("Stun", 0.6f, 1, spell, target, owner);

            missile.SetToRemove();

            // SetStatus & auto attack
            AddBuff("RocketGrab2", 0.6f, 1, spell, target, owner);
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

