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
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Enums;

namespace Spells
{
    public class XenZhaoBattleCry : ISpellScript
    {
		ISpell Spell;
		IObjAiBase Owner;
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            Spell = spell;
            Owner = spell.CastInfo.Owner;
            ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, true);

            AddBuff("XenZhaoBattleCryPassive", 1f, 1, spell, owner, owner, true);
        }
        public void OnLevelUp (ISpell spell)
        {
            ApiEventManager.OnLaunchAttack.AddListener(this, spell.CastInfo.Owner, OnLaunchAttack, false);
        }
        public void OnLaunchAttack(ISpell spell)
        {
            AddBuff("XenZhaoBattleCryPassive", 3f, 1, Spell, Owner, Owner);
        }
        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("XenZhaoBattleCry", 5f, 1, spell, owner, owner,false);
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