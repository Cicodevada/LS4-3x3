using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class ZedUltBuff : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IBuff ThisBuff;
        private ISpell Spelll;
        private IAttackableUnit Target;
        private IObjAiBase Owner;
        private float ticks = 0;
        private float damage;
		IParticle p;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ThisBuff = buff;
            Owner = ownerSpell.CastInfo.Owner;
            Spelll = ownerSpell;
            unit.Stats.SetActionState(ActionState.CAN_MOVE, false);	
			unit.Stats.SetActionState(ActionState.CAN_ATTACK, false);
			//p = AddParticleTarget(Owner, Owner, "Become_Transparent",Owner,10,10,"Buffbone_Cstm_Healthbar");
            SealSpellSlot(Owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true);
			SealSpellSlot(Owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true);
			SealSpellSlot(Owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
			SealSpellSlot(Owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, true);
            buff.SetStatusEffect(StatusFlags.Targetable, false);
            buff.SetStatusEffect(StatusFlags.Ghosted, true);

            Target = unit;

        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveParticle(p);
			SealSpellSlot(Owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
			SealSpellSlot(Owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, false);
			SealSpellSlot(Owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
			SealSpellSlot(Owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);		
			unit.Stats.SetActionState(ActionState.CAN_MOVE, true);
			unit.Stats.SetActionState(ActionState.CAN_ATTACK, true);
            buff.SetStatusEffect(StatusFlags.Targetable, true);
            buff.SetStatusEffect(StatusFlags.Ghosted, false);
        }

        public void OnUpdate(float diff)
        {
         
        }
    }
}