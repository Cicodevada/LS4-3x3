using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class MasterYiPassive : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
			MaxStacks = 4
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
            switch (buff.StackCount)
            {
                case 1:
                    
                    break;
                case 2:                	 			
                    break;
			    case 3:
				    AddBuff("MasterYiDoubleStrike", 3.1f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);
                    break;
                case 4:				 
				    //spell.CastInfo.Owner.SetAutoAttackSpell("MasterYiDoubleStrike", false);               		
                    buff.DeactivateBuff();				
                    break;
            }
        }
     

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			if (buff.TimeElapsed >= buff.Duration)
            {
                RemoveBuff(unit, "MasterYiPassive");
            }
			RemoveBuff(unit, "AatroxWONHLifeBuff");
            RemoveParticle(p);
			RemoveParticle(p1);
			RemoveParticle(p2);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}