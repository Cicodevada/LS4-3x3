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
     
    public class  CharScriptYasuo : ICharScript
    {
		IObjAiBase Owner;
        public void OnActivate(IObjAiBase owner, ISpell spell = null)
        {       
		    Owner = spell.CastInfo.Owner;
            ApiEventManager.OnSpellPostCast.AddListener(this, Owner.GetSpell(0), OnCast);
			ApiEventManager.OnSpellPostCast.AddListener(this, Owner.GetSpell(1), OnCast);
			ApiEventManager.OnSpellPostCast.AddListener(this, Owner.GetSpell(2), OnCast);
			ApiEventManager.OnSpellPostCast.AddListener(this, Owner.GetSpell(3), OnCast);
        }
		public void OnCast(ISpell spell)
        {   
		    //OverrideAnimation(Owner, "Run1_IN_Sheathed", "RUN");
			//OverrideAnimation(Owner, "Yasuo_Idle_IN", "Idle2");
			//OverrideAnimation(Owner, "Yasuo_Idle_IN", "Idle3");
		}
        public void OnDeactivate(IObjAiBase owner, ISpell spell = null)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}