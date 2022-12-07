using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain.GameObjects.Spell.Missile;

namespace Spells
{
    public class LuluR : ISpellScript
    {
		IObjAiBase Owner;
		public ISpellSector AOE;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
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
			Owner = owner;
            AddBuff("LuluR", 7.0f, 1, spell, target, owner);
			if (spell.CastInfo.Owner is IChampion c)
            {
                var damage = 300 + (100 * (spell.CastInfo.SpellLevel - 1)) + (c.Stats.AbilityPower.Total * 0.6f);

                var units = GetUnitsInRange(c.Position, 450f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
                        units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                        AddParticleTarget(c, units[i], "Lulu_R_knock_up_impact.troy", units[i]);
                    }
                }
             
            }
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
