using System.Collections.Generic;
using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain;
using System;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class JavelinToss : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
			MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            TriggersSpellCasts = true,
            IsDamagingSpell = true
            // TODO
        };

        public float finaldamage;
        public Vector2 castcoords;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
			ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
            castcoords = spell.CastInfo.Owner.Position;
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var to = Vector2.Normalize(spellPos - castcoords);
            var range = to * 1500f;
            var trueCoords = castcoords + range;
            //spell.AddProjectile("JavelinToss", castcoords, castcoords, trueCoords, HitResult.HIT_Normal, true);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
			var owner = spell.CastInfo.Owner;
            var ap = owner.Stats.AbilityPower.Total;
            var basedamage = 25 + spell.CastInfo.SpellLevel * 55 + ap;
            var hitcoords = new Vector2(missile.Position.X, missile.Position.Y);
            var distance = Math.Sqrt(Math.Pow(castcoords.X - hitcoords.X, 2) + Math.Pow(castcoords.Y - hitcoords.Y, 2));
            if (Math.Abs(distance) <= 525f)
            {
                finaldamage = basedamage;
            }
            else if (distance > 525f && !(distance >= 1300f))
            {
                var damagerampup = (basedamage * (0.02f * (float)Math.Round(Math.Abs(distance - 525f) / 7.75f)));
                finaldamage = basedamage + damagerampup;
            }
            else if (distance >= 1300f)
            {
                finaldamage = (basedamage + (basedamage * 2));
            }
            target.TakeDamage(owner, finaldamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            if (!target.IsDead)
            {
                AddParticleTarget(owner, target, "Nidalee_Base_Q_Tar", target, bone: "C_BUFFBONE_GLB_CHEST_LOC");
            }

            missile.SetToRemove();
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

