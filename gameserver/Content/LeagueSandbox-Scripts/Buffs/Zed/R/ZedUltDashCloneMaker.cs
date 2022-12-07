using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using System.Numerics;
using GameServerCore;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain;

namespace Buffs
{
    internal class ZedUltDashCloneMaker : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IBuff ThisBuff;
        private ISpell Spell;
        private readonly IAttackableUnit Target = Spells.ZedUlt.Target;
        private IObjAiBase owner;
        private float ticks = 0;
        private float damage;
		IMinion Zed;
		IMinion a;
		IMinion b;
        IBuff thisBuff;
		IParticle P;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
                thisBuff = buff;
                owner = ownerSpell.CastInfo.Owner;
                Spell = ownerSpell;       
			    var ownerSkinID = owner.SkinID;			
                //var m1 = GetPointFromUnit(owner, ((System.Math.Abs(Vector2.Distance(Target.Position, owner.Position)))*2)*1.159f,  30f);
				//var m2 = GetPointFromUnit(owner, ((System.Math.Abs(Vector2.Distance(Target.Position, owner.Position)))*2)*1.159f,  -30f);
                var m1 = GetPointFromUnit(owner, 1200f,  30f);
				var m2 = GetPointFromUnit(owner, 1200f,  -30f);				
                //Zed = AddMinion((IChampion)owner, "Zed", "Zed", owner.Position, owner.Team, owner.SkinID, true, false);             
			    a = AddMinion((IChampion)owner, "ZedShadow", "ZedShadow", m1, owner.Team, owner.SkinID, true, false);
                b = AddMinion((IChampion)owner, "ZedShadow", "ZedShadow", m2, owner.Team, owner.SkinID, true, false);
				ApiEventManager.OnDeath.AddListener(this, a, OnDeath, true);
				ApiEventManager.OnDeath.AddListener(this, b, OnDeath, true);
				//var ZedEnd = GetPointFromUnit(Zed,(System.Math.Abs(Vector2.Distance(Target.Position, Zed.Position))+125f));
				//var aEnd = GetPointFromUnit(a,(System.Math.Abs(Vector2.Distance(Target.Position, a.Position))+125f));
				//var bEnd = GetPointFromUnit(b,(System.Math.Abs(Vector2.Distance(Target.Position, b.Position))+125f));
				CreateTimer(0.001f, () =>
                {
                //PlayAnimation(Zed, "spell4_strike");				
				});	
				//ForceMovement(Zed, null, Target.Position, 1400, 0, 0, 0);	
			    ForceMovement(a, null, Target.Position, 1400, 0, 0, 0);					
				ForceMovement(b, null, Target.Position, 1400, 0, 0, 0);               						 
				AddParticleTarget(owner, a, "Zed_Base_R_Dash.troy", owner);
				AddParticleTarget(owner, b, "Zed_Base_R_Dash.troy", owner);
	    }
		public void OnDeath(IDeathData data)
        {           
            AddParticleTarget(a, a, "Become_Transparent.troy", a);
			AddParticleTarget(b, b, "Become_Transparent.troy", b);
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			//AddParticleTarget(Zed, Zed, "Become_Transparent.troy", Zed);
			AddParticleTarget(a, a, "Become_Transparent.troy", a);
			AddParticleTarget(b, b, "Become_Transparent.troy", b);
			//SetStatus(Zed, StatusFlags.NoRender, true);
			SetStatus(a, StatusFlags.NoRender, true);
			SetStatus(b, StatusFlags.NoRender, true);
			//Zed.TakeDamage(Zed, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
			a.Die(CreateDeathData(false, 0, a, a, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, 0.0f));
			b.Die(CreateDeathData(false, 0, b, b, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, 0.0f));
        }

        public void OnUpdate(float diff)
        {
         
        }
    }
}