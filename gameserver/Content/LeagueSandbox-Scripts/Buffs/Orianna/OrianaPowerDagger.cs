using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain;
using System;

//*=========================================
/*
 * ValkyrieHorns
 * Lastupdated: 3/21/2022
 * 
 * TODOS:
 * 
 * Known Issues:
*/
//*========================================

namespace Buffs
{
    class OrianaPowerDagger : IBuffGameScript
    {
        private IObjAiBase thisOwner;
        private ISpell thisSpell;
        private IBuff thisBuff;
        private float Damage = 0f;
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 2
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisOwner = ownerSpell.CastInfo.Owner;
            thisSpell = ownerSpell;
            thisBuff = buff;
            ApiEventManager.OnHitUnit.AddListener(this, thisOwner, TargetExecute, false);
        }

        private void TargetExecute(IDamageData data)
        {
            var damage = CalculatekDamage(thisOwner.Stats.Level) * thisBuff.StackCount;
            data.Target.TakeDamage(thisOwner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this, thisOwner);
        }

        public void OnPreAttack(ISpell spell)
        {
        }

        private float CalculatekDamage(int ownerLevel)
        {
            var baseDamage = 0f;

            if (ownerLevel >= 16)
            {
                baseDamage = 10f;
            }
            else if (ownerLevel >= 13)
            {
                baseDamage = 8.4f;
            }
            else if (ownerLevel >= 10)
            {
                baseDamage = 6.8f;
            }
            else if (ownerLevel >= 7)
            {
                baseDamage = 5.2f;
            }
            else if (ownerLevel >= 4)
            {
                baseDamage = 3.6f;
            }
            else if (ownerLevel >= 1)
            {
                baseDamage = 2f;
            }

            return baseDamage + (thisOwner.Stats.AbilityPower.Total * .03f);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
