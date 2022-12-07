using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using System.Numerics;
using GameServerCore;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain;

namespace Buffs
{
    internal class EkkoR : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IMinion Ekko;
        ISpell Spell;
		IObjAiBase Owner;
		private IBuff buff;
		IAttackableUnit Unit;
		float timeSinceLastTick = 1000f;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Unit = unit;
			Owner = ownerSpell.CastInfo.Owner;
			Spell = ownerSpell;
			if (ownerSpell.CastInfo.Owner is IChampion owner)
            {
                SetStatus(owner, StatusFlags.Ghosted, true);
				SetStatus(owner, StatusFlags.Targetable, false);
			}
            
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
		   if (Spell.CastInfo.Owner is IChampion c)
           {	 
                SetStatus(c, StatusFlags.Ghosted, false);
				SetStatus(c, StatusFlags.Targetable, true);	   
			    var damage = (150 * Spell.CastInfo.SpellLevel) + (c.Stats.AbilityPower.Total * 1.5f );
                AddParticle(c, null, "Ekko_Base_R_Tar", c.Position);
                var units = GetUnitsInRange(c.Position, 550f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
                        units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
						AddBuff("EkkoPassive", 6f, 1, Spell, units[i], c);
                        //AddParticleTarget(c, units[i], "Ekko_Base_R_Tar_Impact", units[i]);
                    }
                }
		   }
        }
        public void OnUpdate(float diff)
        {
            		
        }
    }
}