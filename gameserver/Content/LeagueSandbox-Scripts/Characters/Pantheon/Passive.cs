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
     
    public class  CharScriptPantheon : ICharScript

    {
        ISpell Spell;
		int counter;
		IObjAiBase Owner;
        public void OnActivate(IObjAiBase owner, ISpell spell = null)

        {
            Owner = spell.CastInfo.Owner;
            Spell = spell;
            {             
				AddBuff("PantheonPassiveCounter", 25000f, 1, spell, owner, owner);
            }      
        }   
        public void OnDeactivate(IObjAiBase owner, ISpell spell = null)
        {
            //ApiEventManager.OnHitUnit.RemoveListener(this);
        }
        public void OnUpdate(float diff)
        {
        }
    }
}