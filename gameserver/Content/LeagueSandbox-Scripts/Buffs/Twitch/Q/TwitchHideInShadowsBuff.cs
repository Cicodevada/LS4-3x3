using System.Numerics;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
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
    class TwitchHideInShadowsBuff : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IParticle pbuff;
        IParticle pbuff2;
        IBuff thisBuff;
		ISpell Spell;
		IObjAiBase owner;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Spell = ownerSpell;
            thisBuff = buff;
			owner = ownerSpell.CastInfo.Owner as IChampion;
			if (unit is IObjAiBase ai)
            {
			StatsModifier.AttackSpeed.PercentBonus += ((owner.GetSpell(0).CastInfo.SpellLevel * 0.05f) + 0.35f);
			unit.AddStatModifier(StatsModifier);
            ai.CancelAutoAttack(false, true);
			}
        }      
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveParticle(pbuff);
            RemoveParticle(pbuff2);
			RemoveBuff(thisBuff);				
        }	       
        public void OnUpdate(float diff)
        {
        }
    }
}