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
    public class CharScriptRenekton : ICharScript
    {
		ISpell Spell;
		int counter;
		public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(IObjAiBase owner, ISpell spell = null)
        {
			Spell = spell;
			var Health = owner.Stats.CurrentMana * 1f;
			var Blood = owner.Stats.ManaPoints.Total * 0.5f;
			var Health2 = owner.Stats.CurrentMana;       
            if (Health2 >= Blood)
			{
				//AddBuff("", 4.0f, 1, spell, owner, owner);
			}			
			else
			{
			}
			ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            owner.Stats.CurrentMana -= Health; 	
        }
		public void OnLaunchAttack(ISpell spell)        
        {
			var owner = spell.CastInfo.Owner;
            owner.Stats.CurrentMana += 10f;			
        }       
        public void OnDeactivate(IObjAiBase owner, ISpell spell = null)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}

