using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class GarenE : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public IStatsModifier StatsModifier { get; private set; }

        IChampion Owner;
        float damage;
        float TimeSinceLastTick = 500f;
        IParticle p;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner as IChampion;
            var ADratio = owner.Stats.AttackDamage.Total * (0.35f + 0.05f * (ownerSpell.CastInfo.SpellLevel - 1));
            damage = 10f + 12.5f * (ownerSpell.CastInfo.SpellLevel - 1) + ADratio;
            Owner = owner;

            OverrideAnimation(unit, "Spell3", "RUN");
            p = AddParticleTarget(owner, unit, "Garen_Base_E_Spin.troy", unit, buff.Duration);

            SetStatus(unit, StatusFlags.CanAttack, false);
            SetStatus(unit, StatusFlags.Ghosted, true);
            // TODO: allow garen move through units
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            // TODO: disallow garen move through units
        }

        public void OnUpdate(float diff)
        {
            TimeSinceLastTick += diff;
            if (TimeSinceLastTick >= 500.0f)
            {
                PlayAnimation(Owner, "Spell3", 0.0f, 0, 1.0f, AnimationFlags.UniqueOverride);

                var units = GetUnitsInRange(Owner.Position, 330f, true).OrderBy(unit => Vector2.DistanceSquared(unit.Position, unit.Position)).ToList();
                units.RemoveAt(0);
                for (int i = units.Count - 1; i >= 0; i--)
                {
                    if (units[i].Team != Owner.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret) && units[i] is IObjAiBase)
                    {
                        if (units[i] is IMinion)
                        {
                            damage *= 0.75f;
                        }
                        units[i].TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    }
                }
                TimeSinceLastTick = 0;
            }
        }
    }
}
