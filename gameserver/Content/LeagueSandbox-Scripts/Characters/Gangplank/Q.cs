using System;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class Parley : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true
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

        public void OnSpellCast(ISpell spell)
        {
         //TODO:Fix broken SpellCast animation
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var damage = -5f + (25f * spell.CastInfo.SpellLevel) + owner.Stats.AttackDamage.Total;
            var isCrit = new Random().Next(0, 100) <= (owner.Stats.CriticalChance.Total * 100f);
            var goldIncome = new[] { 4, 5, 6, 7, 8 }[spell.CastInfo.SpellLevel];
            bool IsCritBool = false;

            if (isCrit == true)
            {
                damage *= owner.Stats.CriticalDamage.Total;
                IsCritBool = true;
            }

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, IsCritBool);
            AddParticleTarget(owner, target, "pirate_parley_tar.troy", target, lifetime: 1f); //TODO: Fix particles that for some reason aren't spawning ||||| Test if particles now work

            if (target.IsDead)
            {
                owner.Stats.Gold += goldIncome;
                owner.Stats.CurrentMana += spell.CastInfo.ManaCost;
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
