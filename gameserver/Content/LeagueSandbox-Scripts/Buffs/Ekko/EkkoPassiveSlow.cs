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
    public class EkkoPassiveSpeed : IBuffGameScript
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
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			thisBuff = buff;
            var owner = ownerSpell.CastInfo.Owner;		
			StatsModifier.MoveSpeed.PercentBonus = 0.2f + (0.1f * ownerSpell.CastInfo.SpellLevel);
			unit.AddStatModifier(StatsModifier);
            activate = AddParticleTarget(owner, unit, "Ekko_Base_P_Speed_line", unit, buff.Duration);
			activate2 = AddParticleTarget(owner, unit, "", unit, buff.Duration,1,"weapon");       
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(activate);
			RemoveParticle(activate2);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}