using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;

namespace Buffs
{
    internal class FioraFlurry : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IBuff thisBuff;
        IParticle highlander;
        string particle;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			thisBuff = buff;
            if (unit is IObjAiBase owner)
            {
            var ELevel = owner.GetSpell(2).CastInfo.SpellLevel;	
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
             ApiEventManager.OnSpellPostCast.AddListener(this, owner.GetSpell("FioraQ"), OnDash); 			
            StatsModifier.AttackSpeed.PercentBonus = StatsModifier.AttackSpeed.PercentBonus += (45f + ELevel * 15f)/100f;
            unit.AddStatModifier(StatsModifier);
			}
        }
		public void OnDash(ISpell spell)
        {
			if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {                                
            var owner = spell.CastInfo.Owner as IChampion;
			AddBuff("FioraFlurryDummy", 3.0f, 1, spell, owner, owner);		
			}         		
        }
        public void OnLaunchAttack(ISpell spell)
        {
			
			if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {                                
            var owner = spell.CastInfo.Owner as IChampion;
			AddBuff("FioraFlurryDummy", 3.0f, 1, spell, owner, owner);		
			}
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(highlander);
			if (buff.TimeElapsed >= buff.Duration)
            {
				ApiEventManager.OnSpellPostCast.RemoveListener(this);
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
			if (unit is IObjAiBase ai)
            {
                SealSpellSlot(ai, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
            }
        }

        private void OnAutoAttack(IAttackableUnit target, bool isCrit)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
