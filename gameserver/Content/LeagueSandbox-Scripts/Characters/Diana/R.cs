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
    public class DianaTeleport : ISpellScript
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
            var ad = owner.Stats.AbilityPower.Total * 0.6f;
            var damage = 100 + 60* (spell.CastInfo.SpellLevel-1) + ad;
            var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
			var distt = dist - 125;
			var targetPos = GetPointFromUnit(owner,distt);
            var time = dist / 2200f;
			PlayAnimation(owner, "Spell4",time);
			AddBuff("Ghosted", time, 1, spell, owner, owner);
			CreateTimer((float) time , () =>
            {                           
            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
			AddParticleTarget(owner, Target, "Diana_Base_R_Tar.troy", Target, 10f);
			//AddParticleTarget(owner, owner, "Diana_Base_R_End.troy", owner, time);
			//AddParticleTarget(owner, owner, "Diana_Base_R_Teleport_Success.troy", owner, time);
			if (Target.HasBuff("DianaMoonlight"))
            {
			spell.SetCooldown(0f, true);
			Target.RemoveBuffsWithName("DianaMoonlight");	
			AddParticleTarget(owner, owner, ".troy", owner, time);
			AddParticleTarget(owner, Target, "Diana_Base_R_Teleport_Success", owner, 10f);
			}
			else
			{
				AddParticleTarget(owner, owner, "Diana_Base_R_End.troy", owner, 10f);
			}
            });
			FaceDirection(targetPos, owner, true);
            ForceMovement(owner, null, targetPos, 2200, 0, 0, 0);
			AddParticle(owner, null, ".troy", owner.Position, lifetime: 10f);
			AddParticle(owner, null, ".troy", owner.Position, lifetime: 10f);
			AddParticle(owner, null, ".troy", owner.Position, lifetime: 10f);
			AddParticleTarget(owner, owner, "Diana_Base_R_Cas.troy", owner, time);
			AddParticleTarget(owner, owner, ".troy", owner, time); 			
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