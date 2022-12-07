using System;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;

namespace Spells
{
    public class Imbue : ISpellScript
    {
        IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
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
            var APratio = owner.Stats.AbilityPower.Total * 0.3f;
            var HPratio = (owner.Stats.HealthPoints.Total - owner.Stats.HealthPoints.BaseValue) * 0.05f;
            float Heal = 20f + spell.CastInfo.SpellLevel * 40f + APratio + HPratio;

            if (Target == owner)
            {
                Target.Stats.CurrentHealth += (Heal * 1.4f);
            }
            else
            {
                owner.Stats.CurrentHealth += Heal;
                Target.Stats.CurrentHealth += Heal;

                AddParticleTarget(owner, Target, "Global_Heal.troy", Target, 1f);
            }
            AddParticleTarget(owner, owner, "Global_Heal.troy", owner, 1f);
            AddParticleTarget(owner, owner, "Imbue_glow.troy", owner, 1f, 0.2f);
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

