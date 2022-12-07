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
    public class ZedRHandler : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IBuff ThisBuff;
        IParticle p;
		IParticle p2;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {       
            if (unit is IObjAiBase owner)
            {                
               owner.SetSpell("ZedR2", 3, true);
            }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {   
             RemoveParticle(p2);
             (unit as IObjAiBase).SetSpell("ZedUlt", 3, true);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}