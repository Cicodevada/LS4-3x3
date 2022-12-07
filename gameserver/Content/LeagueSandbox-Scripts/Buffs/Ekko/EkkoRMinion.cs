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

namespace Buffs
{
    internal class EkkoRMinion : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IMinion Ekko;
        ISpell Spell;
		IObjAiBase Owner;
		private IBuff buff;
		float timeSinceLastTick = 1000f;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Spell = ownerSpell;
			if (ownerSpell.CastInfo.Owner is IChampion owner)
            {
			    Ekko = AddMinion(owner, owner.Model, owner.Model, owner.Position, owner.Team, owner.SkinID, true, false);	
				Ekko.SetTargetUnit(owner, true);
                Ekko.UpdateMoveOrder(OrderType.AttackTo, true);	
                AddBuff("EkkoRInvuln", 250000f, 1, Spell, Ekko, owner);
                //AddParticleTarget(owner, Ekko, "Become_Transparent.troy", Ekko, 25000f, 1);				
				AddParticleTarget(owner, owner, "Ekko_Base_R_ChargeIndicator.troy", owner, 25000f, 1); 
		        AddParticleTarget(owner, owner, "Ekko_Base_R_ChargeUp.troy", owner, 25000f, 1); 
		        AddParticleTarget(owner, owner, "Ekko_Base_R_RewindIndicator.troy", owner, 25000f, 1,"HEAD"); 
		        AddParticleTarget(owner, owner, "Ekko_Base_R_TimeDevice_Active.troy", owner, 25000f, 1,"HEAD"); 
			}
            
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
		   if (Spell.CastInfo.Owner is IChampion owner)
           {		      			  	 
			   
		   }
        }
        public void OnUpdate(float diff)
        {
            timeSinceLastTick += diff;

            if (timeSinceLastTick >= 0f)
            {    
                AddParticleTarget(Owner, Ekko, "Become_Transparent.troy", Ekko, 1f, 1);			
                timeSinceLastTick = -1000f;
            }			
        }
    }
}