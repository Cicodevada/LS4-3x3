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
using GameServerCore.Domain.GameObjects.Spell.Missile;

namespace Buffs
{
    class ClockworkWinding : IBuffGameScript
    {
        private IObjAiBase _owner;
        private ISpell _spell;
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 1
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            _owner = ownerSpell.CastInfo.Owner;
            _spell = ownerSpell;
            ApiEventManager.OnHitUnit.AddListener(this, _owner, TargetExecute, false);
        }

        private void TargetExecute(IDamageData data)
        {
            data.Target.TakeDamage(_owner, CalculateDamage(), DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false); 
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnHitUnit.RemoveListener(this, _owner);
        }

        private float CalculateDamage()
        {
            var ownerLevel = _owner.Stats.Level;
            var baseDamage = 0;

            if (ownerLevel >= 16)
            {
                baseDamage = 50;
            }
            else if (ownerLevel >= 13)
            {
                baseDamage = 42;
            }
            else if (ownerLevel >= 10)
            {
                baseDamage = 34;
            }
            else if (ownerLevel >= 7)
            {
                baseDamage = 26;
            }
            else if (ownerLevel >= 4)
            {
                baseDamage = 18;
            }
            else if (ownerLevel >= 1)
            {
                baseDamage = 10;
            }

            return baseDamage + (_owner.Stats.AbilityPower.Total * .15f);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
