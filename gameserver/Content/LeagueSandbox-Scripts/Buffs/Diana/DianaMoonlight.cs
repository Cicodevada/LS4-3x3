using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;


namespace Buffs
{
    class DianaMoonlight : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData 
        {
            BuffType = BuffType.COMBAT_DEHANCER
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IParticle p;
        IParticle p2;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			if(unit is IChampion)
			{
            p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Diana_Base_Q_Moonlight_Champ", unit, buff.Duration);
			}
			else
			{
             p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "Diana_Base_Q_Moonlight", unit, buff.Duration);
			}
            //TODO: Find the overhead particle effects
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(p);
            RemoveParticle(p2);
        }

        public void OnPreAttack(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
