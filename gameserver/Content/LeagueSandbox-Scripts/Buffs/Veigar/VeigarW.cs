using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using System.Collections.Generic;
using GameServerCore.Domain.GameObjects.Spell.Missile;

namespace Buffs
{
    class VeigarW : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.STACKS_AND_OVERLAPS,
            MaxStacks = byte.MaxValue
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        ISpellSector DamageSector;
        string particles2;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, ownerSpell, TargetExecute, true);

            DamageSector = ownerSpell.CreateSpellSector(new SectorParameters
            {
                Length = 225f,
                Lifetime = -1.0f,
                Tickrate = 0,
                SingleTick = true,
                OverrideFlags = SpellDataFlags.AffectMinions | SpellDataFlags.AffectEnemies | SpellDataFlags.AffectFriends | SpellDataFlags.AffectBarracksOnly,
                Type = SectorType.Area
            });

            switch ((unit as IObjAiBase).SkinID)
            {
                case 8:
                    particles2 = "Veigar_Skin08_W_aoe_explosion.troy";
                    break;

                case 4:
                    particles2 = "Veigar_Skin04_W_aoe_explosion.troy";
                    break;

                default:
                    particles2 = "Veigar_Base_W_aoe_explosion.troy";
                    break;
            }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            DamageSector.ExecuteTick();
            AddParticle(ownerSpell.CastInfo.Owner, null, particles2, DamageSector.Position);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            if (!(target is IBaseTurret || target is ILaneTurret || target.Team == owner.Team || target == owner))
            {
                var ownerSkinID = owner.SkinID;
                var APratio = owner.Stats.AbilityPower.Total;
                var damage = 120f + ((spell.CastInfo.SpellLevel - 1) * 50) + APratio;
                var StacksPerLevel = spell.CastInfo.SpellLevel;

                target.TakeDamage(spell.CastInfo.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            }
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
