using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;

namespace Buffs
{
    public class YasuoQ : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
			BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			PlayAnimation(unit, "Spell1_Wind");
            ((IObjAiBase)unit).SetSpell("YasuoQ2W", 0, true);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            if (((IObjAiBase)unit).Spells[0].SpellName == "YasuoQ2W")
            {
				//PlayAnimation(unit,"Sheath_Run");
				PlayAnimation(unit, "Spell1");
                ((IObjAiBase)unit).SetSpell("YasuoQW", 0, true);
            }
        }

        public void OnUpdate(float diff)
        {
            //nothing!
        }
    }
}
