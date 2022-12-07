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
    class VorpalSpikes : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        int counter;
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IParticle pbuff;
        IParticle pbuff2;
        IBuff thisBuff;
		IObjAiBase owner;
		IBuff Feast;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisBuff = buff;
			if (unit is IObjAiBase ai)
            {
            var owner = ownerSpell.CastInfo.Owner as IChampion;
			StatsModifier.Range.FlatBonus = 50.0f;
			unit.AddStatModifier(StatsModifier);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
			ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
			owner.SkipNextAutoAttack();
            owner.CancelAutoAttack(false, true);
			}
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			var owner = ownerSpell.CastInfo.Owner as IChampion;
			RemoveParticle(pbuff);
            RemoveParticle(pbuff2);
			RemoveBuff(thisBuff);
			if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
			if (unit is IObjAiBase ai)
            {
                SealSpellSlot(ai, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
            }
        }

        public void OnLaunchAttack(ISpell spell)
        {
			if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {
                var owner = spell.CastInfo.Owner as IChampion;
                spell.CastInfo.Owner.SkipNextAutoAttack();
				Shot(spell);
                counter++;      
                if (counter == 2)
                {
                Shot(spell);
                }
				if (counter == 3)
                {              
                Shot(spell);
				thisBuff.DeactivateBuff();
                }				                  
			}
        }
		public void Shot(ISpell spell)
        {
            var owner = spell.CastInfo.Owner as IChampion;
            var targetPos = GetPointFromUnit(owner, 1150.0f);  		        
            SpellCast(owner, 0, SpellSlotType.ExtraSlots, targetPos, targetPos, true, Vector2.Zero);            
        }
        public void OnUpdate(float diff)
        {
        }
    }
}
