using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    public class Destiny_Marker : IBuffGameScript
    {
		IBuff b;
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();


        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			b = buff;
            if (unit is IObjAiBase owner)
            {
                var R2Spell = owner.SetSpell("Destiny Marker", 3, true);
                ApiEventManager.OnSpellCast.AddListener(this, R2Spell, R2OnSpellCast);
            }
        }
        public void R2OnSpellCast(ISpell spell)
        {         
             b.DeactivateBuff();		
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            (unit as IObjAiBase).SetSpell("Destiny", 3, true);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
