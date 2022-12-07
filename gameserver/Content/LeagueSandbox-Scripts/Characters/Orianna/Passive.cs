using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using GameServerLib.GameObjects.AttackableUnits;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Enums;
using System;

namespace Spells
{
    public class OrianaSpellSword : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnHitUnit.AddListener((IChampion)owner, owner,OnHitUnit,true);
        }

        private void OnHitUnit(IDamageData damage)
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

