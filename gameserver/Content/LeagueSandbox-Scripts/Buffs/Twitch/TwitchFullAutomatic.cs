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
    class TwitchFullAutomatic : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IParticle pbuff;
        IParticle pbuff2;
        IBuff thisBuff;
		ISpell Spell;
		IObjAiBase owner;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Spell = ownerSpell;
            thisBuff = buff;
			owner = ownerSpell.CastInfo.Owner as IChampion;
			if (unit is IObjAiBase ai)
            {
			StatsModifier.Range.FlatBonus = 300.0f;
			StatsModifier.AttackDamage.FlatBonus = ((owner.GetSpell(3).CastInfo.SpellLevel * 15) + 25f);
			unit.AddStatModifier(StatsModifier);
            SealSpellSlot(ai, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, true);
			pbuff = AddParticleTarget(ai, ai, "Twitch_Base_R_Buff", ai, lifetime: buff.Duration);
            AddParticle(ai, null, "Twitch_Base_R_Cas", ai.Position, lifetime: buff.Duration);		
			ApiEventManager.OnLaunchAttack.AddListener(this, ai, OnLaunchAttack, false);
            ai.CancelAutoAttack(false, true);
			}
        }
        public void OnLaunchAttack(ISpell spell)
        {		
			if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {
				for (int bladeCount = 0; bladeCount <= 1; bladeCount++)
                {              
				 var end = GetPointFromUnit(owner, 1100f);
				 //SpellCast(owner, 1, SpellSlotType.ExtraSlots, end, Vector2.Zero, true, start);
                 SpellCast(owner, 0, SpellSlotType.ExtraSlots, end, end, false, Vector2.Zero);				 
                }
				//SpellCast(owner, 0, SpellSlotType.ExtraSlots, GetPointFromUnit(owner, 1100f), Vector2.Zero, false, owner.Position);
				//SpellCast(owner, 0, SpellSlotType.ExtraSlots, GetPointFromUnit(owner, 1100f), GetPointFromUnit(owner, 1100f), false, Vector2.Zero);
                //SpellCast(owner, 0, SpellSlotType.ExtraSlots, false, owner.TargetUnit, Vector2.Zero);
			}
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveParticle(pbuff);
            RemoveParticle(pbuff2);
			RemoveBuff(thisBuff);
			if (buff.TimeElapsed >= buff.Duration)
            {
				ApiEventManager.OnPreAttack.RemoveListener(this);
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
			if (unit is IObjAiBase ai)
            {
                SealSpellSlot(ai, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);
            }
        }	       
        public void OnUpdate(float diff)
        {
        }
    }
}
