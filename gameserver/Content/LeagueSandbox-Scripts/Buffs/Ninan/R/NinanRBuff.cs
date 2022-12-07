using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    public class NinanR : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IBuff ThisBuff;
        IParticle p;
		IParticle p2;
		IParticle p3;
        Vector2 v;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			v = ownerSpell.CastInfo.Owner.Position;
			//ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("TalonRake"), WOnSpellCast);
			//ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("TalonCutthroat"), EOnSpellPostCast);
			//ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("TalonBasicAttack"), AOnSpellCast);
			ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("NinanRToggle"), R2OnSpellCast);
            StatsModifier.MoveSpeed.PercentBonus += 0.4f;
            unit.AddStatModifier(StatsModifier);
			AddParticle(ownerSpell.CastInfo.Owner, null, "Talon_base_R_Blade_Hold_Sound3.troy", ownerSpell.CastInfo.Owner.Position, 10f);
            p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Talon_Base_R_Cas_Invis.troy", unit,buff.Duration);
			p2 = AddParticle(ownerSpell.CastInfo.Owner, null, "Talon_base_R_Blade_Hold_Sound1.troy", ownerSpell.CastInfo.Owner.Position, 10f);
			p3 = AddParticle(ownerSpell.CastInfo.Owner, null, "Talon_Base_R_Cas.troy", ownerSpell.CastInfo.Owner.Position);
            if (unit is IObjAiBase owner)
            {
          
                var r2Spell = owner.SetSpell("NinanRToggle", 3, true);
				//CreateTimer((float) 0.5f , () =>
                //{
				//ownerSpell.CastInfo.Owner.GetSpell("TalonShadowAssaultToggle").SetCooldown(0f);
				//});
            }
        }
		
		public void R2OnSpellCast(ISpell spell)
        {          
            ThisBuff.DeactivateBuff();
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {   
             RemoveParticle(p2);
			 RemoveParticle(p3);
			 PlaySound("Play_vo_Talon_TalonShadowAssaultBuff_OnBuffDeactivate", unit);			
             AddParticle(unit, null, "Talon_base_R_Blade_Hold_Sound2", v);			 
             (unit as IObjAiBase).SetSpell("NinanR", 3, true);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}