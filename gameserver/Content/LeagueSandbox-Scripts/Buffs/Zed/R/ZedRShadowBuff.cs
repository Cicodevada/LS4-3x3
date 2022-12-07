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
    class ZedRShadowBuff : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IBuff ThisBuff;
        IMinion Shadow;
        IParticle p;
		IParticle p2;
		IParticle p3;
		IParticle currentIndicator;
        int previousIndicatorState;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ThisBuff = buff;
            Shadow = unit as IMinion;
            var ownerSkinID = Shadow.Owner.SkinID;
            string particles;			
            p = AddParticle(Shadow.Owner, null, "", Shadow.Position, buff.Duration);
            p2 = AddParticle(Shadow.Owner, null, ".troy", Shadow.Position, buff.Duration);
			AddParticleTarget(Shadow.Owner, Shadow, "zed_base_w_tar.troy", Shadow);
            ApiEventManager.OnSpellCast.AddListener(this, Shadow.Owner.GetSpell("ZedR2"), R2OnSpellCast);
			ApiEventManager.OnSpellCast.AddListener(this, Shadow.Owner.GetSpell("ZedShuriken"), QOnSpellCast);
            ApiEventManager.OnSpellPostCast.AddListener(this, Shadow.Owner.GetSpell("ZedShuriken"), QOnSpellPostCast);
            ApiEventManager.OnSpellCast.AddListener(this, Shadow.Owner.GetSpell("ZedPBAOEDummy"), EOnSpellCast);
			currentIndicator = AddParticleTarget(Shadow.Owner, Shadow.Owner, "zed_shadowindicatornearbloop.troy", Shadow, buff.Duration, flags: FXFlags.TargetDirection);
        }
		public void QOnSpellCast(ISpell spell)
        {
            if (Shadow != null && !Shadow.IsDead)
            {
                PlayAnimation(Shadow, "Spell1");
                var targetPos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);
                FaceDirection(targetPos, Shadow);
            }
        }

        public void QOnSpellPostCast(ISpell spell)
        {
            if (Shadow != null && !Shadow.IsDead)
            {
                var owner = spell.CastInfo.Owner;
                var targetPos = new Vector2(spell.CastInfo.TargetPositionEnd.X, spell.CastInfo.TargetPositionEnd.Z);

                SpellCast(Shadow.Owner, 1, SpellSlotType.ExtraSlots, targetPos, Vector2.Zero, true, Shadow.Position);
            }
        }
        public void EOnSpellCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			var ownerSkinID = Shadow.Owner.SkinID;
            if (Shadow != null && !Shadow.IsDead)
            {
                SpellCast(Shadow.Owner, 2, SpellSlotType.ExtraSlots, true, Shadow, Vector2.Zero);
                PlayAnimation(Shadow, "Spell3", 0.5f);
				AddParticle(Shadow.Owner, null, "Zed_Base_E_cas.troy", Shadow.Position);
            }
        }
		public void R2OnSpellCast(ISpell spell)
        {
            var ownerPos = Shadow.Owner.Position;
            currentIndicator = AddParticleTarget(Shadow.Owner, Shadow.Owner, "zed_shadowindicatorfar.troy", Shadow, ThisBuff.Duration, flags: FXFlags.TargetDirection);			
            if (Shadow != null && !Shadow.IsDead)
            {
				TeleportTo(Shadow.Owner, Shadow.Position.X, Shadow.Position.Y);
				TeleportTo(Shadow, ownerPos.X, ownerPos.Y);
				AddParticleTarget(Shadow.Owner, Shadow.Owner, "zed_base_cloneswap.troy", Shadow.Owner);
				AddParticleTarget(Shadow.Owner, Shadow, "zed_base_cloneswap.troy", Shadow);
				AddParticle(Shadow.Owner, null, "", Shadow.Position);
                AddParticle(Shadow.Owner, null, "", Shadow.Position);
            }
            Shadow.Owner.RemoveBuffsWithName("ZedRHandler");			
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveParticle(currentIndicator);  
            if (Shadow != null && !Shadow.IsDead)
            {
				AddParticle(Shadow.Owner, null, "", Shadow.Position);
				if (currentIndicator != null)
                {
                    currentIndicator.SetToRemove();
                }
                if (p != null)
                {
                    p.SetToRemove();
					p2.SetToRemove();
                }
                SetStatus(Shadow, StatusFlags.NoRender, true);
				currentIndicator.SetToRemove();
                AddParticle(Shadow.Owner, null, "", Shadow.Position);
				AddParticle(Shadow.Owner, null, "zed_base_clonedeath.troy", Shadow.Position);
                Shadow.TakeDamage(Shadow, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
            }
        }
        public void OnUpdate(float diff)
        {          		
        }
    }
}