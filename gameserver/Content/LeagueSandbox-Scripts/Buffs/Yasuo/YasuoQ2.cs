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
    public class YasuoQ3W : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
			BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IParticle p1;
        private IParticle p2;
        private IParticle p3;
        private IParticle p4;
		private IParticle p5;
        private IParticle p6;
        private IParticle p7;
        private IParticle p8;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var caster = ownerSpell.CastInfo.Owner;
            ((IChampion)unit).SetSpell("YasuoQ3W", 0, true);
			RemoveBuff(((IChampion)unit), "YasuoQ");
            p1 = AddParticleTarget(caster, (IChampion)unit, "Yasuo_Base_Q3_Indicator_Ring", unit,10f);
            p2 = AddParticleTarget(caster, (IChampion)unit, "Yasuo_Base_Q3_Indicator_Ring_alt", unit,10f);
            p3 = AddParticleTarget(caster, (IChampion)unit, "Yasuo_Base_Q_wind_ready_buff", unit,10f);
            p4 = AddParticleTarget(caster, (IChampion)unit, "Yasuo_Base_Q_strike_build_up_test", unit,10f);
			p5 = AddParticleTarget(caster, (IChampion)unit, "temp_yasuo_wind_anivia_sound", unit,10f);
            p6 = AddParticleTarget(caster, (IChampion)unit, "yasuo_base_q_ready_sound", unit,10f);
			p7 = AddParticleTarget(caster, (IChampion)unit, "temp_yasuo_q_sound", unit,10f);
            p8 = AddParticleTarget(caster, (IChampion)unit, "yasuo_q3_sound", unit,10f);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            if (((IChampion)unit).Spells[0].SpellName == "YasuoQ3W")
            {
                ((IChampion)unit).SetSpell("YasuoQW", 0, true);
            }
            RemoveParticle(p1);
            RemoveParticle(p2);
            RemoveParticle(p3);
            RemoveParticle(p4);
        }

        public void OnUpdate(float diff)
        {
            //empty!
        }
    }
}
