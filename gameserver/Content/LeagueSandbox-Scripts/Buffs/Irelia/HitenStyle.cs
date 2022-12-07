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
    class IreliaHitenStyleCharged : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
		IAttackableUnit Target;
        private ISpell spell;
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			spell = ownerSpell;
			AddParticleTarget(unit, unit, "irelia_hitenStlye_active_glow.troy", unit, 6f,1,"WEAPON");

                if (unit is IObjAiBase obj)
            { 
					SealSpellSlot(obj, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true);
                    ApiEventManager.OnLaunchAttack.AddListener(this, obj, OnLaunchAttack, false);
            }
        }
        public void OnLaunchAttack(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
			Target = spell.CastInfo.Targets[0].Unit;
            float damage = 20 + (30 * (owner.GetSpell(1).CastInfo.SpellLevel - 1));
            float heal = 10 * spell.CastInfo.SpellLevel;
            owner.Stats.CurrentHealth += heal;
            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }           
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnLaunchAttack.RemoveListener(this, unit as IObjAiBase);
			SealSpellSlot(ownerSpell.CastInfo.Owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, false);
        }
        
        public void OnUpdate(float diff)
        {

        }
    }
}