using System.Numerics;
using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    public class FioraQCD : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IBuff ThisBuff;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			ThisBuff = buff;
            if (unit is IObjAiBase owner)
            {
              owner.SetSpell("FioraQ", 0, true);
			  ApiEventManager.OnSpellPostCast.AddListener(this, owner.GetSpell("FioraQ"), Q2OnSpellCast); 
            }
        }
		public void Q2OnSpellCast(ISpell spell)
        {
			ThisBuff.DeactivateBuff();         		
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			if (unit is IObjAiBase owner)
			{			
				var t = (16 - 2* (owner.GetSpell(0).CastInfo.SpellLevel-1)) * (1 - owner.Stats.CooldownReduction.Total);
				owner.Spells[0].SetCooldown(t);
			}	
        }

        public void OnUpdate(float diff)
        {
        }
    }
}