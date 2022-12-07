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
    public class FioraQ : ISpellScript
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
			if (!owner.HasBuff("FioraQCD"))
			{
            AddBuff("FioraQCD", 4, 1, spell, owner, owner);	
            }			
            SpellCast(owner, 0, SpellSlotType.ExtraSlots, false, Target, Vector2.Zero);	
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
	public class FioraQLunge : ISpellScript
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
            var damage = 40 + 25* (owner.GetSpell(0).CastInfo.SpellLevel-1) + ad;
            var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
			var distt = dist - 125;
			var targetPos = GetPointFromUnit(owner,distt);
            var time = dist / 2200f;
			PlayAnimation(owner, "Spell1",time);
			AddBuff("Ghosted", time, 1, spell, owner, owner);
			CreateTimer((float) time , () =>
            {                           
            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
			AddParticleTarget(owner, Target, "FioraQLunge_tar.troy", Target, 10f);
            });
			FaceDirection(targetPos, owner, true);
            ForceMovement(owner, null, targetPos, 2200, 0, 0, 0);
			AddParticleTarget(owner, owner, "FioraQLunge_dashtrail.troy", owner, time);
			AddParticleTarget(owner, owner, "Fiora_Dance_windup.troy", owner, time); 			
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