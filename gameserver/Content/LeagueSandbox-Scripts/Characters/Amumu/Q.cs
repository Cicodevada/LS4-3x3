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
    public class BandageToss : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        private IObjAiBase _owner;
        private ISpell _spell;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            _spell = spell;
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner as IChampion;
            var ownerSkinID = owner.SkinID;
            var targetPos = GetPointFromUnit(owner, 1150.0f);      
            FaceDirection(targetPos, owner);     
            SpellCast(owner, 0, SpellSlotType.ExtraSlots, targetPos, targetPos, false, Vector2.Zero);
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
            //SetSpellToolTipVar(_owner, 2, _owner.Stats.AttackDamage.Total * _spell.SpellData.AttackDamageCoefficient, SpellbookType.SPELLBOOK_CHAMPION, 0, SpellSlotType.SpellSlots);
        }
    }

    public class SadMummyBandageToss : ISpellScript
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
        IObjAiBase Owner;
        IAttackableUnit Target;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			Owner = spell.CastInfo.Owner;
			ApiEventManager.OnMoveEnd.AddListener(this, owner, OnMoveEnd, true);
			ApiEventManager.OnMoveSuccess.AddListener(this, owner, OnMoveSuccess, true);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
			Target = target;
            var owner = spell.CastInfo.Owner;
			var hitcoords = new Vector2(missile.Position.X, missile.Position.Y);
            var distance = Math.Sqrt(Math.Pow(owner.Position.X - hitcoords.X, 2) + Math.Pow(owner.Position.Y - hitcoords.Y, 2));
            if (Math.Abs(distance) <= 1150f)
            {
			    var ap = owner.Stats.AbilityPower.Total * 0.7f;
                var damage = 30 + owner.GetSpell(0).CastInfo.SpellLevel * 50 + ap;
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);       
                AddParticleTarget(owner, target, "BandageToss_tar", target);
			    AddBuff("Stun", 1f, 1, spell, target, owner);
                Dash(spell);				
                missile.SetToRemove();
            }
        }
        public void Dash(ISpell spell)
        {
		    var owner = spell.CastInfo.Owner;
			SetStatus(owner, StatusFlags.Ghosted, true);
			PlayAnimation(owner, "Spell2");			               
			ForceMovement(owner, null, Target.Position, 1400, 0, 0, 0);	
        }
		public void OnMoveEnd(IAttackableUnit owner)
        {
			SetStatus(Owner, StatusFlags.Ghosted, false);
            StopAnimation(Owner, "Spell2",true,true,true);
			//RemoveParticle(P);
        }
		public void OnMoveSuccess(IAttackableUnit unit)
        {        
            if (Owner.Team != Target.Team && Target is IChampion)
            {
                Owner.SetTargetUnit(Target, true);
                Owner.UpdateMoveOrder(OrderType.AttackTo, true);
            }
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
