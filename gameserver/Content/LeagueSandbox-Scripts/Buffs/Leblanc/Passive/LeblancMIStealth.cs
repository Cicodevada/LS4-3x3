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
    internal class LeblancMIFull : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IMinion Leblanc;
        ISpell Spell;
		private IBuff buff;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Spell = ownerSpell;
			if (ownerSpell.CastInfo.Owner is IChampion owner)
            {
		       AddParticleTarget(owner, owner, "LeBlanc_Base_P_poof", owner,10f);
			   var Leblanc = CreatePet(owner, Spell, owner.Position, "Leblanc", "Leblanc", "LeblancMIApplicator", 8.0f, owner.SkinID, showMinimapIfClone: true, isClone: true);
               Leblanc.SetTargetUnit(owner, true);
               Leblanc.UpdateMoveOrder(OrderType.AttackTo, true);				   		 	
			}
            
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {		  
        }
        public void OnUpdate(float diff)
        {	
        }
    }
}