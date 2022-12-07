using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Linq;
using GameServerCore;
using System.Numerics;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class KarthusLayWasteA1 : ISpellScript
    {
        float TimeSinceLastTick;
        Vector2 coords;
        IObjAiBase Owner;
        float damage;
        bool isCrit = false;
        bool limiter = false;
        string enemiesHit = "Karthus_Base_Q_Hit_Many.troy";
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
            Owner = owner;
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            coords = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var APratio = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.3f;
            damage = 20f + (spell.CastInfo.SpellLevel * 20f) + APratio;
            TimeSinceLastTick = 0;
            isCrit = false;
            limiter = true;

            AddParticle(Owner, Owner, "Karthus_Base_Q_Point.troy", coords, 1f);
            AddParticle(Owner, Owner, "Karthus_Base_Q_Ring.troy", coords, 1f);
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
            if (TimeSinceLastTick > 500f &&  limiter == true && coords != null && Owner != null)
            {
                var units = GetUnitsInRange(coords, 200f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (!(units[i].Team == Owner.Team || units[i] is IBaseTurret || units[i] is IObjBuilding || units[i] is IInhibitor))
                    {
                        if (units.Count == 1)
                        {
                            damage *= 2;
                            isCrit = true;
                            enemiesHit = "Karthus_Base_Q_Hit_Single.troy";
                        }
                        AddParticleTarget(Owner, units[i], enemiesHit, units[i], 1f);

                        units[i].TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, isCrit);
                    }

                }
                AddParticle(Owner, Owner, "Karthus_Base_Q_Explosion.troy", coords, 1f, 0.75f); //Double Check the size
                AddParticle(Owner, Owner, "Karthus_Base_Q_Explosion_Sound.troy", coords, 1f);
                limiter = false;       
                //TODO: Fix Towers, inhibs, buildings, etc. causing Q to not Crit 
            }
        }
    }
}
