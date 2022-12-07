using System.Linq;
using GameServerCore;
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
    public class KarthusFallenOne : ISpellScript
    {
        IObjAiBase Owner;
        float TimeSinceLastTick = 0;
        bool limiter = false;
        float damage;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            ChannelDuration = 3f,

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
            Owner = owner;
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var champions = GetChampionsInRange(owner.Position, 20000, true);
            for (int i = 0; i < champions.Count; i++)
            {
                if (champions[i].Team != owner.Team)
                {
                    AddParticleTarget(owner, champions[i], "Karthus_Base_R_Target.troy", champions[i], 3f);
                }
            }
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var APratio = Owner.Stats.AbilityPower.Total * 0.6f;
            damage = 100 + spell.CastInfo.SpellLevel * 150 + APratio;
            TimeSinceLastTick = 0;
            limiter = true;
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
            TimeSinceLastTick += diff;
            if (TimeSinceLastTick > 2700f && limiter == true &&  Owner != null)
            {
                var champions = GetChampionsInRange(Owner.Position, 20000, true);
                for (int i = 0; i < champions.Count; i++)
                {
                    if (champions[i].Team != Owner.Team && !champions[i].IsDead)
                    {
                        champions[i].TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                        AddParticleTarget(Owner, champions[i], "Karthus_Base_R_Explosion.troy", champions[i], 1f);
                    }
                }
                limiter = false;
            }
        }
    }
}
