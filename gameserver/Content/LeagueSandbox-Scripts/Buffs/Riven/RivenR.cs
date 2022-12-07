using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
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
    class RivenFengShuiEngine : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        string pmodelname;
        IParticle pmodel;
		IParticle pbuff;
		IParticle pbuff1;
		IBuff thisBuff;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			var owner = ownerSpell.CastInfo.Owner as IChampion;
			pbuff = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Riven_Base_R_Buff.troy", unit, buff.Duration, bone: "L_HAND");
			pbuff1 = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Riven_Base_R_Buff.troy", unit, buff.Duration, bone: "R_HAND");
            if (unit is IChampion c)
            {
                // TODO: Implement Animation Overrides for spells like these             
                pmodel = AddParticleTarget(c, c, "Riven_Base_R_Sword", c, buff.Duration,bone: "BUFFBONE_GLB_WEAPON_2");
                StatsModifier.AttackDamage.PercentBonus = (0.4f + (0.1f * (ownerSpell.CastInfo.SpellLevel - 1))) * buff.StackCount; // StackCount included here as an example
                StatsModifier.Range.FlatBonus = 75f * buff.StackCount;
                unit.AddStatModifier(StatsModifier);    
				//ownerSpell.CastInfo.Owner.SetAutoAttackSpell("AatroxBasicAttack4", false);
            }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(pmodel);
        }
		public void OnLaunchAttack(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {

        }
    }
}
