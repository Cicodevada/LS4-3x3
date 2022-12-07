using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using GameServerLib.GameObjects.AttackableUnits;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class XenZhaoComboAutoFinish : IBuffGameScript
    {
		public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IAttackableUnit Unit;
        IObjAiBase owner;
        IParticle p;
		IBuff thisBuff;
        IParticle p2;
		ISpell spell;
		int counter = 1;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			if (unit is IObjAiBase obj)
            {
				p = AddParticleTarget(obj, obj, "xenZiou_ChainAttack_cas_01.troy", obj, buff.Duration, 1, "Buffbone_Glb_WEAPON_1");
				p2 = AddParticleTarget(obj, obj, "xenZiou_ChainAttack_indicator.troy", obj, buff.Duration, 1, "Buffbone_Glb_WEAPON_1");
                ApiEventManager.OnHitUnit.AddListener(this, obj, OnHitUnit, false);
                obj.CancelAutoAttack(true);
                owner = obj;
            }
            spell = ownerSpell;
            thisBuff = buff;
        }
		public void OnHitUnit(IDamageData damageData)
        {
			Unit = damageData.Target;
			var ad = owner.Stats.AttackDamage.Total * 0.2f;
            float damage = 15 * owner.GetSpell(0).CastInfo.SpellLevel + ad;
			if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {
                switch (counter)
               {
                case 1:
					Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
					AddParticleTarget(spell.CastInfo.Owner, Unit, "XenZiou_Wind_ChainAttack01.troy", Unit);		
                    counter++;
                    break;
                case 2:
					Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
					AddParticleTarget(spell.CastInfo.Owner, Unit, "XenZiou_Wind_ChainAttack02.troy", Unit);
                    counter++;

                    break;
                case 3:               
				    Unit.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
					AddParticleTarget(spell.CastInfo.Owner, Unit, "XenZiou_Wind_ChainAttack03.troy", Unit);
                    ForceMovement(Unit, "RUN", new Vector2(Unit.Position.X + 5f, Unit.Position.Y + 5f), 13f, 0, 16.5f, 0);
                    thisBuff.DeactivateBuff();
                    break;
               }				
			}
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveParticle(p);
		    RemoveBuff(thisBuff);
            RemoveParticle(p2);
			if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnHitUnit.RemoveListener(this);
            }        
        }
        public void OnUpdate(float diff)
        {        
        }
    }
}