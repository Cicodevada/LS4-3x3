using System.Numerics;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
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
    class AatroxWONHLifeBuff : IBuffGameScript
    {
		public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IParticle pbuff;
        IParticle pbuff2;
        IBuff thisBuff;
		IObjAiBase owner;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisBuff = buff;
			if (unit is IObjAiBase ai)
            {
            var owner = ownerSpell.CastInfo.Owner as IChampion;
			pbuff = AddParticleTarget(unit, unit, "Aatrox_Base_W_WeaponLife.troy", unit, 25000f, 1, "WEAPON");
				    //AddParticleTarget(unit, unit, "Aatrox_Base_W_WeaponLifeR.troy", unit, buff.Duration, 1, "WEAPON");
			pbuff2 = AddParticleTarget(unit, unit, "Aatrox_Base_W_Buff_Life_sound.troy", unit, 25000f, 1, "WEAPON");
				    //p2 = AddParticleTarget(unit, unit, "Aatrox_Base_W_weaponlife_glow.troy", unit, 25000f, 1, "WEAPON");                 
            if (unit.HasBuff("AatroxR"))
               {
				    owner.SetAutoAttackSpell("AatroxBasicAttack6", false);
			   }
			else
			   {
			        owner.SetAutoAttackSpell("AatroxBasicAttack3", false);
			   }		
			ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
			}
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			var owner = ownerSpell.CastInfo.Owner as IChampion;
			RemoveParticle(pbuff);
            RemoveParticle(pbuff2);
			RemoveBuff(thisBuff);
			if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
        }

        public void OnLaunchAttack(ISpell spell)
        {
			
			if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {                       
            spell.CastInfo.Owner.RemoveBuff(thisBuff);
            var owner = spell.CastInfo.Owner as IChampion;
			//spell.CastInfo.Owner.SetAutoAttackSpell("AatroxBasicAttack3", false);
            //SpellCast(spell.CastInfo.Owner, 3, SpellSlotType.ExtraSlots, false, spell.CastInfo.Owner.TargetUnit, Vector2.Zero);
            thisBuff.DeactivateBuff();
			}
        }
        public void OnUpdate(float diff)
        {
        }
    }
}