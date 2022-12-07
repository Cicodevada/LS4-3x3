using System.Collections.Generic;
using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using static GameServerCore.Domain.GameObjects.IGameObject;
using GameServerCore;
using LeagueSandbox.GameServer.GameObjects.Stats;

namespace Buffs
{
    class ZiggsE : IBuffGameScript
    {
		float T;
		IParticle P;
		ISpell S;
		IBuff Ebuff;
		IObjAiBase Owner;
		IAttackableUnit U;
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			U = unit;
			Ebuff = buff;
			S = ownerSpell;
			Owner = ownerSpell.CastInfo.Owner;
            P = AddParticle(Owner, null, "ZiggsE_Mis_Small.troy", unit.Position, 10f);
            P = AddParticle(Owner, null, "ZiggsE_mis_mineopen.troy", unit.Position, 10f);				
        }
        public void Boom(ISpell spell)
        {
			Ebuff.DeactivateBuff();
			if (spell.CastInfo.Owner is IChampion c)
            { 
		        AddParticle(c, null, "", P.Position,10f);	
			    AddParticle(c, null, ".troy", P.Position,10f);		
                var units = GetUnitsInRange(P.Position, 20f, true);
				var damage = 15 + (25 * spell.CastInfo.SpellLevel ) + (c.Stats.AbilityPower.Total * 0.3f);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
						AddBuff("", 2.5f, 1, S, units[i], c, false);
                        AddParticleTarget(c, units[i], "ZiggsE_tar", units[i],10);
						AddParticle(c, null, "ZiggsEMine.troy", U.Position, 10f);
						units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
						//AddParticleTarget(c, units[i], "Ekko_Base_W_Shield_HitDodge", units[i]);
                    }
                }
		    }
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveParticle(P);
			unit.Die(CreateDeathData(false, 0, unit, unit, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, 0.0f));
        }

        public void OnUpdate(float diff)
        {
			T += diff;
			if (T >= 1)
			{
				T = 0;
				if (S.CastInfo.Owner is IChampion c)
                {             				
                    var units = GetUnitsInRange(P.Position, 25f, true);
                    for (int i = 0; i < units.Count; i++)
                    {
                        if (units[i].Team != c.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                        {
							Boom(S);	
                        }		
                    }
		        }
			}
        }
    }
}