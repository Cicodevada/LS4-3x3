using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class WujuStyleSuperChargedVisual : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        private IObjAiBase Owner;
        private ISpell daspell;
        private IObjAiBase daowner;
     
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IBuff thisBuff;
		IAttackableUnit Target;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Owner = ownerSpell.CastInfo.Owner;
            daowner = Owner;
            daspell = ownerSpell;
            SealSpellSlot(daowner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
            ApiEventManager.OnLaunchAttack.AddListener(this, ownerSpell.CastInfo.Owner, TargetTakePoison, false);
        }

        public void TargetTakePoison(ISpell spell)
        {
			var owner = daspell.CastInfo.Owner as IChampion;
			Target = spell.CastInfo.Targets[0].Unit;
			var Elevel = owner.GetSpell("WujuStyle").CastInfo.SpellLevel;
			var AD = owner.Stats.AttackDamage.Total * 0.35f;
            var Damage = 30 + (10*(Elevel -1)) + AD;
			Target.TakeDamage(owner, Damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			ApiEventManager.OnLaunchAttack.RemoveListener(this, unit as IObjAiBase);
			SealSpellSlot(ownerSpell.CastInfo.Owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}