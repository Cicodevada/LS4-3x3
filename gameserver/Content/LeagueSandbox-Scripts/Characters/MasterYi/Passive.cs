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
     
    public class  CharScriptMasterYi : ICharScript

    {
        ISpell Spell;
        public void OnActivate(IObjAiBase owner, ISpell spell = null)

        {

            Spell = spell;
            {
                ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            }
			var ownerSkinID = owner.SkinID;
            if (ownerSkinID == 2)
            {			
            AddParticleTarget(owner, owner, "MasterYi_Skin02_Glow_Sword_Blue.troy", owner, 25000f, 1, "BUFFBONE_Cstm_Sword1_loc");
			}
        }
        public void OnLaunchAttack(ISpell spell)      
        {
            var owner = Spell.CastInfo.Owner;
            AddBuff("MasterYiPassive", 3f, 1, Spell, owner, owner);
        }     
        public void OnDeactivate(IObjAiBase owner, ISpell spell = null)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this);
        }
        public void OnUpdate(float diff)
        {
        }
    }
}