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
    internal class ZedUltDash : IBuffGameScript
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
        IBuff thisBuff;
		IParticle P;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisBuff = buff;
            owner = ownerSpell.CastInfo.Owner;
			FaceDirection(Target.Position, owner, true);
			SetStatus(owner, StatusFlags.Ghosted, true);
			//SetStatus(owner, StatusFlags.NoRender, true);
            Spell = ownerSpell;
            ApiEventManager.OnMoveEnd.AddListener(this, owner, OnMoveEnd, true);			
            var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
			var distt = dist + 125;
			var targetPos = GetPointFromUnit(owner,distt);
			PlayAnimation(owner, "spell4_strike");	
            FaceDirection(targetPos, Spell.CastInfo.Owner, true);
            ForceMovement(owner, null, targetPos, 1400f, 0, 0, 0);
            P = AddParticleTarget(owner, owner, "Zed_Base_R_Dash.troy", owner);
			AddBuff("ZedUltDashCloneMaker", 0.5f, 1, Spell, owner, owner);
        }
		public void OnMoveEnd(IAttackableUnit unit)
        {
			if (owner.Team != Target.Team && Target is IChampion)
            {
                owner.SetTargetUnit(Target, true);
                owner.UpdateMoveOrder(OrderType.AttackTo, true);
            }
			SetStatus(owner, StatusFlags.Ghosted, false);
			//SetStatus(owner, StatusFlags.NoRender, false);
			RemoveBuff(thisBuff);
			owner.RemoveBuffsWithName("ZedUltBuff");
            //owner.RemoveBuffsWithName("ZedUltDashCloneMaker");			
			RemoveParticle(P);		
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);
			AddBuff("ZedUltExecute", 3f, 1, Spell, Target, owner);
			StopAnimation(owner, "spell4_strike", true, true, true);
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {
         
        }
    }
}