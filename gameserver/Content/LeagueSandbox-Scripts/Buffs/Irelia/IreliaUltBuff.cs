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
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using System.Linq;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Buffs
{
    class IreliaTranscendentBlades : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
			MaxStacks = 5
        };
        public IStatsModifier StatsModifier { get; private set; }
        IParticle p;
        IParticle p2;
		IParticle p3;
		IParticle p4;
		IParticle p5;
        IParticle p6;		
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			var owner = ownerSpell.CastInfo.Owner;
			var ownerSkinID = owner.SkinID;
            var trueCoords = new Vector2(ownerSpell.CastInfo.TargetPosition.X, ownerSpell.CastInfo.TargetPosition.Z);
			switch (buff.StackCount)
            {
                case 1:
				    PlayAnimation(unit, "Spell4");
				    p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, ".troy", unit, buff.Duration,1,"BUFFBONE_Cstm_Sword1_loc");
					p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, ".troy", unit, buff.Duration,1,"BUFFBONE_Cstm_Sword1_loc");
					p3 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, ".troy", unit, buff.Duration,1,"BUFFBONE_Cstm_Sword1_loc");
					p4 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, ".troy", unit, buff.Duration,1,"BUFFBONE_Cstm_Sword1_loc");
					p5 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, ".troy", unit, buff.Duration,1,"BUFFBONE_Cstm_Sword1_loc");
                    p6 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "irelia_ult_magic_resist.troy", unit, buff.Duration);					
                    break;
                case 2:
				FaceDirection(trueCoords, owner);
				    RemoveParticle(p);
                    SpellCast(owner, 0, SpellSlotType.ExtraSlots, trueCoords, trueCoords, false, Vector2.Zero);							
                    break;
                case 3:
				FaceDirection(trueCoords, owner);
				    RemoveParticle(p);
				    SpellCast(owner, 0, SpellSlotType.ExtraSlots, trueCoords, trueCoords, false, Vector2.Zero);
                    break;
				case 4:
				FaceDirection(trueCoords, owner);
				    RemoveParticle(p);
                    SpellCast(owner, 0, SpellSlotType.ExtraSlots, trueCoords, trueCoords, false, Vector2.Zero);							
                    break;
                case 5:
				FaceDirection(trueCoords, owner);
				    RemoveParticle(p);
				    SpellCast(owner, 0, SpellSlotType.ExtraSlots, trueCoords, trueCoords, false, Vector2.Zero);
				    buff.DeactivateBuff();
                    break;
			}
            
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
           RemoveParticle(p);
		   RemoveParticle(p2);
		   RemoveParticle(p3);
		   RemoveParticle(p4);
		   RemoveParticle(p5);
		   RemoveParticle(p6);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}