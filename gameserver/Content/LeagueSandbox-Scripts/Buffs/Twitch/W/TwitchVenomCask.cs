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
    class TwitchVenomCask : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
		float T;
        float AP;
		float damage;
        IObjAiBase Owner;
		IMinion E; 
		ISpell S;
		IParticle P;
		IParticle P2;
		IParticle red;
		IParticle green;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			S = ownerSpell;
            Owner = ownerSpell.CastInfo.Owner;
			AP = Owner.Stats.AbilityPower.Total * 0.6f;
            damage = (35f + (55f * Owner.GetSpell(2).CastInfo.SpellLevel) + AP)/12;
			P = AddParticle(Owner, null, "Twitch_Base_W_cas.troy", unit.Position,buff.Duration,1);
            P2 = AddParticle(Owner, null, "Twitch_Base_W_Tar.troy", unit.Position,buff.Duration,1);
            if (Owner.Team == TeamId.TEAM_BLUE)
            {
				red = AddParticle(Owner, null, "twitch_w_indicator_red_team", unit.Position, lifetime: buff.Duration, teamOnly: TeamId.TEAM_PURPLE);
                green = AddParticle(Owner, null, "twitch_w_indicator_green_team", unit.Position, lifetime: buff.Duration, teamOnly: TeamId.TEAM_BLUE);
            }
            else
            {
                red = AddParticle(Owner, null, "twitch_w_indicator_red_team", unit.Position, lifetime: buff.Duration, teamOnly: TeamId.TEAM_BLUE);
                green = AddParticle(Owner, null, "twitch_w_indicator_green_team", unit.Position, lifetime: buff.Duration, teamOnly: TeamId.TEAM_PURPLE);
            }     			
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveParticle(P);
			RemoveParticle(P2);
			RemoveParticle(red);
			RemoveParticle(green);
			unit.TakeDamage(unit, 100000, DamageType.DAMAGE_TYPE_TRUE,DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
        }
        public void OnUpdate(float diff)
        {
            T += diff;
			if (T >= 0)
			{
				T = -250;
				if (S.CastInfo.Owner is IChampion c)
                {             				
                    var units = GetUnitsInRange(P.Position, 250f, true);
                    for (int i = 0; i < units.Count; i++)
                    {
                        if (units[i].Team != c.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                        {
						AddBuff("", 2.5f, 1, S, units[i], c, false);
                        //AddParticleTarget(c, units[i], "MissFortune_Base_E_Unit_Tar", units[i],10);
						//units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL,DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
						//AddParticleTarget(c, units[i], "Ekko_Base_W_Shield_HitDodge", units[i]);
                        }	
                    }
		        }
			}
        }
    }
}