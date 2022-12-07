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
    public class CharScriptNinan : ICharScript
    {
        ISpell Spell;
		IAttackableUnit Target;
        public void OnActivate(IObjAiBase owner, ISpell spell = null)
        {
            Spell = spell;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
        }
        public void OnLaunchAttack(ISpell spell)        
        {
			var owner = spell.CastInfo.Owner;
            Target = spell.CastInfo.Targets[0].Unit;
			var getbuff = Target.GetBuffWithName("NinanPassiveBleed");
            if (Target.HasBuff("NinanPassiveBleed"))
            {        
                AddBuff("NinanPassiveCooldown", 2f, 1, spell, Target, owner);
                getbuff.DeactivateBuff();			
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

