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
    class AatroxPassive : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IMinion Leblanc;
        ISpell Spell;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Spell = ownerSpell;
			if (ownerSpell.CastInfo.Owner is IChampion owner)
            {
			ApiEventManager.OnTakeDamage.AddListener(this, owner, OnTakeDamage, false);
			}
            
        }
		public void OnTakeDamage(IDamageData damageData)       
        {
            if (Spell.CastInfo.Owner is IChampion owner)
            {
            var currentHealth = owner.Stats.CurrentHealth;
			var limitHealth = owner.Stats.HealthPoints.Total * 0.2;
			if (limitHealth >= currentHealth)
			{
				if (owner.HasBuff("AatroxPassive"))
                {
				owner.RemoveBuffsWithName("AatroxPassive");
                }
			}
			}
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
		   if (Spell.CastInfo.Owner is IChampion owner)
           {
           AddBuff("AatroxPassiveDeath", 3f, 1, ownerSpell, owner, owner,false); 
		   }
        }
        public void OnUpdate(float diff)
        {
        }
    }
}