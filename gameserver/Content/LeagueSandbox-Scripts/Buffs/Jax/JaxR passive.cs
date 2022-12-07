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
    class JaxRelentlessAttack : IBuffGameScript
    {
		public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
			BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
			MaxStacks = 3
        };   

        public IStatsModifier StatsModifier { get; private set; }

        IParticle p;
		IParticle p1;
		IParticle p2;
		ISpell spell;
		IBuff thisBuff;
        IAttackableUnit Unit;
		//IAttackableUnit target;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			thisBuff = buff;
            Unit = unit;
			spell = ownerSpell;
            switch (buff.StackCount)
            {
                case 1:
                    
                    break;
                case 2:			
                    break;
			    case 3:
                    SpellCast(spell.CastInfo.Owner, 2, SpellSlotType.ExtraSlots, false, spell.CastInfo.Owner.TargetUnit, Vector2.Zero);                 					
                    buff.DeactivateBuff();				
                    break;              
            }
        }
     

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveBuff(unit, "JaxRelentlessAttack");
            RemoveParticle(p);
			RemoveParticle(p1);
			RemoveParticle(p2);
			if (buff.TimeElapsed >= buff.Duration)
            {
                //ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
        }
		public void OnLaunchAttack(ISpell spell)
        {
			
			if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {                       
            spell.CastInfo.Owner.RemoveBuff(thisBuff);
            var owner = spell.CastInfo.Owner as IChampion;
            spell.CastInfo.Owner.SkipNextAutoAttack();
            SpellCast(spell.CastInfo.Owner, 2, SpellSlotType.ExtraSlots, false, spell.CastInfo.Owner.TargetUnit, Vector2.Zero);
            thisBuff.DeactivateBuff();
			}
        }
        public void OnUpdate(float diff)
        {
        }
    }
}