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
    public class NinanPassiveStack : IBuffGameScript
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
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			owner = ownerSpell.CastInfo.Owner as IChampion;
            Unit = unit;
            switch (buff.StackCount)
            {
                case 1:
                    p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Talon_Base_P_Stack_1.troy", unit, buff.Duration);
                    return;
                case 2:
				    RemoveParticle(p);
                    p1 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Talon_Base_P_Stack_2.troy", unit, buff.Duration);
                    return;
                case 3:
				    RemoveParticle(p1);
					AddBuff("NinanPassiveBleed", 6f, 1, ownerSpell, unit, owner);
					buff.DeactivateBuff();              
                    break;		
            }
        }    
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
			RemoveParticle(p1);
			RemoveParticle(p2);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}