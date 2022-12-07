using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System.Linq;

namespace Spells
{
    public class slashCast: ISpellScript
    {
        IObjAiBase Owner;
		IParticle P;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = false,
            NotSingleTargetSpell = true
        };
        public ISpellSector AOE;
		public List<IAttackableUnit> UnitsHit = new List<IAttackableUnit>();
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
            SetStatus(owner, StatusFlags.Ghosted, true);			
            ApiEventManager.OnMoveEnd.AddListener(this, owner, OnMoveEnd, true); 
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);			
        }   
        public void OnSpellCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			AOE = spell.CreateSpellSector(new SectorParameters
            {
                BindObject = owner,
                Length = 250f,
                Tickrate = 1,
                CanHitSameTargetConsecutively = true,
                OverrideFlags = SpellDataFlags.AffectEnemies | SpellDataFlags.AffectNeutral | SpellDataFlags.AffectMinions | SpellDataFlags.AffectHeroes,
                Type = SectorType.Area
            });
        }

        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			PlayAnimation(owner, "Spell3");
			var current = new Vector2(owner.Position.X, owner.Position.Y);
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var dist = Vector2.Distance(current, spellPos);

            if (dist > 625.0f)
            {
                dist = 625.0f;
            }

            FaceDirection(spellPos, owner, true);
            var trueCoords = GetPointFromUnit(owner, dist);
			P = AddParticleTarget(owner, owner, "Slash.troy", owner);
            ForceMovement(owner, "Spell3", trueCoords, 1400, 0, 0, 0);	
			
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;	
            var ap = owner.Stats.AttackDamage.Total * 1.3f;
            var damage = 80f + (30f * (spell.CastInfo.SpellLevel - 1)) + ap;
			if (!UnitsHit.Contains(target))
            {
            UnitsHit.Add(target);
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
			AddParticleTarget(owner, target, ".troy", owner);
			}

        }
        public void OnMoveEnd(IAttackableUnit owner)
        {
			SetStatus(owner, StatusFlags.Ghosted, false);
            StopAnimation(owner, "Spell3",true,true,true);
			RemoveParticle(P);
			AOE.SetToRemove();
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