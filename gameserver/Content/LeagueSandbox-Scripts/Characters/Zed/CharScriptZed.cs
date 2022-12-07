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
    public class CharScriptZed : ICharScript
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
			var owner = spell.CastInfo.Owner;
            Target = spell.CastInfo.Targets[0].Unit;
			float BBlood = Target.Stats.HealthPoints.Total * 0.5f;
			float XBlood = Target.Stats.CurrentHealth;
			if (BBlood >= XBlood && !Target.HasBuff("ZedPassiveToolTip") && Target.Team != owner.Team && !(Target is IObjBuilding || Target is IBaseTurret))
			{		
				AddBuff("ZedPassiveToolTip", 10f, 1, spell, Target, owner);
			}
			else
			{
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