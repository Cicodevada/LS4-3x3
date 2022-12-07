using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
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
    class JaxCounterStrike : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };
        public IStatsModifier StatsModifier { get; private set; }
        IParticle pbuff;
        IParticle pbuff2;
        IBuff thisBuff;
		IObjAiBase owner;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			pbuff = AddParticleTarget(unit, unit, "JaxDodger.troy", unit, buff.Duration);
			PlayAnimation(unit, "spell3",buff.Duration);
			CreateTimer((float) 1f , () =>
                {
				ownerSpell.CastInfo.Owner.GetSpell("JaxCounterStrike").SetCooldown(0f);
				});
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			StopAnimation(unit, "spell3");
			PlayAnimation(unit, "spell3b",0.3f);
			AddParticleTarget(unit, unit, "Counterstrike_cas.troy", unit, 1f,1);
			var AD = ownerSpell.CastInfo.Owner.Stats.AttackDamage.FlatBonus * 0.5f;
            var damage = 25 + 30 * ownerSpell.CastInfo.SpellLevel + AD;          
            var units = GetUnitsInRange(unit.Position, 400f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == unit.Team || units[i] is IBaseTurret || units[i] is IObjBuilding || units[i] is IInhibitor))
                {
                    units[i].TakeDamage(unit, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                    AddParticleTarget(unit, units[i], "globalhit_orange_tar.troy", units[i], 1f);               
                    AddBuff("Stun", 1f, 1, ownerSpell, units[i], ownerSpell.CastInfo.Owner, false);
                }
            }
            RemoveParticle(pbuff);
            RemoveParticle(pbuff2);			
        }

        public void OnUpdate(float diff)
        {
            //nothing!
        }
    }
}