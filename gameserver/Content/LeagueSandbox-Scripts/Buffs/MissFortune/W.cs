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
    public class MissFortuneViciousStrikes : IBuffGameScript
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
		IParticle p3;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            StatsModifier.MoveSpeed.PercentBonus += 0.4f;
			StatsModifier.AttackSpeed.PercentBonus = 0.15f + (0.15f * ownerSpell.CastInfo.SpellLevel);
            unit.AddStatModifier(StatsModifier);
            p = AddParticleTarget(ownerSpell.CastInfo.Owner, unit, "MissFortune_Base_W_buf.troy", unit, 2.5f,1,"WEAPON");
			p2 = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, "MissFortune_Base_W_Buf_LWeapon.troy", ownerSpell.CastInfo.Owner, 10f,1,"WEAPON");
			p3 = AddParticleTarget(ownerSpell.CastInfo.Owner, ownerSpell.CastInfo.Owner, "MissFortune_Base_W_Buf_RWeapon.troy", ownerSpell.CastInfo.Owner, 10f,1,"WEAPON");        
        }
		
		public void R2OnSpellCast(ISpell spell)
        {          
            ThisBuff.DeactivateBuff();
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {   
		     RemoveParticle(p);
             RemoveParticle(p2);
			 RemoveParticle(p3);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}