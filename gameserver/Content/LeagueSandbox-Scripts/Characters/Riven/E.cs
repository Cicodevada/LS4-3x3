using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;

namespace Spells
{
    public class RivenFeint : ISpellScript
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
			owner.SetTargetUnit(null, true);
			owner.CancelAutoAttack(false, false);
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
			PlayAnimation(owner, "Spell3" , 0.25f);
			AddBuff("RivenFeint", 3f, 1,  spell, spell.CastInfo.Owner, spell.CastInfo.Owner);	
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);

            FaceDirection(spellPos, owner, true);
            var trueCoords = GetPointFromUnit(owner, spell.SpellData.CastRangeDisplayOverride);

            ForceMovement(owner, null, trueCoords, 1450, 0, 0, 0, movementOrdersFacing: ForceMovementOrdersFacing.KEEP_CURRENT_FACING);
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
