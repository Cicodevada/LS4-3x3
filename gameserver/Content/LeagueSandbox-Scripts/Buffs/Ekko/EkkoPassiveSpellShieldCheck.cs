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
    class EkkoPassiveSlow : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        float damage;
        IAttackableUnit Unit;
        IObjAiBase owner;
        IParticle p;
		IParticle p1;
		IBuff thisBuff;
		bool isVisible = true;
        IParticle p2;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			thisBuff = buff;
            owner = ownerSpell.CastInfo.Owner as IChampion;
            Unit = unit;
            var APratio = owner.Stats.AbilityPower.Total * 0.8f;         	
			damage = 20 + ( 10* owner.Stats.Level ) + APratio;							
            Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_PERIODIC, false);
            AddParticleTarget(owner, unit, "Ekko_Base_P_Proc", unit, 10f, 1f);				
            //p = AddParticleTarget(owner, unit, "Ekko_Base_P_Disappear", unit, 0.1f, 1f);
			//p1 = AddParticleTarget(owner, unit, "Ekko_Base_P_Slow_Avatar", unit, 0.1f, 1f);
            p2 = AddParticleTarget(owner, unit, "Ekko_Base_P_Proc_Cool", unit, buff.Duration, 1f);				
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
			RemoveParticle(p1);
			RemoveBuff(thisBuff);
            RemoveParticle(p2);
        }
        public void OnUpdate(float diff)
        {        
        }
    }
}