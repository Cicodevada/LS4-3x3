using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using GameServerLib.GameObjects.AttackableUnits;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Enums;

namespace Spells
{
    public class FizzPiercingStrike : ISpellScript
    {
		IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
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
            owner.SetTargetUnit(null, true);			
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
            PlayAnimation(owner, "SPELL1", 0.28f);			
			var ad = owner.Stats.AbilityPower.Total * 0.35f;
			var Wdamage = 30 + 10 * owner.GetSpell(1).CastInfo.SpellLevel + ad;
            var damage = 10f + owner.GetSpell(0).CastInfo.SpellLevel * 10f + owner.Stats.AttackDamage.Total;
			var QWdamage = Wdamage + damage;
            AddParticleTarget(owner, owner, "Fizz_PiercingStrike.troy", owner, 0.5f);
            AddParticleTarget(owner, Target, "Fizz_PiercingStrike_tar.troy", Target);
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var to = Vector2.Normalize(new Vector2(Target.Position.X, Target.Position.Y) - current);
            var range = to * 550;
            var trueCoords = current + range;
            ForceMovement(owner, null, trueCoords, 1400, 0, 0, 0);
			if (owner.HasBuff("FizzSeastonePassive"))
			{
			    Target.TakeDamage(owner, QWdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
				owner.RemoveBuffsWithName("FizzSeastonePassive");
			}
			else 
			{
				Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
			}         
			if (owner.HasBuff("FizzSeastoneTrident"))
			{
			AddBuff("FizzSeastoneTridentActive", 3f, 1, spell, Target, owner);
			}
			else 
			{
			}	 	
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
