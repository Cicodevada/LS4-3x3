using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System.Linq;

namespace Spells
{
    public class JaxLeapStrike : ISpellScript
    {
        IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
			owner.CancelAutoAttack(false);
        }

        public void OnSpellCast(ISpell spell)
        {       
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;     
            var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
			var distt = dist - 125;
			var targetPos = GetPointFromUnit(owner,distt);
            var time = distt / 2200f;
			AddBuff("Ghosted", time, 1, spell, owner, owner);
			var APratio = owner.Stats.AbilityPower.Total*0.6f;
            var ADratio = owner.Stats.AttackDamage.FlatBonus;
            var damage = 70* spell.CastInfo.SpellLevel +ADratio + APratio;
			ForceMovement(owner, null, targetPos, 1700, 0, 120, 0);
			PlayAnimation(owner, "spell2",0.3f);
			CreateTimer((float) time , () =>
            {        
			    if (!(Target.Team == owner.Team || Target is IBaseTurret || Target is IObjBuilding || Target is IInhibitor))
                {
			    Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
			    AddParticleTarget(owner, Target, "jax_leapstrike_tar.troy", Target, 10f);
			    }
			});			
        }
		public void Jump(ISpell spell)
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