using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;

namespace Buffs
{
    class FizzChurnTheWatersCling : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IBuff ThisBuff;
        IMinion FizzBait;
        IParticle p;
		IParticle p2;
		IParticle p3;
        int previousIndicatorState;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ThisBuff = buff;
            FizzBait = unit as IMinion;
            var ownerSkinID = FizzBait.Owner.SkinID;
            string particles;			
            p = AddParticleTarget(FizzBait.Owner, FizzBait, "Fizz_UltimateMissile_Orbit", FizzBait, buff.Duration);
            p2 = AddParticleTarget(FizzBait.Owner, FizzBait, "Fizz_Ring_Green.troy", FizzBait, buff.Duration);
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            if (FizzBait != null && !FizzBait.IsDead)
            {
				AddParticle(FizzBait.Owner, null, "Fizz_SharkSplash", FizzBait.Position);
			    AddParticle(FizzBait.Owner, null, "Fizz_SharkSplash_Ground", FizzBait.Position);
                if (p != null)
                {
                    p.SetToRemove();
					p2.SetToRemove();
                }
                SetStatus(FizzBait, StatusFlags.NoRender, true);
                AddParticle(FizzBait.Owner, null, "", FizzBait.Position);
                FizzBait.TakeDamage(FizzBait, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
				var units = GetUnitsInRange(FizzBait.Position, 260f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != FizzBait.Owner.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
						var AP = FizzBait.Owner.Stats.AbilityPower.Total * 0.65f;
						var RLevel = FizzBait.Owner.GetSpell(3).CastInfo.SpellLevel;
						var damage = 200 + (125 * (RLevel - 1)) + AP;
                        units[i].TakeDamage(FizzBait.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
					    AddParticleTarget(FizzBait.Owner, units[i], ".troy", units[i], 1f);
				        AddParticleTarget(FizzBait.Owner, units[i], ".troy", units[i], 1f);
                    }
                }        
            }
        }
        public void OnUpdate(float diff)
        {          
        }
    }
}