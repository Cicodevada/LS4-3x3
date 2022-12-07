using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;

namespace Buffs
{
    class LeblancSlideReturn : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IBuff ThisBuff;
        IMinion LeBlanc;
        IParticle p;
        int previousIndicatorState;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ThisBuff = buff;
            LeBlanc = unit as IMinion;
            var ownerSkinID = LeBlanc.Owner.SkinID;
            string particles;       
            unit.AddStatModifier(StatsModifier);
            buff.SetStatusEffect(StatusFlags.Targetable, false);
            buff.SetStatusEffect(StatusFlags.Ghosted, true);
			switch ((LeBlanc.Owner as IObjAiBase).SkinID)
            {
                case 3:
                    particles = "LeBlanc_Base_W_return_indicator.troy";
                    break;

                case 4:
                    particles = "LeBlanc_Base_W_return_indicator.troy";
                    break;

                default:
                    particles = "LeBlanc_Base_W_return_indicator.troy";
                    break;
            }
            ApiEventManager.OnSpellCast.AddListener(this, LeBlanc.Owner.GetSpell("LeblancSlideReturn"), W2OnSpellCast);
			var direction = new Vector3(LeBlanc.Owner.Position.X, 0, LeBlanc.Owner.Position.Y);
            p = AddParticle(LeBlanc.Owner, null, particles, LeBlanc.Position, buff.Duration);
        }
		public void W2OnSpellCast(ISpell spell)
        {
			if (LeBlanc != null && !LeBlanc.IsDead)
            {
				LeBlanc.Owner.RemoveBuffsWithName("LeblancSlide");
                TeleportTo(LeBlanc.Owner, LeBlanc.Position.X, LeBlanc.Position.Y);
                LeBlanc.TakeDamage(LeBlanc, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
                AddParticle(LeBlanc.Owner, null, "LeBlanc_Base_W_return_activation.troy", LeBlanc.Owner.Position);
            }
            ThisBuff.DeactivateBuff();
			if (p != null)
                {
                    p.SetToRemove();
                }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            if (LeBlanc != null && !LeBlanc.IsDead)
            {
                if (p != null)
                {
                    p.SetToRemove();
                }            
                SetStatus(LeBlanc, StatusFlags.NoRender, true);
                AddParticle(LeBlanc.Owner, null, "LeBlanc_Base_W_return_activation.troy", LeBlanc.Owner.Position);
                LeBlanc.TakeDamage(LeBlanc, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
            }
        }
        public void OnUpdate(float diff)
        {          
        }
    }
}