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
using System;


namespace Spells
{
    public class Deceive : ISpellScript
    {
        Vector2 teleportTo;
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
            teleportTo = new Vector2(end.X, end.Y);
            float targetPosDistance = Math.Abs((float)Math.Sqrt(Math.Pow(owner.Position.X - teleportTo.X, 2f) + Math.Pow(owner.Position.Y - teleportTo.Y, 2f)));
            FaceDirection(teleportTo, owner);
            teleportTo = GetPointFromUnit(owner, Math.Min(targetPosDistance, 400f));
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddParticle(owner, null, "JackintheboxPoof2.troy", owner.Position, 2f);
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            TeleportTo(spell.CastInfo.Owner, teleportTo.X, teleportTo.Y);
            AddBuff("Deceive", 3.5f, 1, spell, owner, owner);
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