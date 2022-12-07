using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using GameServerCore;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Scripting.CSharp;
using System;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;


namespace Buffs
{
    internal class DontM : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        private IBuff ThisBuff;
		IAttackableUnit Unit;
		ISpell Spell;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Spell = ownerSpell;
			Unit = unit;
			ThisBuff = buff;
			var owner = ownerSpell.CastInfo.Owner as IChampion;
			owner.StopMovement(); 
            owner.SetTargetUnit(null, true);			
			unit.Stats.SetActionState(ActionState.CAN_MOVE, false);
	
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			unit.Stats.SetActionState(ActionState.CAN_MOVE, true);
        }
        public void OnUpdate(float diff)
        {
        }
    }
}