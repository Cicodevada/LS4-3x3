using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class RengarE : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.SLOW,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IParticle p;
		IParticle p1;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            StatsModifier.MoveSpeed.PercentBonus -= 0.15f + 0.15f * (ownerSpell.CastInfo.SpellLevel);
            unit.AddStatModifier(StatsModifier);
            p = AddParticleTarget(ownerSpell.CastInfo.Owner, null, "Global_Slow.troy", unit, buff.Duration);
			p1 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Rengar_Base_E_Tar.troy", unit, buff.Duration);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
			RemoveParticle(p1);
        }

        public void OnPreAttack(ISpell spell)
        {

        }

        public void OnUpdate(float diff)
        {
        }
    }
}