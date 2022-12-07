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
    internal class RengarPassiveBuffDash : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IBuff ThisBuff;
        private ISpell Spell;
        IAttackableUnit Target;
        private IObjAiBase owner;
        private float ticks = 0;
        private float damage;
        IBuff thisBuff;
		IParticle P;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisBuff = buff;
            owner = ownerSpell.CastInfo.Owner;
			SetStatus(owner, StatusFlags.Ghosted, true);
            Spell = ownerSpell;
            ApiEventManager.OnMoveEnd.AddListener(this, owner, OnMoveEnd, true);		
            var Target = Spell.CastInfo.Targets[0].Unit;
			var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
			var distt = dist - 125;
			var time = distt / 2400;
			var targetPos = GetPointFromUnit(owner,distt);
			FaceDirection(targetPos, Spell.CastInfo.Owner, true);
			PlayAnimation(owner, "dash1",4f); 
            ForceMovement(Spell.CastInfo.Owner, null, targetPos, 2400, 0, 120, 0);
			AddParticleTarget(owner, owner, "Rengar_Base_P_Leap_Dust.troy", owner);
			AddParticleTarget(owner, owner, "Rengar_Base_P_Leap_Grass.troy", owner);
        }
		public void OnMoveEnd(IAttackableUnit unit)
        {  
			SetStatus(owner, StatusFlags.Ghosted, false);
			RemoveBuff(thisBuff);			
			RemoveParticle(P);		
			//StopAnimation(owner, "", true, true, true);
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {
         
        }
    }
}