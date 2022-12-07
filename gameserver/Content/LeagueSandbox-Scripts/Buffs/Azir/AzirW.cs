using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;

namespace Buffs
{
    class AzirW : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IBuff ThisBuff;
		IObjAiBase Owner;
		IAttackableUnit U;
		private readonly IMinion Soldier = Spells.AzirW.Soldier;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			U = unit;
            ThisBuff = buff;
			Owner = ownerSpell.CastInfo.Owner;
			PlayAnimation(unit, "Spawn");
			Soldier.UpdateMoveOrder(OrderType.AttackTo, true);	
			FaceDirection(GetPointFromUnit(Owner, 1150.0f), unit,true);
			AddParticleTarget(Owner, unit, "Azir_Base_W_SpawnIn", Owner);
			AddParticleTarget(Owner, unit, "Azir_Base_W_Soldier_Dissipate", Owner);
			AddParticleTarget(Owner, unit, "Azir_Base_W_SoldierCape", Owner);
			AddParticleTarget(Owner, unit, "Azir_Base_W_SpawnIn", Owner);
			AddParticleTarget(Owner, unit, "Azir_Base_W_SoldierIndicator", Owner,10,1,"head","head");
            //AddParticleTarget(Owner, unit, "Azir_Base_W_Soldier_Cas", unit);类似于亚索Q的特效
            ApiEventManager.OnLaunchAttack.AddListener(this, Owner, OnLaunchAttack, false);
            ApiEventManager.OnSpellPostCast.AddListener(this, Owner.GetSpell("AzirQ"), QOnSpellPostCast);
            ApiEventManager.OnSpellCast.AddListener(this, Owner.GetSpell("AzirE"), EOnSpellCast);
            SealSpellSlot(Owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
			SealSpellSlot(Owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, false);			
        }
		public void QOnSpellPostCast(ISpell spell)
        {
			if (U != null && !U.IsDead)
            {
            var owner = spell.CastInfo.Owner;
			var current = new Vector2(Soldier.Position.X, Soldier.Position.Y);
            var spellPos = new Vector2(Owner.GetSpell("AzirQ").CastInfo.TargetPosition.X, Owner.GetSpell("AzirQ").CastInfo.TargetPosition.Z);
            var dist = Vector2.Distance(current, spellPos);

            if (dist > 1200.0f)
            {
                dist = 1200.0f;
            }

            FaceDirection(spellPos, Soldier, true);
            var trueCoords = GetPointFromUnit(Soldier, dist);
			PlayAnimation(Soldier, "Dash_Exit");
			PlayAnimation(Soldier, "Dash2");
			PlayAnimation(Soldier, "Attack1");
			AddParticleTarget(owner, Soldier, "Azir_Base_Q_SoldierMoveIndicator.troy", Soldier);
			AddParticleTarget(owner, Soldier, "Azir_base_Q_Tar.troy", Soldier);
			AddParticleTarget(owner, Soldier, "Azir_Base_Q_End.troy", Soldier,bone:"weapon");
			AddParticle(Soldier, null, ".troy", Soldier.Position);
            ForceMovement(Soldier, null, trueCoords, 1400, 0, 0, 0);	
            }			
        }
		public void EOnSpellCast(ISpell spell)
        {
			if (U != null && !U.IsDead)
            {
            var owner = spell.CastInfo.Owner;
			var current = new Vector2(owner.Position.X, owner.Position.Y);
            var spellPos = new Vector2(Owner.GetSpell("AzirE").CastInfo.TargetPosition.X, Owner.GetSpell("AzirE").CastInfo.TargetPosition.Z);        
			PlayAnimation(owner, "Spell3");
			AddParticleTarget(owner, owner, ".troy", owner);
			AddParticleTarget(owner, owner, ".troy", owner);
			AddParticleTarget(owner, owner, ".troy", owner);
			AddParticle(owner, null, ".troy", owner.Position);
            ForceMovement(owner, null, Soldier.Position, 1700, 0, 0, 0);	
            }			
        }
		public void OnLaunchAttack(ISpell spell)
        {
			if(Soldier is IMinion S)
            {
			    S.SetTargetUnit(spell.CastInfo.Targets[0].Unit, true);
			}				
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			SealSpellSlot(Owner, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true);
			SealSpellSlot(Owner, SpellSlotType.SpellSlots, 2, SpellbookType.SPELLBOOK_CHAMPION, true);
			AddParticle(Owner, null, "Azir_Base_W_Soldier_Outline", unit.Position);
            if (unit != null && !unit.IsDead)
            {
				unit.TakeDamage(unit, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
            }
        }
        public void OnUpdate(float diff)
        {          
        }
    }
}