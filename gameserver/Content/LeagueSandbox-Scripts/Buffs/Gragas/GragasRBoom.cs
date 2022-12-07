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
    class GragasRBoom : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IBuff ThisBuff;
        IMinion Boom;
        IParticle p;
		IParticle p2;
		IParticle p3;
        int previousIndicatorState;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ThisBuff = buff;
            Boom = unit as IMinion;
            string particles;	
			var units = GetUnitsInRange(Boom.Position, 260f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != Boom.Owner.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
						var AP = Boom.Owner.Stats.AbilityPower.Total * 0.7f;
						var RLevel = Boom.Owner.GetSpell(3).CastInfo.SpellLevel;
						var damage = 200 + (100 * (RLevel - 1)) + AP;
                        units[i].TakeDamage(Boom.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
					    AddParticleTarget(Boom.Owner, units[i], "Gragas_Base_R_Tar.troy", units[i], 1f);
				        AddParticleTarget(Boom.Owner, units[i], ".troy", units[i], 1f);
                    }
                }        
            p = AddParticle(Boom.Owner, null, "Gragas_Base_R_End", Boom.Position, buff.Duration);
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
                Boom.TakeDamage(Boom, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
        }
        public void OnUpdate(float diff)
        {          
        }
    }
}