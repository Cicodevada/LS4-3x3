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
    internal class RengarQBuff : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IBuff thisBuff;
        IObjAiBase Unit;
        IParticle p;
		ISpell ownerSpell;
        IParticle p2;
		IAttackableUnit target;
		int counter = 1;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisBuff = buff;
            if(unit is IObjAiBase ai)
            {
                Unit = ai;
				SealSpellSlot(ai, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true);
				ApiEventManager.OnLaunchAttack.AddListener(this, ai, OnLaunchAttack, false);
				ai.CancelAutoAttack(true, true);
            }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			if(unit is IObjAiBase ai)
            {
                Unit = ai;
			    SealSpellSlot(ai, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
			}
			if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
            RemoveParticle(p);
            RemoveParticle(p2);
        }
		public void OnLaunchAttack(ISpell spell)
        {
			if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {           		      		
				thisBuff.DeactivateBuff();                   				
			}
        }     
        public void OnUpdate(float diff)
        {

        }
    }
	class RengarQEmp : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IBuff thisBuff;
        IObjAiBase Unit;
        IParticle p;
		ISpell ownerSpell;
        IParticle p2;
		IAttackableUnit target;
		int counter = 1;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisBuff = buff;
            if(unit is IObjAiBase ai)
            {
                Unit = ai;
				SealSpellSlot(ai, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true);
				ApiEventManager.OnLaunchAttack.AddListener(this, ai, OnLaunchAttack, false);
				ai.CancelAutoAttack(true, true);
            }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			if(unit is IObjAiBase ai)
            {
                Unit = ai;
			    SealSpellSlot(ai, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
			}
			if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
            RemoveParticle(p);
            RemoveParticle(p2);
        }
		public void OnLaunchAttack(ISpell spell)
        {
			if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {           		      		
				thisBuff.DeactivateBuff();                   				
			}
        }     
        public void OnUpdate(float diff)
        {

        }
    }
}
