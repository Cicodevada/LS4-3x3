using System.Linq;
using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Scripting.CSharp;   
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Domain;

namespace ItemPassives
{
    public class ItemID_3074 : IItemScript
    {
		private IObjAiBase owner;
        private ISpell spell;
		IAttackableUnit Target;
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IObjAiBase owner)
        {
			ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
        }
        public void OnLaunchAttack(ISpell spell)        
        {
			var owner = spell.CastInfo.Owner;
            Target = spell.CastInfo.Targets[0].Unit;
			AddParticleTarget(owner, Target, "TiamatMelee_itm_hydra.troy", owner);
			var units = GetUnitsInRange(Target.Position, 350f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != owner.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
					
							var damage = owner.Stats.AttackDamage.Total*0.2f;
                            units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);										
                    }
                }    
        }    
        public void OnDeactivate(IObjAiBase owner)
        {
			ApiEventManager.OnLaunchAttack.RemoveListener(this);
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {      
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}