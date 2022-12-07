using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;



namespace Buffs
{
    class DefensiveBallCurl : IBuffGameScript
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
		IParticle p;
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			ibuff = buff;
			spell = ownerSpell;
			if (unit.Model == "Rammus")
            {
                unit.ChangeModel("RammusDBC");
            }

            if (unit is IObjAiBase obj)
            { 
		            StatsModifier.MoveSpeed.PercentBonus -= 0.15f;
                    obj.AddStatModifier(StatsModifier);
					p = AddParticleTarget(obj, obj, "defensiveballcurl_buf", obj, 10f,1,"");	
                    ApiEventManager.OnTakeDamage.AddListener(this, obj, TakeDamage, false);
            }
        }
        public void TakeDamage(IDamageData damageData)
        {
			    if (damageData.Target.HasBuff("DefensiveBallCurl") && !(damageData.Attacker is IObjBuilding || damageData.Attacker is IBaseTurret) )
				{
				var damage = 50f;
			    damageData.Attacker.TakeDamage(damageData.Target, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                AddParticleTarget(damageData.Target, damageData.Attacker, "thornmail_tar", damageData.Attacker, 10f,1,"");
                }				
        }    
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveParticle(p);
			if (unit.Model == "RammusDBC")
            {
               unit.ChangeModel("Rammus");
            }
        }
        public void OnUpdate(float diff)
        {        
        }
    }
}