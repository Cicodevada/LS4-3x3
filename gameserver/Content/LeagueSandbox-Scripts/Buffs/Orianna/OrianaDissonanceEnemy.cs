using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;

//*=========================================
/*
 * ValkyrieHorns
 * Lastupdated: 3/20/2022
 * 
 * TODOS:
 * Add particles for orianna W ms slow
 * 
 * Known Issues:
 * Buff Icon for this buff does not show up.
 * Maybe class name used client side is different compared to the ones listed within the severside files?
 * Also possible that during this patch they didn't have icons? Need to find old Orianna replay file to confirm this issue.
*/
//*========================================

namespace Buffs
{
    class OrianaDissonanceEnemy : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_DEHANCER,
            BuffAddType = BuffAddType.RENEW_EXISTING,
            MaxStacks = 1
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier ()
        {
        };

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            StatsModifier.MoveSpeed.PercentBonus = - new[] { .2f, .25f, .3f, .35f, .4f }[ownerSpell.CastInfo.SpellLevel - 1];
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnPreAttack(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
