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
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace CharScripts
{
    public class CharScriptAzirSoldier : ICharScript
    {
        ISpell Spell;
		IAttackableUnit Target;
		IObjAiBase Owner;
        public void OnActivate(IObjAiBase owner, ISpell spell = null)
        {
			Owner = owner;
            ApiEventManager.OnDeath.AddListener(this, owner, OnDeath, true);
        }
        public void OnDeath(IDeathData data)
        {           
            AddParticleTarget(Owner, Owner, "Azir_Base_W_SoldierTimeout.troy", Owner,10,10);
			AddParticleTarget(Owner, Owner, "Azir_Base_W_Soldier_Outline.troy", Owner,10,10);
        }
        public void OnDeactivate(IObjAiBase owner, ISpell spell = null)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}