using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;

namespace Spells
{
    public class LucianE : ISpellScript
    {
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
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var trueCoords = GetPointFromUnit(spell.CastInfo.Owner, spell.SpellData.CastRangeDisplayOverride);

            FaceDirection(trueCoords, spell.CastInfo.Owner, true);
            ForceMovement(spell.CastInfo.Owner, "Spell3", trueCoords, 1350, 0, 0, 0, movementOrdersFacing: GameServerCore.Enums.ForceMovementOrdersFacing.KEEP_CURRENT_FACING);
            AddParticleTarget(spell.CastInfo.Owner, spell.CastInfo.Owner, "Lucian_E_cas_trail.troy", spell.CastInfo.Owner, 1f);

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
