using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;


namespace Buffs
{
    internal class BlindMonkETwoMissile : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IParticle p;
		IParticle p2;
		IParticle p3;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            StatsModifier.MoveSpeed.PercentBonus -= 0.05f + 0.15f * (ownerSpell.CastInfo.SpellLevel);
            unit.AddStatModifier(StatsModifier);
            p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "blindMonk_E_tempestFist_cripple.troy", unit, buff.Duration);
			p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "blindMonk_E_tempestFist_cripple_02.troy", unit, buff.Duration);
			p3 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, ".troy", unit, buff.Duration);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
        }

        public void OnPreAttack(ISpell spell)
        {

        }

        public void OnUpdate(float diff)
        {
        }
    }
}