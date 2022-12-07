using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class GarenR : ISpellScript
    {
        IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
            TriggersSpellCasts = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var PercentMissingHP = new[] { 0.2857f, 0.3333f, 0.4f }[spell.CastInfo.SpellLevel];
            var damage = 175f * spell.CastInfo.SpellLevel + PercentMissingHP * (Target.Stats.HealthPoints.Total - Target.Stats.CurrentHealth);

            
            AddParticleTarget(owner, Target, "Garen_Base_R_Sword_Tar.troy", Target, lifetime: 1f);

            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

            AddParticle(owner, Target, "Garen_Base_R_Tar_Impact.troy", Target.Position, 1f);
            if (Target.IsDead)
            {
                AddParticleTarget(owner, Target, "Garen_Base_R_Champ_Kill.troy", Target, 1f);
                AddParticleTarget(owner, Target, "Garen_Base_R_Champ_Death.troy", Target, 1f);
            }

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
