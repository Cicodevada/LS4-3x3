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
    class AatroxWLife : IBuffGameScript
    {
		public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
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
			ApiEventManager.OnSpellCast.AddListener(this, owner.GetSpell("AatroxW"), WOnSpellCast);
        }
		public void WOnSpellCast(ISpell spell)
        {   		
            owner.RemoveBuffsWithName("AatroxWLife");
			owner.SetSpell("AatroxW2", 1, true);
			AddBuff("AatroxWPower", 25000f, 1, spell, owner, owner);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
			RemoveBuff(thisBuff);
            RemoveParticle(p2);
        }
        public void OnUpdate(float diff)
        {        
        }
    }
}