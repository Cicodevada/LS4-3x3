using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class FizzJump : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IAttackableUnit Unit;
        IObjAiBase owner;
        IParticle p;
		IBuff thisBuff;
        IParticle p2;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			thisBuff = buff;
            owner = ownerSpell.CastInfo.Owner as IChampion;
            Unit = unit;
			owner.StopMovement(); 
            owner.SetTargetUnit(null, true);		
			owner.SetSpell("FizzJumpTwo", 2, true);
            unit.Stats.SetActionState(ActionState.CAN_MOVE, false);			
            p = AddParticleTarget(owner, unit, "", unit, buff.Duration, 1f);
            p2 = AddParticleTarget(owner, unit, "", unit, buff.Duration, 1f);
			ApiEventManager.OnSpellCast.AddListener(this, owner.GetSpell("FizzJumpTwo"), E2OnSpellCast);
			if (unit.IsDead)
            {
			RemoveParticle(p);
			RemoveBuff(thisBuff);
            RemoveParticle(p2);
			}
        }
		public void E2OnSpellCast(ISpell spell)
        {   		
            RemoveBuff(thisBuff);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			PlayAnimation(owner, "spell3c");
			AddBuff("FizzTrickSlamSoundDummy", 1f, 1, ownerSpell, owner, owner);
			unit.Stats.SetActionState(ActionState.CAN_MOVE, true);	 
			AddParticleTarget(owner, owner, ".troy", owner, 0.5f);
			AddParticle(owner, null, ".troy", owner.Position);
			CreateTimer((float) 0.5f , () =>
            {
			var AP = owner.Stats.AbilityPower.Total * 0.75f;
			var RLevel = owner.GetSpell(2).CastInfo.SpellLevel;
			var damage = 70 + (50 * (RLevel - 1)) + AP;
            if (owner.HasBuff("FizzJumpTwo"))
            {				
			var units = GetUnitsInRange(owner.Position, 450f, true);
			for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != owner.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
                        units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
					    AddParticleTarget(owner, units[i], ".troy", units[i], 1f);
				        AddParticleTarget(owner, units[i], "Fizz_TrickSlam_tar.troy", units[i], 1f);
                    }
                }      
			AddParticleTarget(owner, owner, "Fizz_TrickSlam.troy", owner);
			}
			else
			{				
			var units = GetUnitsInRange(owner.Position, 250f, true);
			for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != owner.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
                        units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
					    AddParticleTarget(owner, units[i], ".troy", units[i], 1f);
				        AddParticleTarget(owner, units[i], "Fizz_TrickSlam_tar.troy", units[i], 1f);
                    }
                }      
			AddParticleTarget(owner, owner, "Fizz_TrickSlamTwo.troy", owner);
			}       
            });			
			owner.SetSpell("FizzJump", 2, true);
            RemoveParticle(p);
			RemoveBuff(thisBuff);
            RemoveParticle(p2);
        }
        public void OnUpdate(float diff)
        {        
        }
    }
}