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
using System.Linq;
using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;


namespace CharScripts
{
     
    public class  CharScriptDarius : ICharScript

    {
        ISpell Spell;
		IAttackableUnit Target;
        public void OnActivate(IObjAiBase owner, ISpell spell = null)

        {

            Spell = spell;
            {
                ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            }
        }
        public void OnLaunchAttack(ISpell spell)      
        {
			Target = spell.CastInfo.Targets[0].Unit;
            var owner = Spell.CastInfo.Owner;
            if (owner.HasBuff("DariusHemoVisual"))
			{
		    AddBuff("DariusHemo", 6.0f, 5, Spell, Target, owner);
			}
			else
			{
			AddBuff("DariusHemo", 6.0f, 1, Spell, Target, owner);
			}
        }     
        public void OnDeactivate(IObjAiBase owner, ISpell spell = null)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}