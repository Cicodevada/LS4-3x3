using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class VeigarPrimordialBurst : ISpellScript
    {
        IStatsModifier statsModifier = new StatsModifier();

        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            }
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ownerSkinID = owner.SkinID;
            var APratio = owner.Stats.AbilityPower.Total * 1.2f;
            var targetAP = target.Stats.AbilityPower.Total * 0.8f;
            var damage = 250 + ((spell.CastInfo.SpellLevel -1) * 125) + APratio + targetAP;
            var StacksPerLevel = spell.CastInfo.SpellLevel;

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

            string particles;
            if (ownerSkinID == 8) 
            {
                particles = "Veigar_Skin08_R_tar.troy";
            }
            else
            {
                particles = "Veigar_Base_R_tar.troy";
            }

            if (!target.IsDead)
            {
                AddParticleTarget(owner, target, particles, target, 1f);
            }
            else
            {
                var buffer = owner.Stats.AbilityPower.FlatBonus;

                statsModifier.AbilityPower.FlatBonus += (StacksPerLevel + 2) - buffer;
                owner.AddStatModifier(statsModifier);

                if (ownerSkinID == 8)
                {
                    AddParticle(owner, target, "Veigar_Skin08_R_tar.troy", target.Position, 1f);

                }
                else
                {
                    AddParticle(owner, target, "Veigar_Base_R_tar.troy", target.Position, 1f);
                }
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
