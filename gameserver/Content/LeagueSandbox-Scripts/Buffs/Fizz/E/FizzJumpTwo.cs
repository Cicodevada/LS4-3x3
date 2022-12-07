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
    class FizzJumpTwo : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IAttackableUnit Unit;
        IObjAiBase owner;
        IParticle p;
		IBuff thisBuff;
        IParticle p2;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			thisBuff = buff;
            owner = ownerSpell.CastInfo.Owner as IChampion;
            Unit = unit;
            owner.Stats.SetActionState(ActionState.CAN_MOVE, false);			
            p = AddParticleTarget(owner, unit, "", unit, buff.Duration, 1f);
            p2 = AddParticleTarget(owner, unit, "", unit, buff.Duration, 1f);
			ApiEventManager.OnSpellCast.AddListener(this, owner.GetSpell("FizzJumpTwo"), E2OnSpellCast);
        }
		public void E2OnSpellCast(ISpell spell)
        {   		
            RemoveBuff(thisBuff);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }
        public void OnUpdate(float diff)
        {        
        }
    }
}