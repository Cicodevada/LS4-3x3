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
    public class PantheonQ : ISpellScript
    {
        int ticks;
        IObjAiBase Owner;
        IStatsModifier statsModifier = new StatsModifier();
        ISpell Spell;
        float stacks;
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
            Owner = owner;

        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ownerSkinID = owner.SkinID;
            var APratio = owner.Stats.AttackDamage.Total * 0.6f;
            var damage = 65f + ((spell.CastInfo.SpellLevel - 1) * 40) + APratio;
            var StacksPerLevel = spell.CastInfo.SpellLevel;

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
			AddParticleTarget(owner, target, "Pantheon_Base_Q_tar.troy", target, 1f);
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
            Owner.Stats.ManaRegeneration.FlatBonus = Owner.Stats.ManaRegeneration.BaseValue * ((100 / Owner.Stats.ManaPoints.Total) * ((Owner.Stats.ManaPoints.Total - Owner.Stats.CurrentMana) / 100)); //I'm too lazy to make this properly
        }
    }
}
