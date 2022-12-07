using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using System.Numerics;
using GameServerCore;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain;

namespace CharScripts
{
    public class CharScriptRengar : ICharScript
    {
		ISpell Spell;
		int counter;
		public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(IObjAiBase owner, ISpell spell = null)
        {
			Spell = spell;
			var CBZ = owner.Stats.CurrentMana;
            owner.Stats.CurrentMana -= CBZ; 
        }
        public void OnDeactivate(IObjAiBase owner, ISpell spell = null)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}

