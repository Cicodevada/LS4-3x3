using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Scripting.CSharp;
using System;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Collections.Generic;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Buffs
{
    internal class MissFortuneBulletTime : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.HEAL
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();   

        IObjAiBase owner;
        float tickTime;
		ISpell S;
		Vector2 targetPos;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			S = ownerSpell;
            owner = ownerSpell.CastInfo.Owner;
            owner.AddStatModifier(StatsModifier);
			for (int bladeCount = 0; bladeCount <= 8; bladeCount++)
             {                            
				 targetPos = GetPointFromUnit(owner, 1200f,  (-24f + (bladeCount * 8f)));
				 //SpellCast(owner, 1, SpellSlotType.ExtraSlots, end, Vector2.Zero, true, start);
                 //SpellCast(owner, 1, SpellSlotType.ExtraSlots, end, end, true, Vector2.Zero);				 
             }
			//targetPos = new Vector2(ownerSpell.CastInfo.TargetPosition.X, ownerSpell.CastInfo.TargetPosition.Z);
        }   

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ApiEventManager.RemoveAllListenersForOwner(this);       
        }

        public void OnUpdate(float diff)
        {
            if (tickTime >= 0.0f)
            {
				for (int bladeCount = 0; bladeCount <= 8; bladeCount++)
                {                            
				 targetPos = GetPointFromUnit(owner, 1200f,  (-24f + (bladeCount * 8f)));
				 SpellCast(owner, 2, SpellSlotType.ExtraSlots, targetPos, targetPos, false, Vector2.Zero);
				 //SpellCast(owner, 1, SpellSlotType.ExtraSlots, end, Vector2.Zero, true, start);
                 //SpellCast(owner, 1, SpellSlotType.ExtraSlots, end, end, true, Vector2.Zero);				 
                }          
                tickTime = -250;
            }
            tickTime += diff;
        }
    }
}
