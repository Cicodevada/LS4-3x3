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
    class NinanRMisBuff : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IAttackableUnit Target;
        IBuff ThisBuff;
        IMinion Blade;
        IParticle p;
        int previousIndicatorState;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ThisBuff = buff;
            Blade = unit as IMinion;
            var ownerSkinID = Blade.Owner.SkinID;
            string particles;       
            unit.AddStatModifier(StatsModifier);
            buff.SetStatusEffect(StatusFlags.Targetable, false);
            buff.SetStatusEffect(StatusFlags.Ghosted, true);
			switch ((Blade.Owner as IObjAiBase).SkinID)
            {
				case 1:
                    particles = "Talon_Skin01_R_Blade_Hold.troy";
                    break;
				case 2:
                    particles = "Talon_Skin02_R_Blade_Hold.troy";
                    break;
                case 3:
                    particles = "Talon_Skin03_R_Blade_Hold.troy";
                    break;

                case 4:
                    particles = "Talon_Skin04_R_Blade_Hold.troy";
                    break;
				case 5:
                    particles = "Talon_Skin05_R_Blade_Hold.troy";
                    break;

                default:
                    particles = "Ninan_Base_R_Blade_Hold.troy";
                    break;
            }
			ApiEventManager.OnLaunchAttack.AddListener(this, Blade.Owner, QAOnSpellCast, false);
            ApiEventManager.OnSpellCast.AddListener(this, Blade.Owner.GetSpell("NinanRToggle"), R2OnSpellCast);
			ApiEventManager.OnSpellCast.AddListener(this, Blade.Owner.GetSpell("NinanQ"), QAOnSpellCast);			
	        ApiEventManager.OnSpellPostCast.AddListener(this, Blade.Owner.GetSpell("NinanQ"), QAOnSpellCast);
            //ApiEventManager.OnSpellCast.AddListener(this, Blade.Owner.GetSpell("NinanW"), R2OnSpellCast);			
	        ApiEventManager.OnSpellPostCast.AddListener(this, Blade.Owner.GetSpell("NinanW"), R2OnSpellCast);
			ApiEventManager.OnSpellPostCast.AddListener(this, Blade.Owner.GetSpell("NinanE"), R2OnSpellCast);
			var direction = new Vector3(Blade.Owner.Position.X, 0, Blade.Owner.Position.Y);
            p = AddParticle(Blade.Owner, Blade, particles, Blade.Position, buff.Duration,1f,"middle","middle");
            //p = AddParticlePos(Blade.Owner,particles,Blade.Position,Blade.Position,buff.Duration);
            //p = AddParticleTarget(Blade.Owner, Blade.Owner, "", Blade, buff.Duration, flags: FXFlags.TargetDirection);
        }
		public void QAOnSpellCast(ISpell spell)
        {   
		    Target = spell.CastInfo.Targets[0].Unit;
		    Blade.Owner.RemoveBuffsWithName("NinanR");
            if (Blade != null && !Blade.IsDead)
            {
                if (p != null)
                {
                    p.SetToRemove();
                }
                //Blade.Owner.RemoveBuffsWithName("TalonShadowAssaultToggle");
                SetStatus(Blade, StatusFlags.NoRender, true);
                AddParticle(Blade.Owner, null, "Talon_base_R_hold_active", p.Position);
				CreateTimer((float) 0.3 , () =>
                {
				SpellCast(Blade.Owner, 4, SpellSlotType.ExtraSlots, true, Target, p.Position);
				});
                Blade.TakeDamage(Blade, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);				
            }			
            ThisBuff.DeactivateBuff();
        }
		public void R2OnSpellCast(ISpell spell)
        {   
		    Blade.Owner.RemoveBuffsWithName("NinanR");
            if (Blade != null && !Blade.IsDead)
            {
                if (p != null)
                {
                    p.SetToRemove();
                }
                //Blade.Owner.RemoveBuffsWithName("TalonShadowAssaultToggle");
                SetStatus(Blade, StatusFlags.NoRender, true);
                AddParticle(Blade.Owner, null, "Talon_base_R_hold_active", p.Position);
				CreateTimer((float) 0.3 , () =>
                {
				SpellCast(Blade.Owner, 4, SpellSlotType.ExtraSlots, true, Blade.Owner, p.Position);
				});
				Blade.TakeDamage(Blade, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
            }			
            ThisBuff.DeactivateBuff();
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
          	R2OnSpellCast(ownerSpell);		
        }
        public void OnUpdate(float diff)
        {          
        }
    }
}