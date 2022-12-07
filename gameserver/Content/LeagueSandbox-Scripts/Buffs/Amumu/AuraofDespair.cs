using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class AuraofDespair : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        private float DamageManaTimer;
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private ISpell originSpell;
        private float[] manaCost = { 8.0f, 8.0f, 8.0f, 8.0f, 8.0f };
        private IObjAiBase Owner;
        private IBuff thisBuff;
        public ISpellSector AuraAmumu;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Owner = ownerSpell.CastInfo.Owner;
            originSpell = ownerSpell;
            thisBuff = buff;
            ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, TargetExecute, false);

            AuraAmumu = ownerSpell.CreateSpellSector(new SectorParameters
            {
                BindObject = ownerSpell.CastInfo.Owner,
                Length = 300f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            float ap = Owner.Stats.AbilityPower.Total;
            float lvlmaxhp = (((0.0025f * (ap / 100)) + (0.00425f + 0.00075f * spell.CastInfo.SpellLevel)) * target.Stats.HealthPoints.Total);
            var damage = 4 + spell.CastInfo.SpellLevel * 2 + lvlmaxhp;

            target.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnSpellHit.RemoveListener(this);
            AuraAmumu.SetToRemove();
        }

        public void OnUpdate(float diff)
        {
            if (Owner != null && thisBuff != null && originSpell != null)
            {
                DamageManaTimer += diff;

                if (DamageManaTimer >= 500f)
                {
                    if (manaCost[originSpell.CastInfo.SpellLevel - 1] > Owner.Stats.CurrentMana)
                    {
                        RemoveBuff(thisBuff);
                    }
                    else
                    {
                        Owner.Stats.CurrentMana -= manaCost[originSpell.CastInfo.SpellLevel - 1];
                    }

                    DamageManaTimer = 0;
                }
            }
        }
    }
}