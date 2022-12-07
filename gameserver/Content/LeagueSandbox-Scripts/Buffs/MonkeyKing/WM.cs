using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using System.Collections.Generic;
using GameServerCore.Domain.GameObjects.Spell.Missile;

namespace Buffs
{
    class MonkeyKingDecoyClone : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IObjAiBase Owner;
		IParticle p;
		IParticle p2;
		IBuff thisBuff;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			thisBuff = buff;
			Owner = ownerSpell.CastInfo.Owner;
			OverrideAnimation(unit, "spell2", "death");
            AddParticleTarget(Owner, unit, "MonkeyKing_Base_W_Copy.troy", unit,buff.Duration,1); 		

        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveBuff(thisBuff);
			unit.TakeDamage(unit, 1000000, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);
			SetStatus(unit, StatusFlags.NoRender, true);	
			if (ownerSpell.CastInfo.Owner is IChampion c)
            {
				AddParticle(unit, null, "MonkeyKing_Base_W_Cas_Team_ID_Green.troy", unit.Position); 
			    AddParticle(unit, null, "MonkeyKing_Base_W_Death_Team_ID_Green.troy", unit.Position);
				AddParticleTarget(unit, unit, "Become_Transparent.troy", unit);
				AddParticleTarget(c, c, ".troy", c, 10f);
                var damage = 65 + (35 * (ownerSpell.CastInfo.SpellLevel - 1)) + (c.Stats.AttackDamage.Total * 0.2f);
                var units = GetUnitsInRange(c.Position, 350f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
                            units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
						    AddParticleTarget(c, units[i], "MonkeyKing_Base_W_Tar_Decoy.troy", units[i], 1f);
				            AddParticleTarget(c, units[i], ".troy", units[i], 1f);
                    }
                }             
            }
        }
        public void OnUpdate(float diff)
        {

        }
    }
}