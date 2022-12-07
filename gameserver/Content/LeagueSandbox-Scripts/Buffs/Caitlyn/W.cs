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
    class CaitlynTrap : IBuffGameScript
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
		IAttackableUnit Unit;
		IParticle p;
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Unit = unit;
			owner = ownerSpell.CastInfo.Owner;
			ibuff = buff;
			spell = ownerSpell;

            if (unit is IObjAiBase obj)
            { 
		            p = AddParticleTarget(owner, obj, "caitlyn_Base_yordleTrap_idle", obj, 100f,1,"");
		            ApiEventManager.OnCollision.AddListener(this, obj, Publish, false);
            }
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveParticle(p);
			if (unit is IObjAiBase obj)
            { 
		            unit.TakeDamage(unit, 1000000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);
					AddParticleTarget(owner, unit, "caitlyn_Base_yordleTrap_trigger_sound", unit, 10f,1,"");
                    ApiEventManager.OnPreAttack.RemoveListener(this, obj as IObjAiBase);
                    ApiEventManager.OnCollision.RemoveListener(this, obj as IObjAiBase);						
               				
            }
        }
		public void Publish(IGameObject owner, IGameObject target)
        {
			if (target.Team != owner.Team && !(target is IObjBuilding || target is IBaseTurret))
			{
			AOE(spell);
			ibuff.DeactivateBuff();
			}
		}
		public void AOE(ISpell spell)
        {
             var owner = spell.CastInfo.Owner;
			 var ap = owner.Stats.AbilityPower.Total * 0.6f;
             var damage = 80 + spell.CastInfo.SpellLevel * 50 + ap;
			 var units = GetUnitsInRange(Unit.Position, 100f, true);
                for (int i = 0; i < units.Count; i++)
                {
                if (units[i].Team != owner.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {					     
                         units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
						 AddBuff("Stun", 1.5f, 1, spell, units[i], owner);
						 AddParticleTarget(owner, units[i], "caitlyn_Base_yordleTrap_impact_debuf", units[i], 1.5f,1,"");
						 AddParticleTarget(owner, units[i], "caitlyn_Base_yordleTrap_trigger", units[i], 10f,1,"");
						 AddParticleTarget(owner, units[i], "caitlyn_Base_yordleTrap_trigger_02", units[i], 10f,1,"");					
                    }	
                }
        } 
       
        
        public void OnUpdate(float diff)
        {        			
        }
    }
}