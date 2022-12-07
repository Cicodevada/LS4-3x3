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
    class AatroxESlow : IBuffGameScript
    {
		public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.SLOW,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IParticle p;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			var owner = ownerSpell.CastInfo.Owner;
            StatsModifier.MoveSpeed.PercentBonus = -0.4f;
			p = AddParticleTarget(owner, unit, "Aatrox_Base_E_Slow.troy", unit);
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			p.SetToRemove();
        }

        public void OnPreAttack(ISpell spell)
        {

        }

        public void OnUpdate(float diff)
        {
        }
    }
}