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
    internal class NinanE2 : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_DEHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; }

        ISpell originSpell;
        IBuff thisBuff;
        IRegion bubble1;
        IRegion bubble2;
        IParticle p;
		IObjAiBase Owner;
        IAttackableUnit Unit;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Unit = unit;
			Owner = ownerSpell.CastInfo.Owner;
            originSpell = ownerSpell;
            thisBuff = buff;
            Owner.SetSpell("NinanE2", 2, true);
			Owner.GetSpell(2).SetCooldown(1f, true);
            ApiEventManager.OnSpellCast.AddListener(this, Owner.GetSpell("NinanE2"), E2OnSpellCast);
            AddParticleTarget(Owner, unit, "", unit, buff.Duration);
            AddParticleTarget(Owner, unit, "", unit, buff.Duration);
            p = AddParticleTarget(Owner, unit, "Ninan_Base_E2_target_tell", unit, buff.Duration);
        }
        public void E2OnSpellCast(ISpell spell)
        {   		
			var dist = System.Math.Abs(Vector2.Distance(Unit.Position, Owner.Position));
			var distt = dist - 125;
			var time = distt/2200f;
			var targetPos = GetPointFromUnit(Owner,distt);
			var ad = (Owner.Stats.AttackDamage.Total - Owner.Stats.AttackDamage.BaseValue) * 0.5f;
            var damage = 35 + 20* (Owner.GetSpell(2).CastInfo.SpellLevel-1) + ad;
			PlayAnimation(Owner, "Spell3");                         
			FaceDirection(Unit.Position, Owner, true);
            ForceMovement(Owner, null, Unit.Position, 2200, 0, 0, 0);
			AddParticleTarget(Owner, Owner, "Talon_Base_E_cas_trail.troy", Owner, time,1,"chest");
			AddParticleTarget(Owner, Owner, "Talon_Base_E_land.troy", Owner, time,1,"chest");
            thisBuff.DeactivateBuff();
			CreateTimer((float) time , () =>
            {
            StopAnimation(Owner, "Spell3",true,true,true);
            PlayAnimation(Owner, "Spell1",0.5f);  				
			AddParticleTarget(Owner, Unit, ".troy", Owner);          
            Unit.TakeDamage(Owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
			AddParticleTarget(Owner, Unit, "Talon_Base_Q1_cas", Unit,10f, 1f,"chest","chest");
            //AddParticleTarget(Owner, Unit, "Talon_Base_W_Tar.troy", Owner);
			AddParticleTarget(Owner, Unit, "Talon_Base_Q1_tar", Unit,10f, 1f,"chest","chest");
            PlaySound("Play_sfx_Talon_TalonQAttack_OnHit", Owner);				
            AddParticleTarget(Owner, Unit, ".troy", Owner);	
            AddParticleTarget(Owner, Unit, "", Unit);			
            });
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Owner.SetSpell("NinanE", 2, true);
            p.SetToRemove();
        }

        public void OnUpdate(float diff)
        {
        }
    }
}