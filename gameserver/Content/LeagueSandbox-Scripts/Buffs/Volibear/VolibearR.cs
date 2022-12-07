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
    class VolibearR : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IParticle pbuff;
        IParticle pbuff2;
		IParticle pbuff3;
        IBuff thisBuff;
		IObjAiBase owner;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisBuff = buff;
			if (unit is IObjAiBase ai)
            {
            var owner = ownerSpell.CastInfo.Owner as IChampion;
			if (ai.HasBuff("VolibearQ"))
            {
				OverrideAnimation(ai, "spell1_run", "RUN");
			}
			else
			{
				OverrideAnimation(ai, "RUN", "spell1_run");
			}
			//OverrideAnimation(ai, "spell1_run", "RUN");
			StatsModifier.MoveSpeed.PercentBonus += 0.4f;
			AddParticleTarget(owner, ai, "", ai, buff.Duration, 1f);
			AddParticleTarget(owner, ai, "", ai, buff.Duration, 1f);
			pbuff = AddParticleTarget(owner, ai, "volibear_R_attack_buf_right", ai, buff.Duration, 1f,"R_HAND");
            pbuff2 = AddParticleTarget(owner, ai, "volibear_R_attack_buf_left", ai, buff.Duration, 1f,"L_HAND");
			StatsModifier.Range.FlatBonus = 50.0f;
			unit.AddStatModifier(StatsModifier);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, true);
			//ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
			owner.SkipNextAutoAttack();
            owner.CancelAutoAttack(false, true);
			}
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			var owner = ownerSpell.CastInfo.Owner as IChampion;
			//OverrideAnimation(unit, "RUN", "spell1_run");
			RemoveParticle(pbuff);
            RemoveParticle(pbuff2);
			RemoveParticle(pbuff3);
			RemoveBuff(thisBuff);
			if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
			if (unit is IObjAiBase ai)
            {
                SealSpellSlot(ai, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);
            }
        }

        public void OnLaunchAttack(ISpell spell)
        {
			
			if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {                       
            spell.CastInfo.Owner.RemoveBuff(thisBuff);
            var owner = spell.CastInfo.Owner as IChampion;
            spell.CastInfo.Owner.SkipNextAutoAttack();
            SpellCast(spell.CastInfo.Owner, 0, SpellSlotType.ExtraSlots, false, spell.CastInfo.Owner.TargetUnit, Vector2.Zero);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);
            thisBuff.DeactivateBuff();
			}
        }
        public void OnUpdate(float diff)
        {
        }
    }
}
