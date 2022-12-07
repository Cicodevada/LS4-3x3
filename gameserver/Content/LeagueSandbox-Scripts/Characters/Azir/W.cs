using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;


namespace Spells
{
    public class AzirW : ISpellScript
    {
        public static IMinion Soldier;
		IObjAiBase Owner;
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            // TODO
            TriggersSpellCasts = true,
        };

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
			var Cursor = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var current = new Vector2(Owner.Position.X, Owner.Position.Y);
            var distance = Cursor - current;
            Vector2 truecoords;
            if (distance.Length() > 450f)
            {
                distance = Vector2.Normalize(distance);
                var range = distance * 450f;
                truecoords = current + range;
            }
            else
            {
                truecoords = Cursor;
            }

            Soldier = AddMinion(Owner, "AzirSoldier", "AzirSoldier", truecoords, Owner.Team, Owner.SkinID, true, false);
			AddBuff("AzirW", 10f, 1, spell, Soldier, Soldier);      
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