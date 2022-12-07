using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using System.Linq;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class RiftWalk : ISpellScript
    {
        IBuff Buff;
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true
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

        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var trueCoords = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var startPos = owner.Position;

            var to = trueCoords - startPos;
            if (to.Length() > 700f)
            {
                trueCoords = GetPointFromUnit(owner, 475f);
            }
            PlayAnimation(owner, "Spell3", 0, 0, 1);
            AddBuff("RiftWalk", 20.0f, 1, spell, owner, owner);
            TeleportTo(owner, trueCoords.X, trueCoords.Y);
            AddParticle(owner, null, "Kassadin_Base_R_appear.troy", owner.Position);

            var AOEdmg = spell.CreateSpellSector(new SectorParameters
            {
                Length = 250f,
                SingleTick = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var buff = spell.CastInfo.Owner.GetBuffWithName("RiftWalk");
            float MANA = spell.CastInfo.Owner.Stats.ManaPoints.Total * 0.02f + (0.01f * buff.StackCount);
            float damage = 60f + 20f * spell.CastInfo.SpellLevel + MANA + (30f * spell.CastInfo.SpellLevel) * buff.StackCount;
            //TODO: Find a way to increase damage and ManaCost based on stacks

            target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
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