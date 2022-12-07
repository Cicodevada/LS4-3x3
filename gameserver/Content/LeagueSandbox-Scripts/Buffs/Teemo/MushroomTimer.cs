using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;
namespace Buffs
{
    internal class BantamTrap : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IAttackableUnit Unit;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            Unit = unit;
            //var spell = ownerSpell;
            //var owner = spell.CastInfo.Owner;
            //ApiEventManager.OnSpellSectorHit.AddListener(this, new KeyValuePair<ISpell, IObjAiBase>(spell, owner), TargetExecute, false);
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellSector sector)
        {
            //AddBuff("BantamTrap", 4f, 1, spell, target, spell.CastInfo.Owner);
            //AddParticleTarget(spell.CastInfo.Owner, Unit, "ShroomMine.troy", Unit, 2f);
            //Unit.Die(null);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            unit.TakeDamage(ownerSpell.CastInfo.Owner, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}

