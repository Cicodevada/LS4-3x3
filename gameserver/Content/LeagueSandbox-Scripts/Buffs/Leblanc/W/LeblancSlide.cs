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
    public class LeblancSlide : IBuffGameScript
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

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			//buff.SetStatusEffect(StatusFlags.Targetable, false);	
		    buff.SetStatusEffect(StatusFlags.Ghosted, true);
			ApiEventManager.OnSpellCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("LeblancSlideReturn"), W2OnSpellCast);         
            if (unit is IObjAiBase owner)
            {
          
                var r2Spell = owner.SetSpell("LeblancSlideReturn", 1, true);
            }
        }
		
		public void W2OnSpellCast(ISpell spell)
        {          
            ThisBuff.DeactivateBuff();
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {   
             RemoveParticle(p2);
			 buff.SetStatusEffect(StatusFlags.Ghosted, false);
			 //buff.SetStatusEffect(StatusFlags.Targetable, true);
             (unit as IObjAiBase).SetSpell("LeblancSlide", 1, true);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}