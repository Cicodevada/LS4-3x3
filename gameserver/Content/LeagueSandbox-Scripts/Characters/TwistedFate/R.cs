using System.Linq;
using GameServerCore;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class Destiny : ISpellScript
    {
        IObjAiBase Owner;
        float damage;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
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
            var owner = spell.CastInfo.Owner;
			AddBuff("Destiny Marker", ((owner.GetSpell(3).CastInfo.SpellLevel * 2) + 4f), 1, spell, Owner, Owner);
            var champions = GetChampionsInRange(owner.Position, 20000, true);
            for (int i = 0; i < champions.Count; i++)
            {
                if (champions[i].Team != owner.Team)
                {
                    AddBuff("Destiny", ((owner.GetSpell(3).CastInfo.SpellLevel * 2) + 4f), 1, spell, champions[i], Owner);
                }
            }
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
