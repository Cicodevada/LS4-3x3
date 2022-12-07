using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;

namespace Buffs
{
    class GragasQBoom : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IBuff ThisBuff;
        IMinion Boom;
        IParticle p;
		IParticle p2;
		IParticle p3;
        int previousIndicatorState;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ThisBuff = buff;
            Boom = unit as IMinion;
            var ownerSkinID = Boom.Owner.SkinID;
            string particles;
            CreateTimer((float) 0.0001 , () =>
            {        
	        AddBuff("GragasQ", 3.9999f, 1, ownerSpell, Boom.Owner, Boom.Owner, false);
			});				
			Boom.Owner.SetSpell("GragasQToggle", 0, true);
            p = AddParticle(Boom.Owner, null, "Gragas_Base_Q_BarrelPiece", Boom.Position, buff.Duration);
            p2 = AddParticle(Boom.Owner, null, "Gragas_Base_Q_Ally.troy", Boom.Position, buff.Duration);
            ApiEventManager.OnSpellCast.AddListener(this, Boom.Owner.GetSpell("GragasQToggle"), Q2OnSpellCast);
        }
		public void Q2OnSpellCast(ISpell spell)
        {   		
            RemoveBuff(ThisBuff);
			Boom.Owner.RemoveBuffsWithName("GragasQ");		
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Boom.Owner.SetSpell("GragasQ", 0, true);
            if (Boom != null && !Boom.IsDead)
            {
				AddParticle(Boom.Owner, null, "Gragas_Base_Q_End", Boom.Position);
                if (p != null)
                {
                    p.SetToRemove();
					p2.SetToRemove();
                }
                SetStatus(Boom, StatusFlags.NoRender, true);
                AddParticle(Boom.Owner, null, "", Boom.Position);
                Boom.TakeDamage(Boom, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
				var units = GetUnitsInRange(Boom.Position, 260f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != Boom.Owner.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
						var AP = Boom.Owner.Stats.AbilityPower.Total * 0.6f;
						var RLevel = Boom.Owner.GetSpell(0).CastInfo.SpellLevel;
						var damage = 80 + (40 * (RLevel - 1)) + AP;
                        units[i].TakeDamage(Boom.Owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
					    AddParticleTarget(Boom.Owner, units[i], ".troy", units[i], 1f);
				        AddParticleTarget(Boom.Owner, units[i], ".troy", units[i], 1f);
                    }
                }        
            }
        }
        public void OnUpdate(float diff)
        {          
        }
    }
}