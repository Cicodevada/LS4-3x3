using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class EkkoR : ISpellScript
    {
		IBuff Buff;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
			ApiEventManager.OnLevelUpSpell.AddListener(this, spell, OnLevelUp, true);
        }
		public void OnLevelUp (ISpell spell)
        {
			var owner = spell.CastInfo.Owner as IChampion;
            Buff = AddBuff("EkkoRMinion", 250000f, 1, spell, owner, owner);
			CreateTimer(0.1f, () =>
            {
			ApiEventManager.OnLevelUpSpell.RemoveListener(this);
			});
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			//Ekko = (Buff.BuffScript as Buffs.EkkoRMinion).EkkoSpawn();
        }

        public void OnSpellCast(ISpell spell)
        {   
            var owner = spell.CastInfo.Owner;		
			AddBuff("EkkoR", 0.5f, 1, spell, owner, owner);         
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
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
