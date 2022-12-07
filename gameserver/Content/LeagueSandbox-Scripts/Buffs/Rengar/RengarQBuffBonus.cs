using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using System.Collections.Generic;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;


namespace Buffs
{
    internal class RengarQBuffBonusInternal : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IBuff thisBuff;
        IObjAiBase Unit;
        IParticle p;
		ISpell ownerSpell;
        IParticle p2;
		IAttackableUnit target;
		int counter = 1;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisBuff = buff;
            if(unit is IObjAiBase ai)
            {
                Unit = ai;
				ApiEventManager.OnLaunchAttack.AddListener(this, ai, OnLaunchAttack, false);
            }
            StatsModifier.AttackSpeed.PercentBonus += 1f;
            StatsModifier.Range.FlatBonus += 50;
            unit.AddStatModifier(StatsModifier);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
        }
		public void OnLaunchAttack(ISpell spell)
        {
			if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {
                switch (counter)
               {
                case 1:
                    counter++;
                    break;
                case 2:
					thisBuff.DeactivateBuff();                   
                    break;
               }					
			}
        }     
        public void OnUpdate(float diff)
        {

        }
    }
}