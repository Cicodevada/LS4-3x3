using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    class XenZhaoBattleCry : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
        };
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        
        public void OnUpdate(float diff)
        {
        }

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
			AddParticleTarget(unit, unit, "xenziou_battle_cry_weapon_01.troy.troy", unit, buff.Duration, 1, "Buffbone_Glb_WEAPON_1");
            AddParticleTarget(unit, unit, "xen_ziou_battleCry_cas.troy", unit, buff.Duration, 1, "Buffbone_Glb_WEAPON_1");
			AddParticleTarget(unit, unit, "xen_ziou_battleCry_cas_02.troy", unit, buff.Duration, 1, "Buffbone_Glb_WEAPON_1");
			AddParticleTarget(unit, unit, "xen_ziou_battleCry_cas_03.troy", unit, buff.Duration, 1, "Buffbone_Glb_WEAPON_1");
			AddParticleTarget(unit, unit, "xen_ziou_battleCry_cas_04.troy", unit, buff.Duration, 1, "Buffbone_Glb_WEAPON_1");
			AddParticleTarget(unit, unit, "xen_ziou_battleCry_cas_05.troy", unit, buff.Duration, 1);
            StatsModifier.AttackSpeed.PercentBonus = 0.4f + (0.1f * ownerSpell.CastInfo.SpellLevel);
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            
        }
    }
}