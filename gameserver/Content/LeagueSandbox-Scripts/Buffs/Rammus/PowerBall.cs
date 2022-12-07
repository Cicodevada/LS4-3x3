using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Domain.GameObjects.Spell.Missile;


namespace Buffs
{
    class PowerBall : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
		float Speed;
		IAttackableUnit Target;
        private ISpell spell;
		IBuff ibuff;
		IObjAiBase owner;
		float DamageManaTimer;
		float T = 0.15f;
		float M;
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			owner = ownerSpell.CastInfo.Owner;
			ibuff = buff;
			spell = ownerSpell;
			if (unit.Model == "Rammus")
            {
                unit.ChangeModel("RammusPB");
            }

            if (unit is IObjAiBase obj)
            { 
		            StatsModifier.MoveSpeed.PercentBonus += T;
					obj.AddStatModifier(StatsModifier);
            }
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			AOE(ownerSpell);
			if (unit.Model == "RammusPB")
            {
               unit.ChangeModel("Rammus");
            }
			if (unit is IObjAiBase obj)
            { 
                    ApiEventManager.OnPreAttack.RemoveListener(this, obj as IObjAiBase);
                    ApiEventManager.OnCollision.RemoveListener(this, obj as IObjAiBase);						
               				
            }
			CreateTimer((float) 0.0001 , () =>
            {
			PlayAnimation(unit, "spell1",0.3f);
			});	
        }
		public void AOE(ISpell spell)
        {
             var owner = spell.CastInfo.Owner;
			 var ap = owner.Stats.AbilityPower.Total * 0.8f;
             var damage = 70 + spell.CastInfo.SpellLevel * 45 + ap;
			 AddParticleTarget(owner, owner, "PowerBallStop", owner);
			 var units = GetUnitsInRange(GetPointFromUnit(owner, 125f), 260f, true);
                for (int i = 0; i < units.Count; i++)
                {
                if (units[i].Team != owner.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {					     
                         units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
						 AddBuff("Stun", 1f, 1, spell, units[i], owner);
						 AddParticleTarget(owner, units[i], "PowerBallHit", units[i], 10f,1,"");
                    }	
                }
        } 
       
        
        public void OnUpdate(float diff)
        {
          if (owner != null && ibuff != null && spell != null)
            {
                DamageManaTimer += diff;

                if (DamageManaTimer >= 10f)
                {				
					M = T * 1.2f;
                    var units = GetUnitsInRange(GetPointFromUnit(owner, 125f), 75f, true);
                    for (int i = 0; i < units.Count; i++)
                    {
                    if (units[i].Team != owner.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                       {					     
                         ibuff.DeactivateBuff(); 
                       }	
                    }					
                    DamageManaTimer = 0;
                }
            }				
        }
    }
}