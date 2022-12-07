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
    public class CharScriptAatrox : ICharScript
    {
		ISpell Spell;
		int counter;
		public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(IObjAiBase owner, ISpell spell = null)
        {
			Spell = spell;
			var Health = owner.Stats.CurrentMana * 1f;
            owner.Stats.CurrentMana -= Health; 
			AddBuff("AatroxPassive", 25000f, 1, Spell, Spell.CastInfo.Owner , Spell.CastInfo.Owner,true);
            if (owner.HasBuff("AatroxR"))
                {
				OverrideAnimation(owner, "Idle_IN_ULT", "Idle_IN");
			    }
			    else
			    {
				//OverrideAnimation(owner, "RUN", "RUN_ULT");
			    }			
        }
		public void OnLevelUp (IAttackableUnit owner)
        {
         
        }
        public void OnDeactivate(IObjAiBase owner, ISpell spell = null)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}

