using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;


namespace ItemSpells
{
    public class TrinketTotemLvl1 : ISpellScript
    {
        IMinion ward;
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
            var Cursor = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var distance = Cursor - current;
            Vector2 truecoords;
            if (distance.Length() > 500f)
            {
                distance = Vector2.Normalize(distance);
                var range = distance * 500f;
                truecoords = current + range;
            }
            else
            {
                truecoords = Cursor;
            }

            ward = AddMinion(owner, "YellowTrinket", "YellowTrinket", truecoords, owner.Team, owner.SkinID, false, true);
			AddParticle(owner, null, "Global_Trinket_Yellow.troy", truecoords);
            AddBuff("YellowTriket", 65f, 1, spell, ward, ward);
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
