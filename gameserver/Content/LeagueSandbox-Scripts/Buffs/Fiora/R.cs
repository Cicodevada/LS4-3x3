using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using GameServerCore;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Scripting.CSharp;
using System;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;


namespace Buffs
{
    internal class FioraDanceStrike : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private IBuff ThisBuff;
		IAttackableUnit Unit;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Unit = unit;
			ThisBuff = buff;
			var owner = ownerSpell.CastInfo.Owner as IChampion;
			owner.StopMovement();
            //owner.SetDashingState(true);			
			AddParticleTarget(owner, owner, "", owner, buff.Duration);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, true);
			PlayAnimation(owner, "",3f);
			buff.SetStatusEffect(StatusFlags.Targetable, false);
			//unit.Stats.SetActionState(ActionState.CAN_MOVE, false);
			SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 0, SpellbookType.SPELLBOOK_SUMMONER, true);
            SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 1, SpellbookType.SPELLBOOK_SUMMONER, true);
			//buff.SetStatusEffect(StatusFlags.Stunned, true);
			unit.Stats.SetActionState(ActionState.CAN_ATTACK, false);
            buff.SetStatusEffect(StatusFlags.Ghosted, true);		
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			var owner = ownerSpell.CastInfo.Owner as IChampion;
			//owner.SetDashingState(false);
			AddParticleTarget(owner, owner, "", owner);
			StopAnimation(owner, "");
			buff.SetStatusEffect(StatusFlags.Targetable, true);
			//unit.Stats.SetActionState(ActionState.CAN_MOVE, true);
			unit.Stats.SetActionState(ActionState.CAN_ATTACK, true);
            buff.SetStatusEffect(StatusFlags.Ghosted, false);
			//buff.SetStatusEffect(StatusFlags.Stunned, false);
			SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 0, SpellbookType.SPELLBOOK_SUMMONER, false);
            SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 1, SpellbookType.SPELLBOOK_SUMMONER, false);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, false);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);
        }
        public void OnUpdate(float diff)
        {
        }
    }
}