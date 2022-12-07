using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using System.Collections.Generic;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;


namespace Buffs
{
    internal class FizzSeastonePassive : IBuffGameScript
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
		IAttackableUnit Target;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisBuff = buff;
            if(unit is IObjAiBase ai)
            {
                Unit = ai;
				SealSpellSlot(ai, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true);
                ApiEventManager.OnLaunchAttack.AddListener(this, ai, TargetExecute, true);
				//SetAnimStates(ai, new Dictionary<string, string> { { "Attack1", "Spell1" } });
                //p = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, "Rengar_Base_Q_Buf_AttackSpeed.troy", unit, buff.Duration, 1, "WEAPON");
				p = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, ".troy", unit, buff.Duration, 1, "");
                p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, ".troy", unit, buff.Duration, 1, "");
                ai.SkipNextAutoAttack();
				ownerSpell.CastInfo.Owner.CancelAutoAttack(true);
            }
			buff.SetStatusEffect(StatusFlags.Ghosted, true);
            StatsModifier.Range.FlatBonus += 50;
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			if(unit is IObjAiBase ai)
            {
                Unit = ai;
				//ApiEventManager.OnLaunchAttack.RemoveListener(this);
			    SealSpellSlot(ai, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, false);
			}
            RemoveParticle(p);
            RemoveParticle(p2);
        }
        public void TargetExecute(ISpell spell)
        {
            if (!thisBuff.Elapsed() && thisBuff != null && Unit != null)
            {
				Target = spell.CastInfo.Targets[0].Unit;
                float ad = Unit.Stats.AbilityPower.Total * 0.35f;
                float damage = 30 + 10 * Unit.GetSpell(1).CastInfo.SpellLevel + ad;
                Target.TakeDamage(Unit, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
				AddParticleTarget(Unit, Target, "fizz_seastoneactive_hit_sound.troy", Unit);
				AddParticleTarget(Unit, Target, "Fizz_MarinerDoom.troy", Unit);
				AddParticleTarget(Unit, Target, "Fizz_PiercingStrike_tar.troy", Unit);
				AddParticleTarget(Unit, Target, "Fizz_PiercingStrike.troy", Unit);
                thisBuff.DeactivateBuff();
            }
        }
        public void OnUpdate(float diff)
        {

        }
    }
}