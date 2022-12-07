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

namespace Spells
{
    public class PantheonW : ISpellScript
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
        }

        public void OnSpellCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
			var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
			var distt = dist + 1;
			var time = dist / 1700f;
			var targetPos = GetPointFromUnit(owner,distt);
			PlayAnimation(owner, "Spell2",time);
            AddParticle(owner, null, ".troy", owner.Position, lifetime: 10f);
            ForceMovement(owner, null, Target.Position, 1700, 0, 120, 0);
			var APratio = owner.Stats.AbilityPower.Total * 1f;
            var damage = 50f + ((spell.CastInfo.SpellLevel - 1) * 25) + APratio;
            if (!owner.HasBuff("PantheonPassiveShield"))
            {
                owner.RemoveBuffsWithName("PantheonPassiveCounter");
            }			
			CreateTimer((float) time , () =>
            {  
			Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddBuff("Stun", 1f, 1, spell, Target, owner);
			//AddParticleTarget(owner, owner, "Pantheon_Base_W_buf.troy", Target, 4f);
            AddParticleTarget(owner, Target, "Pantheon_Base_W_tar.troy", Target, 10f);
			});	
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
