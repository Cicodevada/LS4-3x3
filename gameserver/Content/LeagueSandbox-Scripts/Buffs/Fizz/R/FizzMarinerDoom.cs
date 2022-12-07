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
    class FizzMarinerDoom : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IBuff ThisBuff;
        IParticle p;
		IParticle p2;
		IParticle p3;
        int previousIndicatorState;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ThisBuff = buff;
			var owner = ownerSpell.CastInfo.Owner;
            string particles;       
            p = AddParticleTarget(owner, unit, "Fizz_UltimateMissile_Orbit", unit, buff.Duration);
            p2 = AddParticleTarget(owner, unit, "Fizz_Ring_Green.troy", unit, buff.Duration);
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			if (unit != null && !unit.IsDead)
            {
			    var owner = ownerSpell.CastInfo.Owner;
				IMinion FizzShark = AddMinion(owner, "FizzShark", "FizzShark", unit.Position, owner.Team, owner.SkinID, true, false);
				AddBuff("FizzSharkBuff", 0.001f, 1, ownerSpell, FizzShark, FizzShark.Owner, false);
				AddParticleTarget(owner, unit, "Fizz_SharkSplash", unit);
			    AddParticleTarget(owner, unit, "Fizz_SharkSplash_Ground", unit);
                if (p != null)
                {
                    p.SetToRemove();
					p2.SetToRemove();
                }
				var units = GetUnitsInRange(unit.Position, 260f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != owner.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
						var AP = owner.Stats.AbilityPower.Total * 0.65f;
						var RLevel = owner.GetSpell(3).CastInfo.SpellLevel;
						var damage = 200 + (125 * (RLevel - 1)) + AP;
                        units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
					    AddParticleTarget(owner, units[i], ".troy", units[i], 1f);
				        AddParticleTarget(owner, units[i], ".troy", units[i], 1f);
                    }
                } 
            }				
        }
        public void OnUpdate(float diff)
        {          
        }
    }
}