using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class RunePrison : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.STUN,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; }

        IParticle p;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            buff.SetStatusEffect(StatusFlags.Stunned, true);
            p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "RunePrison_tar", unit, buff.Duration);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            SetStatus(unit, StatusFlags.Stunned, false);
            RemoveParticle(p);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}