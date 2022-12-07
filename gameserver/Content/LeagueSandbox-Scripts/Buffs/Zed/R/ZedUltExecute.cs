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
    internal class ZedUltExecute : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; }

        IObjAiBase Owner;
        IAttackableUnit Target;
        ISpell rspell;
        bool didcast = false;
        float findamage;
		float t;
		IParticle a;

        private readonly IAttackableUnit target;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            Owner = owner;
            Target = unit;
            rspell = ownerSpell;
			AddParticleTarget(owner, unit, "Zed_Base_R_tar_Impact.troy", unit, 10f);
            AddParticleTarget(owner, unit, "Zed_Base_R_tar_DelayedDamage.troy", unit, lifetime: 3f);
            AddParticleTarget(owner, unit, "Zed_Ult_DashEnd.troy", unit);
            ApiEventManager.OnTakeDamage.AddListener(this, unit, TakeDamage, false);
			ApiEventManager.OnDeath.AddListener(this, unit, OnDeath, true);
        }

        public void OnDeath(IDeathData data)
        {           
            
        }
        public void TakeDamage(IDamageData damageData)
        {
            findamage += damageData.Damage;
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveParticle(a);
            var owner = ownerSpell.CastInfo.Owner;
            float damage = Owner.Stats.AttackDamage.Total;
            float percdamage = 5f + rspell.CastInfo.SpellLevel * 15f;
            float finaldamage = (findamage / 100f * percdamage);
            findamage = damage + finaldamage;
			if (Target.IsDead)
            {
			AddParticleTarget(owner, unit, "Zed_Base_R_Tar_pop_Kill.troy", unit);
			}
			else
			{
			AddParticleTarget(owner, unit, "Zed_Base_R_Tar_pop_noKill.troy", unit);
			}         
            unit.TakeDamage(Owner, findamage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
        }

        public void OnUpdate(float diff)
        { 
           t += diff;
		   if (t > 0)
		   {
			   if (findamage >= Target.Stats.CurrentHealth)
			   {
				   if (a != null)
				   {		 
				   }
				   else
				   {
					   a = AddParticleTarget(Owner, Target, "Zed_Base_R_buf_tell.troy", Target,10);
				   }
			   }
			   t = 0;
		   }
        }
    }
}
