using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using System.Linq;
using GameServerCore;


namespace Buffs
{
    class KatarinaR : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.RENEW_EXISTING,
            MaxStacks = 1
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();


        private IObjAiBase Owner;
        float somerandomTick;
        IAttackableUnit Target1;
        IAttackableUnit Target2;
        IAttackableUnit Target3;
        IParticle p;

        ISpell spell;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            IChampion champion = unit as IChampion;
            Owner = ownerSpell.CastInfo.Owner;
            spell = ownerSpell;
			var owner = ownerSpell.CastInfo.Owner;
            p = AddParticleTarget(owner, owner, "Katarina_deathLotus_cas.troy", owner, lifetime: 2.5f, bone: "C_BUFFBONE_GLB_CHEST_LOC");


            var champs = GetChampionsInRange(owner.Position, 500f, true).OrderBy(enemy => Vector2.Distance(enemy.Position, owner.Position)).ToList();
            if (champs.Count > 3)
            {
                foreach (var enemy in champs.GetRange(0, 4)
                     .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
                {
					SpellCast(owner, 0, SpellSlotType.ExtraSlots, true, enemy, Vector2.Zero);
                    if (Target1 == null) Target1 = enemy;
                    else if (Target2 == null) Target2 = enemy;
                    else if (Target3 == null) Target3 = enemy;                 
                }
            }
            else
            {
                foreach (var enemy in champs.GetRange(0, champs.Count)
                    .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
                {
                    SpellCast(owner, 0, SpellSlotType.ExtraSlots, true, enemy, Vector2.Zero);
                    if (Target1 == null) Target1 = enemy;
                    else if (Target2 == null) Target2 = enemy;
                    else if (Target3 == null) Target3 = enemy;                 
                }
            }
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
            StopAnimation(Owner, "Spell4");
        }

        public void OnPreAttack(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
            somerandomTick += diff;
            if (somerandomTick >= 250f)
            {
                if (Target1 != null) SpellCast(Owner, 0, SpellSlotType.ExtraSlots, true, Target1, Vector2.Zero);
                if (Target2 != null) SpellCast(Owner, 0, SpellSlotType.ExtraSlots, true, Target2, Vector2.Zero);
                if (Target3 != null) SpellCast(Owner, 0, SpellSlotType.ExtraSlots, true, Target3, Vector2.Zero);
                somerandomTick = 0;
            }

        }
    }
}
