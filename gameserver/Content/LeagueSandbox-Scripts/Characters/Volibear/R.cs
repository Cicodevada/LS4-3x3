using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class VolibearR : ISpellScript
    {
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
			if (owner.HasBuff("VolibearQ"))
            {
				OverrideAnimation(owner, "spell1_spell4", "spell4");
			}
			else
			{
				OverrideAnimation(owner, "spell4", "spell1_spell4");
			}
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            //AddParticleTarget(owner, owner, pcastname, owner);
        }

        public void OnSpellPostCast(ISpell spell)
        {
            if (spell.CastInfo.Owner is IChampion c)
            {
				AddBuff("VolibearR", 12.0f, 1, spell, c, c);
				AddParticleTarget(c, c, "Volibear_R_cas", c);
				AddParticleTarget(c, c, "Volibear_R_cas_02", c);
				AddParticleTarget(c, c, "Volibear_R_cas_03", c);
				AddParticleTarget(c, c, "Volibear_R_cas_04", c);
				AddParticleTarget(c, c, "volibear_R_lightning_arms", c);
                var damage = 75 + (40 * (spell.CastInfo.SpellLevel - 1)) + (c.Stats.AbilityPower.Total * 0.3f);

                var units = GetUnitsInRange(c.Position, 450f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
                        units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false); 
                        AddParticleTarget(c, units[i], "Volibear_R_tar", units[i]);
                        AddParticleTarget(c, units[i], "Volibear_R_tar_02", units[i]);								
                    }
                }

                //AddBuff("AatroxR", 12f, 1, spell, spell.CastInfo.Owner, spell.CastInfo.Owner);
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
