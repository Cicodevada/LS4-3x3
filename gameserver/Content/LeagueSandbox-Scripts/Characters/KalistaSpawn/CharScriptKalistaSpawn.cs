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
    public class CharScriptKalistaSpawn : ICharScript
    {
        ISpell Spell;
		IAttackableUnit Target;
        public void OnActivate(IObjAiBase owner, ISpell spell = null)
        {
           AddParticleTarget(owner, owner, "Kalista_Base_W_Alerted.troy", owner, int.MaxValue);
		   AddParticleTarget(owner, owner, "Kalista_Base_W_Avatar.troy", owner, int.MaxValue);
		   AddParticleTarget(owner, owner, "Kalista_Base_W_Glow.troy", owner, int.MaxValue);
		   AddParticleTarget(owner, owner, "Kalista_Base_W_Glow2.troy", owner, int.MaxValue);
		   AddParticleTarget(owner, owner, "Kalista_Base_W_Glow.troy", owner, int.MaxValue);
		   AddParticleTarget(owner, owner, "Kalista_Base_W_ViewCone.troy", owner, int.MaxValue);
        }
        public void OnLaunchAttack(ISpell spell)        
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