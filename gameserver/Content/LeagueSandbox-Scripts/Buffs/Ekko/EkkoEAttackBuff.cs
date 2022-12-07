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
    internal class EkkoEAttackBuff : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IParticle activate;
		IParticle activate2;
		IBuff thisBuff;
		IObjAiBase owner;
        public void OnUpdate(float diff)
        {
        }

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			thisBuff = buff;
            var owner = ownerSpell.CastInfo.Owner;
			StatsModifier.Range.FlatBonus = 350.0f;
			if (unit is IObjAiBase ai)
            {
				OverrideAnimation(ai, "Spell3_Dash_to_Run", "Run");            
            }
			StatsModifier.MoveSpeed.PercentBonus = 0.2f + (0.1f * ownerSpell.CastInfo.SpellLevel);
			unit.AddStatModifier(StatsModifier);
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
			ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
			ApiEventManager.OnPreAttack.AddListener(this, owner, OnPreAttack, false);
            activate = AddParticleTarget(owner, unit, "ekko_base_e_indicator", unit, buff.Duration);
			AddParticleTarget(unit, unit, "ekko_base_e_vanish", unit);
			activate2 = AddParticleTarget(owner, unit, "ekko_base_e_weapon_glow", unit, buff.Duration,1,"weapon");       
            owner.SkipNextAutoAttack();
            owner.CancelAutoAttack(true, true);
        }
		public void OnPreAttack(ISpell spell)
        {
			
			if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {                       
            var owner = spell.CastInfo.Owner as IChampion;
            PlayAnimation(owner, "Spell3_Attack",1.5f);
			}
        }
        public void OnLaunchAttack(ISpell spell)
        {
			
			if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {                       
            var owner = spell.CastInfo.Owner as IChampion;
            spell.CastInfo.Owner.SkipNextAutoAttack();
            SpellCast(spell.CastInfo.Owner, 3, SpellSlotType.ExtraSlots, false, spell.CastInfo.Owner.TargetUnit, Vector2.Zero);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
            thisBuff.DeactivateBuff();
			}
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(activate);
			RemoveParticle(activate2);
			if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
			if (unit is IObjAiBase ai)
            {
				OverrideAnimation(ai, "Run", "Spell3_Dash_to_Run");
                SealSpellSlot(ai, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
            }
        }
    }
}
