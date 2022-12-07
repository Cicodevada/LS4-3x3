using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Collections.Generic;
using System.Numerics;
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
    public class NinanPassiveBleed : IBuffGameScript
    {
		public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; }
        IAttackableUnit Unit;
        IObjAiBase owner;
        IParticle p;
        IParticle p1;
		IParticle p2;
		IBuff thisbuff;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {     
            p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Talon_Base_P_Stack_3.troy", ownerSpell.CastInfo.Owner, buff.Duration);
        }    
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
			RemoveParticle(p1);
			RemoveParticle(p2);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}