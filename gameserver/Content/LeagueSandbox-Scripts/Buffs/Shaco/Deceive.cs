using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;

namespace Buffs
{
    internal class Deceive : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.RENEW_EXISTING
        };
        public BuffType BuffType => BuffType.DAMAGE;
        public BuffAddType BuffAddType => BuffAddType.RENEW_EXISTING;
        public int MaxStacks => 1;
        public bool IsHidden => false;

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IBuff thisBuff;
        IParticle p;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisBuff = buff;
            if (unit is IChampion champion)
            {
                ownerSpell.SetSpellToggle(true);
                ApiEventManager.OnPreAttack.AddListener(this, champion, OnPreAttack, true);
            }
            StatsModifier.CriticalChance.FlatBonus += 1f;
            StatsModifier.CriticalDamage.FlatBonus += -0.6f + (0.2f * (ownerSpell.CastInfo.SpellLevel - 1)); //Figure out later why this won't work
            unit.AddStatModifier(StatsModifier);
        }
        public void OnPreAttack(ISpell spell)
        {
            if (thisBuff != null)
            {
                thisBuff.DeactivateBuff();
            }
            RemoveParticle(p);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var champion = unit as IChampion;
            ownerSpell.SetCooldown(ownerSpell.GetCooldown());
            champion.GetSpell(ownerSpell.SpellName).SetSpellToggle(false);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}

