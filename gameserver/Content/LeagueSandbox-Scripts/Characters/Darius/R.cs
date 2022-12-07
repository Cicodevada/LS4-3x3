using LeagueSandbox.GameServer.API;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
namespace Spells
{
    public class DariusExecute : ISpellScript
    {
        IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
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
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
			//var StackDamage = owner.GetBuffWithName("DariusHemo").StackCount;
			//var MAX = StackDamage * 0.2f;
            var ad = owner.Stats.AttackDamage.FlatBonus*1.5f;
            var damage = 160 * spell.CastInfo.SpellLevel +ad;
			//var MAXdamage = damage + damage * MAX;
            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_SPELL, false);
			if (Target.IsDead)
            {
			AddBuff("DariusHemoVisual", 6.0f, 1, spell, owner, owner);
			}
			if (owner.HasBuff("DariusHemoVisual"))
			{
		    AddBuff("DariusHemo", 6.0f, 5, spell, Target, owner);
			}
			else
			{
			AddBuff("DariusHemo", 6.0f, 1, spell, Target, owner);
			}
            AddParticleTarget(owner, Target, "darius_R_tar.troy", Target);
		    AddParticleTarget(owner, Target, "darius_R_tar_02.troy", Target, 10f);
			AddParticleTarget(owner, Target, "darius_R_tar_03.troy", Target, 10f);
			AddParticleTarget(owner, Target, "", Target, 10f);
            AddParticleTarget(owner, Target, "darius_Base_R_tar.troy", Target, 1f, 1f);
			AddParticleTarget(owner, owner, "darius_R_blood_spatter_self.troy", owner, 6f);
            AddParticleTarget(owner, owner, "darius_R_cast_axe.troy", owner);
			AddParticleTarget(owner, owner, "Darius_R_Ready.troy", owner);
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