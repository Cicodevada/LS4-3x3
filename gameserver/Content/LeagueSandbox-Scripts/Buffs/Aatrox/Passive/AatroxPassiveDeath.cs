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
    internal class AatroxPassiveDeath : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private IBuff ThisBuff;
		IAttackableUnit Unit;
		float timeSinceLastTick;
        float TickingDamage;
		ISpell Spell;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Spell = ownerSpell;
			Unit = unit;
			ThisBuff = buff;
			var owner = ownerSpell.CastInfo.Owner as IChampion;
			owner.StopMovement(); 
            owner.SetTargetUnit(null, true);			
			AddParticleTarget(owner, owner, "Aatrox_Passive_Death_Activate", owner, buff.Duration);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, true);
			PlayAnimation(owner, "Passive_Death",3f);
			buff.SetStatusEffect(StatusFlags.Targetable, false);
			unit.Stats.SetActionState(ActionState.CAN_MOVE, false);
			SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 0, SpellbookType.SPELLBOOK_SUMMONER, true);
            SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 1, SpellbookType.SPELLBOOK_SUMMONER, true);
			buff.SetStatusEffect(StatusFlags.Stunned, true);
			unit.Stats.SetActionState(ActionState.CAN_ATTACK, false);
            buff.SetStatusEffect(StatusFlags.Ghosted, true);
        }
		public void Heal(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
			var BloodP = owner.Stats.CurrentMana;
			var Blood = owner.Stats.ManaPoints.Total * 0.35f;
            var Health = (owner.Stats.CurrentMana + Blood) / 12f;
			var HealthM = owner.Stats.CurrentMana / 12f;
            owner.Stats.CurrentHealth += Health;
            owner.Stats.CurrentMana -= HealthM; 		
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			var owner = ownerSpell.CastInfo.Owner as IChampion;
			AddParticleTarget(owner, owner, "Aatrox_Passive_Death_End", owner);
			StopAnimation(owner, "Passive_Death",true,true,true);
			buff.SetStatusEffect(StatusFlags.Targetable, true);
			unit.Stats.SetActionState(ActionState.CAN_MOVE, true);
			unit.Stats.SetActionState(ActionState.CAN_ATTACK, true);
            buff.SetStatusEffect(StatusFlags.Ghosted, false);
			buff.SetStatusEffect(StatusFlags.Stunned, false);
			SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 0, SpellbookType.SPELLBOOK_SUMMONER, false);
            SealSpellSlot(owner, SpellSlotType.SummonerSpellSlots, 1, SpellbookType.SPELLBOOK_SUMMONER, false);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, false);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);
			AddBuff("AatroxPassiveActivate", 12f, 1, ownerSpell, ownerSpell.CastInfo.Owner , ownerSpell.CastInfo.Owner,false);
        }
        public void OnUpdate(float diff)
        {
			timeSinceLastTick += diff;
            if (timeSinceLastTick >= 250.0f)
            {              
                Heal(Spell);
                timeSinceLastTick = 0;
            }
        }
    }
}