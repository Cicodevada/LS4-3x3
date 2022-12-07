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
    class EkkoW : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
		float T;
        IBuff WB;
		ISpell S;
        IObjAiBase Owner;
		IMinion W; 
		IParticle P;
        public ISpellSector AOE;
		IAttackableUnit U;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			U = unit;
			WB = buff;
			S = ownerSpell;
            Owner = ownerSpell.CastInfo.Owner;
			P = AddParticle(Owner, null, "Ekko_Base_W_Detonate_Slow.troy", unit.Position,10f);
            var units = GetUnitsInRange(P.Position, 350f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != Owner.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
						AddBuff("EkkoSlow", 2f, 1, S, units[i], Owner, false);
                    }
                }			
        }    
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			unit.TakeDamage(unit, 100000, DamageType.DAMAGE_TYPE_TRUE,DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }
		public void Boom(ISpell spell)
        {
			WB.DeactivateBuff();
			if (spell.CastInfo.Owner is IChampion c)
            { 
		        AddParticle(c, null, "", P.Position,10f);	
			    AddParticle(c, null, "Ekko_Base_W_Detonate.troy", P.Position,10f);		
                var units = GetUnitsInRange(P.Position, 350f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
						AddBuff("EkkoWStun", 2.5f, 1, S, units[i], c, false);
                        AddParticleTarget(c, units[i], "Ekko_Base_W_Crit_Tar", units[i],10);
						//AddParticleTarget(c, units[i], "Ekko_Base_W_Shield_HitDodge", units[i]);
                    }
                }
		    }
        }
        public void OnUpdate(float diff)
        {
            T += diff;
			if (T >= 1)
			{
				T = 0;
				if (S.CastInfo.Owner is IChampion c)
                {             				
                    var units = GetUnitsInRange(P.Position, 350f, true);
                    for (int i = 0; i < units.Count; i++)
                    {
                        if (units[i] == c)
                        {
							Boom(S);
							AddBuff("EkkoWShield", 2f, 1, S, c, c, false);			
                        }		
                    }
		        }
			}
        }
    }
}