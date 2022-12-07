using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using System.Collections.Generic;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;


namespace Buffs
{
    internal class RengarManager : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
			MaxStacks = 5
        };

        public IStatsModifier StatsModifier { get; private set; }

        IParticle p;
		IParticle p1;
		IParticle p2;
		ISpell spell;
        IAttackableUnit Unit;
		//IAttackableUnit target;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Unit = unit;
			spell = ownerSpell;
			unit.Stats.CurrentMana += 1f;
            switch (buff.StackCount)
            {
                case 1:                  
                    break;
                case 2:                	 			
                    break;
			    case 3:
                    break;
                case 4:				 			
                    break;
				case 5:
                    RemoveBuff(unit, "RengarManager");                 			
                    AddBuff("RengarFerocityManager", 8.0f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);				
                    return;
            }
        }
     

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			if (buff.TimeElapsed >= buff.Duration)
            {
                RemoveBuff(unit, "RengarManager");
            }
            RemoveParticle(p);
			RemoveParticle(p1);
			RemoveParticle(p2);
        }  
        public void OnUpdate(float diff)
        {

        }
    }
}