using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using System.Numerics;
using GameServerCore;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain;

namespace Buffs
{
    class AatroxQDescent : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IBuff thisBuff;
        private ISpell spell;
        private IObjAiBase owner;
		IParticle P;
		string pcastname;
        string phitname;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			thisBuff = buff;
            owner = ownerSpell.CastInfo.Owner;
			owner.StopMovement();
            spell = ownerSpell;
			var Cursor = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var distance = Cursor - current;
            Vector2 truecoords;
            if (distance.Length() > 600f)
            {
                distance = Vector2.Normalize(distance);
                var range = distance * 600f;
                truecoords = current + range;
            }
            else
            {
                truecoords = Cursor;
            }
			ApiEventManager.OnMoveEnd.AddListener(this, owner, OnMoveEnd, true);
            ApiEventManager.OnMoveSuccess.AddListener(this, owner, OnMoveSuccess, true);				
			P = AddParticleTarget(owner, owner, "Aatrox_Base_Q_Cast.troy", owner, 10f);
            ForceMovement(owner, null, truecoords, 2450, 0, 0, 0);
        }
        public void OnMoveEnd(IAttackableUnit unit)
        {
			if (spell.CastInfo.Owner is IChampion c)
            {
				if (c.HasBuff("AatroxR"))
                {
				OverrideAnimation(c, "RUN_ULT", "RUN");
			    }
			    else
			    {
				//OverrideAnimation(c, "RUN", "RUN_ULT");
			    }
				StopAnimation(c, "Spell1",true,true,true);
				SetStatus(owner, StatusFlags.Ghosted, false);			  			
            }
			RemoveBuff(thisBuff);		
			RemoveParticle(P);		
        }
		public void OnMoveSuccess(IAttackableUnit unit)
        {
			if (spell.CastInfo.Owner is IChampion c)
            {
				AOE(spell);	  			
            }	
        }
        public void AOE(ISpell spell)
        {
     		if (spell.CastInfo.Owner is IChampion c)
            {
				if (c.SkinID == 1)
                {
                    pcastname = "Aatrox_Skin01_Q_Land";
                }
                else
                {
                    pcastname = "Aatrox_Base_Q_Land";
                }
			    AddParticle(c, null, pcastname, c.Position);
				AddParticleTarget(c, c, "Aatrox_Base_Q_land_sound.troy", c, 10f);
                var damage = 70 + (45 * (spell.CastInfo.SpellLevel - 1)) + (c.Stats.AttackDamage.Total * 0.6f);
                var units = GetUnitsInRange(c.Position, 260f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
                            units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
						    AddParticleTarget(c, units[i], "Aatrox_Base_Q_Hit.troy", units[i], 1f);
				            AddParticleTarget(c, units[i], ".troy", units[i], 1f);
                    }
                }
                var unitss = GetUnitsInRange(c.Position, 100f, true);
                for (int i = 0; i < unitss.Count; i++)
                {	
                    if (unitss[i].Team != c.Team && !(unitss[i] is IObjBuilding || unitss[i] is IBaseTurret))
                    {
						AddBuff("AatroxQKnockup", 1f, 1, spell, unitss[i], c);	
                    }
                }
			}
        }		
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {		
        }
        public void OnUpdate(float diff)
        {
        }
    }
}