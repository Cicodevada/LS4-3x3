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
    public class IreliaGatotsu : ISpellScript
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
            var ad = owner.Stats.AttackDamage.Total * 1.2f;
            var damage = 20 + 30* (spell.CastInfo.SpellLevel-1) + ad;
            var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
			var distt = dist - 125;
			var targetPos = GetPointFromUnit(owner,distt);
            var time = distt / 2200f;
			PlayAnimation(owner, "Spell1",time);
			AddBuff("Ghosted", time, 1, spell, owner, owner);
			CreateTimer((float) time , () =>
            {                           
            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
			AddParticleTarget(owner, Target, "irelia_gotasu_tar.troy", Target, 10f);
			if (Target.IsDead)
            {
			spell.SetCooldown(0f, true);
			AddParticleTarget(owner, owner, "irelia_gotasu_mana_refresh.troy", owner, time);
			AddParticleTarget(owner, owner, "irelia_gotasu_ability_indicator.troy", owner, time);
			}
            });
			FaceDirection(targetPos, owner, true);
            ForceMovement(owner, null, targetPos, 2200, 0, 0, 0);
			AddParticle(owner, null, "irelia_gotasu_cas.troy", owner.Position, lifetime: 10f);
			AddParticle(owner, null, "irelia_gotasu_cast_01.troy", owner.Position, lifetime: 10f);
			AddParticle(owner, null, "irelia_gotasu_cast_02.troy", owner.Position, lifetime: 10f);
			AddParticleTarget(owner, owner, "irelia_gotasu_dash_01.troy", owner, time);
			AddParticleTarget(owner, owner, "irelia_gotasu_dash_02.troy", owner, time); 			
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