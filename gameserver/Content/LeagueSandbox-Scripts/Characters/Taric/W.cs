using System.Linq;
using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class Shatter : ISpellScript
    {
        ISpell Spell;
        IObjAiBase Owner;
        float timeSinceLastTick = 500f;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            //ADD skill levelUp listener
            Spell = spell;
            Owner = owner;
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {

        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner as IChampion;
            var armor = spell.CastInfo.Owner.Stats.Armor.Total;
            var damage = spell.CastInfo.SpellLevel * 40 + armor * 0.2f;
            var buffOwnerDuration = 10f * (1 - owner.Stats.CooldownReduction.Total); //Setting buff duration to spell.CastInfo.Cooldown wasn't working

            AddParticleTarget(owner, owner, "Shatter_nova.troy", owner, 1f);

            var units = GetUnitsInRange(spell.CastInfo.Owner.Position, 375, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].Team != owner.Team && !(units[i] is IBaseTurret || units[i] is IObjBuilding || units[i] is IInhibitor))
                {
                    units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                    AddBuff("Shatter", buffOwnerDuration, 1, spell, units[i], owner);
                    AddParticleTarget(owner, units[i], "globalhit_bloodslash.troy", units[i], 1, 1f); //Not sure about this PFX
                }
            }
            AddBuff("Shatter", buffOwnerDuration, 1, spell, owner, owner);
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
            
            timeSinceLastTick += diff;

            if (timeSinceLastTick >= 500f)
            {
                if (Spell.CurrentCooldown <= 0 && !Owner.HasBuff("Shatter") && Spell.CastInfo.SpellLevel >= 1)
                {
                    AddBuff("ShatterSelfBonus", 2f, 1, Spell, Owner, Owner, true);
                }
                if (Spell.CastInfo.SpellLevel >= 1 && !Owner.IsDead)
                {
                    AddBuff("ShatterAuraSelf", 2f, 1, Spell, Owner, Owner, false);
                }
                timeSinceLastTick = 0f;
            }
            if (Owner.HasBuff("Shatter"))
            {
                RemoveBuff(Owner, "ShatterSelfBonus");
            }
        }
    }
}

