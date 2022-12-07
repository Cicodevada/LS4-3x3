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
    public class AhriTumble : ISpellScript
    {
		ISpell Spell;
		IObjAiBase Owner;
		IAttackableUnit Target1;
        IAttackableUnit Target2;
        IAttackableUnit Target3;
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
			Spell = spell;
			Owner = spell.CastInfo.Owner;;
			PlayAnimation(owner, "Spell4");
			SetStatus(Owner, StatusFlags.Ghosted, true); 
			ApiEventManager.OnMoveEnd.AddListener(this, owner, OnMoveEnd, true);
			var current = new Vector2(owner.Position.X, owner.Position.Y);
            var dist = Vector2.Distance(current, start);

            FaceDirection(start, owner, true);

            if (dist > spell.SpellData.CastRangeDisplayOverride)
            {
                start = GetPointFromUnit(owner, spell.SpellData.CastRangeDisplayOverride);
            }
			ForceMovement(owner, null, start, 1700, 0, 0, 0);
			AddParticleTarget(owner, owner, "Ahri_SpiritRush_cas", owner);
			AddParticleTarget(owner, owner, "Ahri_SpiritRush_mis", owner);        

        }

        public void OnSpellCast(ISpell spell)
        {
        }
		public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner as IChampion;
			AddBuff("AhriTumble", 15.0f, 1, spell, owner, owner);
        }
		public void OnMoveEnd(IAttackableUnit owner)
        {
			Shot(Spell);
			//Owner.SetTargetUnit(Target, true);
			SetStatus(Owner, StatusFlags.Ghosted, false);         
            StopAnimation(Owner, "Spell4",true,true,true);
        }    
        public void Shot(ISpell spell)
        {
			var champs = GetChampionsInRange(Owner.Position, 500f, true).OrderBy(enemy => Vector2.Distance(enemy.Position, Owner.Position)).ToList();
            if (champs.Count > 3)
            {
                foreach (var enemy in champs.GetRange(0, 4)
                     .Where(x => x.Team == CustomConvert.GetEnemyTeam(Owner.Team)))
                {
                    if (Target1 == null) Target1 = enemy;
                    else if (Target2 == null) Target2 = enemy;
                    else if (Target3 == null) Target3 = enemy;                 
                }
            }
            else
            {
                foreach (var enemy in champs.GetRange(0, champs.Count)
                    .Where(x => x.Team == CustomConvert.GetEnemyTeam(Owner.Team)))
                {
                    if (Target1 == null) Target1 = enemy;
                    else if (Target2 == null) Target2 = enemy;
                    else if (Target3 == null) Target3 = enemy;                 
                }
            }
			if (Target1 != null) SpellCast(Owner, 5, SpellSlotType.ExtraSlots, true, Target1, Vector2.Zero);
			if (Target2 != null) SpellCast(Owner, 5, SpellSlotType.ExtraSlots, true, Target2, Vector2.Zero);
			if (Target3 != null) SpellCast(Owner, 5, SpellSlotType.ExtraSlots, true, Target3, Vector2.Zero);
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
	public class AhriTumbleMissile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            },
            TriggersSpellCasts = true

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
            var owner = spell.CastInfo.Owner;
			var ownerSkinID = owner.SkinID;
            float ad = (owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue) * 0.5f;
            float damage = 35 + (owner.GetSpell(3).CastInfo.SpellLevel-1 - 1) * 20 + ad;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            AddParticleTarget(owner, target, "Ahri_SpiritRush_tar.troy", target);
			//AddParticleTarget(Owner, Unit, "Talon_Base_Q2_tar.troy", Unit);		   
			missile.SetToRemove();
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
