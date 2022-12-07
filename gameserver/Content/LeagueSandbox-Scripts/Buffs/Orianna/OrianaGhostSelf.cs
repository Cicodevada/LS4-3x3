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
 * 
 * Known Issues:
 * Places a buff timer countdown despite this buff not having a timer. Need to figure out why
 * 
*/
//*=========================================

namespace Buffs
{
    class OrianaGhostSelf : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier ()
        {
        };

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var spellLevel = ownerSpell.CastInfo.SpellLevel - 1;
            var bonusResistances = new[] { 10, 15, 20, 25, 30 }[spellLevel];
            StatsModifier.Armor.FlatBonus = bonusResistances;
            StatsModifier.MagicResist.FlatBonus = bonusResistances;
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
