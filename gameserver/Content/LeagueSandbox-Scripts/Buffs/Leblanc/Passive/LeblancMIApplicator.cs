using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using System.Numerics;
using GameServerCore;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain;

namespace Buffs
{
    internal class LeblancMIApplicator : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IMinion Leblanc;
        ISpell Spell;
		private IBuff Buff;
		IObjAiBase Owner;
		IAttackableUnit Unit;
		float timeSinceLastTick = 500f;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Buff = buff;
			Unit = unit;
			Spell = ownerSpell;
			Owner = ownerSpell.CastInfo.Owner;
			if (ownerSpell.CastInfo.Owner is IChampion owner)
            {
			   AddParticleTarget(owner, unit, "LeBlanc_Base_P_poof", owner);
			   ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
		       AddParticleTarget(owner, unit, "LeBlanc_Base_P_image", owner, 25000f);		  		   
			}
			ApiEventManager.OnDeath.AddListener(this, unit, OnDeath, true);
            
        }
		public void OnLaunchAttack(ISpell spell)
        {
			
			if (Buff != null && Buff.StackCount != 0 && !Buff.Elapsed())
            {                              
               if (Unit is IPet Leblanc)
               {
                  Leblanc.SetTargetUnit(spell.CastInfo.Targets[0].Unit, true);
               }			
			}
        }
		public void OnDeath(IDeathData data)
        {           
            if (Buff != null && !Buff.Elapsed())
            {
                Buff.DeactivateBuff();
            }
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            if (ownerSpell.CastInfo.Owner is IChampion owner)
            {
            ApiEventManager.OnLaunchAttack.RemoveListener(this);				
			unit.Die(CreateDeathData(false, 0, unit, unit, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, 0.0f));
			AddParticle(owner, unit, "LeBlanc_Base_P_imageDeath", unit.Position);
            }			
        }
        public void OnUpdate(float diff)
        {
			timeSinceLastTick += diff;

            if (timeSinceLastTick >= 500.0f && Unit != null)
            {
                ForceMovement(Unit, null, Owner.Position, 450, 0, 0, 0);
                timeSinceLastTick = 0f;
            }
			
        }
    }
}