using System.Numerics;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class EzrealEssenceFlux : ISpellScript
    {
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
        }

        public void OnSpellCast(ISpell spell)
        {
            AddParticleTarget(spell.CastInfo.Owner, spell.CastInfo.Owner, "ezreal_bow_yellow.troy", spell.CastInfo.Owner, 1f, bone: "L_HAND");
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner as IChampion;
            var trueCoords = GetPointFromUnit(owner, 1000f);

            SpellCast(owner, 2, SpellSlotType.ExtraSlots, trueCoords, trueCoords, false, Vector2.Zero);
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
    public class EzrealEssenceFluxMissile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true
            // TODO
        };


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

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var champion = target as IChampion;
            var owner = spell.CastInfo.Owner as IChampion;
            var spellLevel = owner.GetSpell("EzrealEssenceFlux").CastInfo.SpellLevel;
            if (champion == null)
            {
                return;
            }

            if (champion.Team == owner.Team && champion != owner)
            {
                AddBuff("EzrealEssenceFlux", 5f, 1, spell, champion, owner);
                AddBuff("EzrealRisingSpellForce", 6f, 1, spell, owner, owner);
            }
            else if (champion == owner) //TODO: Fix getting self proc at cast (you are supposed to have to E/Flash into it in order to get the buff i think)
            {
                AddBuff("EzrealEssenceFlux", 5f, 1, spell, champion, owner);
            }
            else
            {
                var APratio = owner.Stats.AbilityPower.Total * 0.8f;
                var damage = 25 + (45 * spellLevel) + APratio;

                champion.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddBuff("EzrealRisingSpellForce", 6f, 1, spell, owner, owner);
            }
            AddParticleTarget(owner, champion, "Ezreal_essenceflux_tar.troy", champion, lifetime: 1f);
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
