using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameMaths;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class OlafAxeThrowCast : ISpellScript
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
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner as IChampion;
            var current = owner.Position;
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var to = Vector2.Distance(current, spellPos);
            Vector2 trueCoords;

            if (to < 400f)
            {
                trueCoords = GetPointFromUnit(owner, 400f);
            }
            else if (to > 1000f)
            {
                trueCoords = GetPointFromUnit(owner, 1000f);
            }
            else
            {
                trueCoords = spellPos;
            }

            SpellCast(owner, 0, SpellSlotType.ExtraSlots, trueCoords, trueCoords, false, Vector2.Zero);
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
    public class OlafAxeThrow : ISpellScript
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

        //Vector2 direction;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
            //ApiEventManager.OnMissileEnd.AddListener(this, spell, OnMissileEnd, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var ADratio = owner.Stats.AttackDamage.Total * spell.SpellData.AttackDamageCoefficient;
            var APratio = owner.Stats.AbilityPower.Total * spell.SpellData.MagicDamageCoefficient;
            var damage = 15 + spell.CastInfo.SpellLevel * 20 + ADratio + APratio;

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            AddBuff("OlafAxeThrow", 2f, 1, spell, target, owner); //TODO: Find Proper Buff name

            AddParticleTarget(owner, target, "olaf_axeThrow_tar.troy", target, 1f);
            AddParticleTarget(owner, target, "olaf_axeThrow_tar_02.troy", target, 1f);
            AddParticleTarget(owner, target, "olaf_axeThrow_tar_03.troy", target, 1f);

        }
        /*public void OnMissileEnd(ISpell spell, ISpellMissile missileSpell)
        {
            var axe = AddMinion(spell.CastInfo.Owner, "OlafAxe", "OlafAxe", missileSpell.Position);
            AddParticle(axe, "olaf_axeThrow_tar.troy", axe.Position, lifetime: 1f);
            AddParticle(axe, "olaf_axeThrow_tar_02.troy", axe.Position, lifetime: 1f);
            AddParticle(axe, "olaf_axeThrow_tar_03.troy", axe.Position, lifetime: 1f);
        }*/

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

