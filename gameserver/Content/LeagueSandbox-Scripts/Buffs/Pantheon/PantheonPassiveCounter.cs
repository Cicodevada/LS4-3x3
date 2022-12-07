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
    internal class PantheonPassiveCounter : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; }

        ISpell Spell;
		int counter;
		IObjAiBase Owner;
        IAttackableUnit Target;
		IBuff thisBuff;
        bool didcast = false;
        float findamage;

        private readonly IAttackableUnit target;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			thisBuff = buff;
            var owner = ownerSpell.CastInfo.Owner;
            Owner = owner;
            Target = unit;
            Spell = ownerSpell;
			ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            //ApiEventManager.OnTakeDamage.AddListener(this, unit, TakeDamage, false);
        }
        public void TakeDamage(IDamageData damageData)
        {
        }
		public void OnLaunchAttack(ISpell spell)      
        {
            var owner = Spell.CastInfo.Owner;
			if (!Owner.HasBuff("PantheonPassiveShield"))
            {
				counter++;      
                if (counter == 2)
                {             
                }
				if (counter == 3)
                {              
                }
				if (counter == 4)
                {
                Owner.RemoveBuffsWithName("PantheonPassiveCounter");
				thisBuff.DeactivateBuff();
                counter = 0;			
                }
            }
        }   

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveBuff(thisBuff);
			AddBuff("PantheonPassiveShield", 25000f, 1, ownerSpell, Owner, Owner);
        }
        public void OnUpdate(float diff)
        {         
        }
    }
}