using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects.Spell;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Numerics;
using GameServerCore.Scripting.CSharp;

namespace CharScripts
{
    public class CharScriptDiana : ICharScript
    {
        IObjAiBase diana = null;
        float stanceTime = 500;
        float stillTime = 0;
        bool beginStance = false;
        bool stance = false;

        ISpell Spell;
        public void OnActivate(IObjAiBase owner, ISpell spell = null)
        {
            Spell = spell;
            {
                ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            }
			var ownerSkinID = owner.SkinID;        
        }
        public void OnLaunchAttack(ISpell spell)      
        {
            var owner = Spell.CastInfo.Owner;
            AddBuff("DianaPassive", 4f, 1, Spell, owner, owner);
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