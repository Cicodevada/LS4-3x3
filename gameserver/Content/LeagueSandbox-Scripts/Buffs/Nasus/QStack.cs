using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    internal class NasusQStacks : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 999999
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();    
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //ApiEventManager.OnKillUnit.AddListener(this, AtOwner, NasusMoreStacks, true);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            //ApiEventManager.OnHitUnit.RemoveListener(this);

        }

        public void OnUpdate(float diff)
        {

        }
    }
}