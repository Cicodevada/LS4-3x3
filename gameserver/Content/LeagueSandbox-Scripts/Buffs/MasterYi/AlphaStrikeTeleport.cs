using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects.Spell;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;

namespace Buffs
{
    internal class AlphaStrikeTeleport : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        private IObjAiBase Owner;
        public IStatsModifier StatsModifier { get; private set; }

        private readonly IAttackableUnit target = Spells.AlphaStrike.Target;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Owner = ownerSpell.CastInfo.Owner;
            SetStatus(unit, StatusFlags.NoRender, true);
			buff.SetStatusEffect(StatusFlags.Ghosted, true);    
			SetStatus(unit, StatusFlags.Targetable, false);
			unit.Stats.SetActionState(ActionState.CAN_MOVE, false);	
			unit.Stats.SetActionState(ActionState.CAN_ATTACK, false);
            SealSpellSlot(Owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true);
			SealSpellSlot(Owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true);
			SealSpellSlot(Owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
			SealSpellSlot(Owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, true);
			SealSpellSlot(Owner, SpellSlotType.SummonerSpellSlots, 0, SpellbookType.SPELLBOOK_SUMMONER, true);
            SealSpellSlot(Owner, SpellSlotType.SummonerSpellSlots, 1, SpellbookType.SPELLBOOK_SUMMONER, true);			
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            if (unit is IObjAiBase ai)
            {
            TeleportTo(ai, target.Position.X, target.Position.Y);
			}
			if (Owner.Team != target.Team && target is IChampion)
            {
                Owner.SetTargetUnit(target, true);
                Owner.UpdateMoveOrder(OrderType.AttackTo, true);
            }
			SetStatus(unit, StatusFlags.NoRender, false);
			SetStatus(unit, StatusFlags.Ghosted, false);
            SetStatus(unit, StatusFlags.Targetable, true);
            unit.Stats.SetActionState(ActionState.CAN_MOVE, true);	
			unit.Stats.SetActionState(ActionState.CAN_ATTACK, true);			
			SealSpellSlot(Owner, SpellSlotType.SummonerSpellSlots, 0, SpellbookType.SPELLBOOK_SUMMONER, false);
            SealSpellSlot(Owner, SpellSlotType.SummonerSpellSlots, 1, SpellbookType.SPELLBOOK_SUMMONER, false);
			SealSpellSlot(Owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
			SealSpellSlot(Owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, false);
			SealSpellSlot(Owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
			SealSpellSlot(Owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}