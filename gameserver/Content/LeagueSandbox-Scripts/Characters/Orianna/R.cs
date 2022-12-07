using System;
using System.Numerics;
using System.Collections.Generic;

using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Sector;

using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects.Spell.Missile;

//*=========================================
/*
 * ValkyrieHorns
 * Lastupdated: 3/20/2022
 * 
 * TODOS:
 * Find way to implement the knockup component, pretty sure Yasuo can ult off of Orianna ult.
 * Or was that only added way later?
 * 
 * ==OrianaDetonateCommand==
 * 
 *= =OrianaDissonanceWave==
 * 
 * Known Issues:
 * 
*/
//*========================================

namespace Spells
{
    public class OrianaDetonateCommand : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        private IObjAiBase _orianna;
        private ISpell _spell;

        private Buffs.OriannaBallHandler _ballHandler;

        private bool _queuedCast = false;

        private ISpellSector _detonateSector;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _orianna = owner;
            _spell = spell;
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            _ballHandler = (_orianna.GetBuffWithName("OriannaBallHandler").BuffScript as Buffs.OriannaBallHandler);
        }

        public void OnSpellCast(ISpell spell)
        {
            if (_ballHandler.GetIsAttached())
            {
                var attachPos = _ballHandler.GetAttachedChampion().Position;
                SpellCast(_orianna, 3, SpellSlotType.ExtraSlots, attachPos, attachPos, false, attachPos, overrideForceLevel: spell.CastInfo.SpellLevel);
            }
            else
            {
                if (_ballHandler.GetFlightState())
                {
                    _queuedCast = true;
                }
                else
                {
                    var ballPos = _ballHandler.GetBall().Position;
                    SpellCast(_orianna, 3, SpellSlotType.ExtraSlots, ballPos, ballPos, false, ballPos, overrideForceLevel: spell.CastInfo.SpellLevel);
                }
            }
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
            var spellLevel = spell.CastInfo.SpellLevel - 1;
            var coolDown = new[] { 120f, 105f, 90f }[spellLevel];
            spell.SetCooldown(coolDown);

            var manaCost = new[] { 100, 125, 150 }[spellLevel];
            _orianna.Stats.CurrentMana -= manaCost;
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

    public class OrianaDissonanceWave : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            NotSingleTargetSpell = true,
        };

        private IObjAiBase _orianna;
        private ISpell _spell;
        private ISectorParameters _enemySector;

        private Buffs.OriannaBallHandler _ballHandler;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _orianna = owner;
            _spell = spell;
            ApiEventManager.OnSpellHit.AddListener(this,spell,TargetExecute,false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Console.WriteLine("Triggered: OrianaDissonanceWave");
            _ballHandler = (_orianna.GetBuffWithName("OriannaBallHandler").BuffScript as Buffs.OriannaBallHandler);
        }

        public void OnSpellCast(ISpell spell)
        {
            if (_ballHandler.GetIsAttached())
            {
                ExcuteSpell(_ballHandler.GetAttachedChampion().Position);
            }
            else
            {
                ExcuteSpell(_ballHandler.GetBall().Position);
            }
        }

        public void OnSpellPostCast(ISpell spell)
        {
            _outerSector.SetToRemove();
        }

        private void ExcuteSpell(Vector2 position)
        {
            var tempMinion = AddMinion(_orianna, "TestCubeRender", "OriannaRMarker", position, _orianna.Team, ignoreCollision: true, targetable: false);
            AddBuff("ExpirationTimer", 1.0f, 1, _spell, tempMinion, _orianna);

            _outerSector = _spell.CreateSpellSector(new SectorParameters
            {
                BindObject = tempMinion,
                Length = 600,
                CanHitSameTarget = false,
                CanHitSameTargetConsecutively = false,
                Type = SectorType.Area,
                SingleTick = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
            });
        }

        private ISpellSector _outerSector;

        private void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            if (_ballHandler.GetIsAttached())
            {
                target.FaceDirection(new Vector3(_ballHandler.GetAttachedChampion().Position.X, 0, _ballHandler.GetAttachedChampion().Position.Y));
            }
            else
            {
                target.FaceDirection(new Vector3(_ballHandler.GetBall().Position.X, 0, _ballHandler.GetBall().Position.Y));
            }

            //AddBuff("Stun", .75f, 1, _spell, target, _orianna);
            
            //ForceMovement(target, "STUNNED", _ballHandler.GetBall().Position, 800f, 500f, .4f, 0f);

            var spellLevel = spell.CastInfo.SpellLevel - 1;
            var baseDamage = new[] { 150, 225, 300, }[spellLevel];
            var magicDamage = _orianna.Stats.AbilityPower.Total * .7f;
            var finalDamage = baseDamage + magicDamage;

            //TODO: Find ult hit particle
            //target.TakeDamage(_orianna, finalDamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);

        }

        private void ApplyUlt(float idealDistance)
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
