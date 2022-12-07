using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Scripting.CSharp;
using System;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class AlphaStrike: ISpellScript
    {
        public static IAttackableUnit Target = null;
		IBuff HandlerBuff;
        IMinion Shadow;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
			ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
			SetStatus(owner, StatusFlags.NoRender, false);
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			owner.CancelAutoAttack(false, true);
            owner.SetTargetUnit(null, true);	
			Target = target;
			AddBuff("AlphaStrikeTeleport", 3f, 1, spell, owner, owner);
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			SpellCast(owner, 2, SpellSlotType.ExtraSlots, false, Target, Vector2.Zero);
			owner.StopMovement();
        }
		public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;

            //SpellCast(owner, 2, SpellSlotType.ExtraSlots, false, target, missile.Position);

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
	public class AlphaStrikeBounce : ISpellScript
    {
		ISpell spell;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,       
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Chained,
                MaximumHits = 4
            }
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
            var owner = spell.CastInfo.Owner as IChampion;
            var spellLevel = owner.GetSpell("AlphaStrike").CastInfo.SpellLevel;              
            var ADratio = owner.Stats.AttackDamage.Total*0.9f;
            var damage =  -10 + 35 * spellLevel  + ADratio;
            AddParticleTarget(owner, target, "MasterYi_Base_Q_Tar.troy", target, 1f, 1f);
            AddParticleTarget(owner, target, ".MasterYi_Base_Q_Tar_Mark", target, 1f, 1f);
			AddParticleTarget(owner, target, "MasterYi_Base_Q_Hit.troy", target, 3f, 1f);
			AddParticleTarget(owner, target, "MasterYi_Base_Q_Crit_tar.troy", target, 3f, 1f);
			AddParticleTarget(owner, target, "MasterYi_Base_Q_Cas.troy", owner, 3f, 1f);
			AddParticleTarget(owner, target, ".MasterYi_Base_Q_Mis", target, 3f, 1f);
			target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
			ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }
		public void OnMissileEnd(ISpellMissile missile)
        {
			var owner = missile.CastInfo.Owner as IChampion;
			owner.RemoveBuffsWithName("AlphaStrikeTeleport");	        
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