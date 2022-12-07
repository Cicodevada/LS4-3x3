using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Collections.Generic;
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
    public class EkkoPassive : IBuffGameScript
    {
		public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
			MaxStacks = 3
        };

        public IStatsModifier StatsModifier { get; private set; }
        IAttackableUnit Unit;
        IObjAiBase owner;
		IBuff buff;
        IParticle p;
        IParticle p1;
		IParticle p2;
		ISpell Spell;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Spell = ownerSpell;
			owner = ownerSpell.CastInfo.Owner as IChampion;
            Unit = unit;
            switch (buff.StackCount)
            {
                case 1:
				    if (unit is IChampion)
					{
                    p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Ekko_Base_P_Stack1.troy", unit, buff.Duration);
					}
					else
					{
					p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Ekko_Base_P_Stack1_Minion.troy", unit, buff.Duration);
					}
                    break;
                case 2:
				    if (unit is IChampion)
					{
                    p1 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Ekko_Base_P_Stack2.troy", unit, buff.Duration);
                    p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Ekko_Base_P_Stack3_Warning.troy", unit, buff.Duration);
					}
					else
					{
					p1 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Ekko_Base_P_Stack2_Minion.troy", unit, buff.Duration);
                    p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Ekko_Base_P_Stack3_Warning.troy", unit, buff.Duration);
					}
                    AddBuff("EkkoPassiveSpellShieldCheck", 3f, 1, Spell, unit, owner);					
                    break;
                case 3:
				    RemoveBuff(unit, "EkkoPassive");                 
					AddBuff("EkkoPassiveSlow", 3f, 1, Spell, unit, owner);
					if (unit is IChampion)
					{
                    AddBuff("EkkoPassiveSpeed", 3f, 1, Spell, owner, owner); 	
					} 					
                    return;          					
            }
        }    
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			if (buff.TimeElapsed >= buff.Duration)
            {
                RemoveBuff(unit, "EkkoPassive");
            }
            RemoveParticle(p);
			RemoveParticle(p1);
			RemoveParticle(p2);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}