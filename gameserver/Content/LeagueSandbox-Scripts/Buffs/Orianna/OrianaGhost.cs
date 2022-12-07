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
using GameServerCore.Domain;

//*=========================================
/*
 * ValkyrieHorns
 * Lastupdated: 3/20/2022
 * 
 * TODOS:
 * Figure out if Orianna's Ally Ring is: 
 * Visible to her or not in this patch.
 * Visible to the rest of her team or not.
 * Wait for LeagueSandbox GamerServer to implement Stealth to hide E particle. 
 * Add in check for if buff holder is outside of ball leash range and cast OriannReturn if they are.
 * 
 * Live Severs she is able to see the ring.
 * 
 * Known Issues:
 * Places a buff timer countdown despite this buff not having a timer. Need to figure out why
 * 
*/
//*=========================================

namespace Buffs
{
    class OrianaGhost : IBuffGameScript
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

        IObjAiBase _orianna;
        Buffs.OriannaBallHandler _ballHandler;
        IParticle _bind;
        IParticle _ring;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            _orianna = ownerSpell.CastInfo.Owner;
            _ballHandler = (_orianna.GetBuffWithName("OriannaBallHandler").BuffScript as Buffs.OriannaBallHandler);
            ApiEventManager.OnDeath.AddListener(this, unit, TargetExecute, false);

            var spellLevel = ownerSpell.CastInfo.SpellLevel - 1;
            var bonusResistances = new[] { 10, 15, 20, 25, 30 }[spellLevel];
            StatsModifier.Armor.FlatBonus = bonusResistances;
            StatsModifier.MagicResist.FlatBonus = bonusResistances;
            unit.AddStatModifier(StatsModifier);

            _bind = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Oriana_Ghost_bind", unit, 2300f, flags: FXFlags.TargetDirection);
            _ring = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "OriannaEAllyRangeRing", unit, 2300f, flags: FXFlags.TargetDirection,teamOnly: ownerSpell.CastInfo.Owner.Team);
        }

        private void TargetExecute(IDeathData obj)
        {
            _ballHandler.GetAttachedChampion().RemoveBuffsWithName("OrianaGhost");
            _ballHandler.DropBall();
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnDeath.RemoveListener(this);
            _bind.SetToRemove();
            _ring.SetToRemove();
        }

        public void OnPreAttack(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
