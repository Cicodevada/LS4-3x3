using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class ZedUlt : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IBuff ThisBuff;
        private ISpell Spell;
        private readonly IAttackableUnit Target = Spells.ZedUlt.Target;
        private IObjAiBase Owner;
        private float ticks = 0;
        private float damage;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ThisBuff = buff;
            Owner = ownerSpell.CastInfo.Owner;
			Owner.StopMovement();
            Spell = ownerSpell;
            IMinion Shadow = AddMinion(Owner, "ZedShadow", "ZedShadow", Owner.Position, Owner.Team, Owner.SkinID, true, false);
			AddBuff("ZedRShadowBuff", 6.0f, 1, Spell, Shadow, Owner); 			
			AddParticleTarget(Owner, Owner, "Zed_Base_R_cas.troy", Owner, 10f);
			SealSpellSlot(Owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, true);
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			AddBuff("ZedUltDash", 6.0f, 1, Spell, Owner, Owner, false);			
        }     
        public void OnUpdate(float diff)
        {
            ticks += diff;
			if (ticks >= 0f)
			{
				//FaceDirection(Target.Position, Owner, true);
				ticks = 0f;
			}
        }
    }
}