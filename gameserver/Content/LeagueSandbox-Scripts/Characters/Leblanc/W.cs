using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System.Linq;

namespace Spells
{
    public class LeblancSlide: ISpellScript
    {
        ISpell spell;
		IParticle P;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = false,
            NotSingleTargetSpell = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			ApiEventManager.OnMoveEnd.AddListener(this, owner, OnMoveEnd, true);
			ApiEventManager.OnMoveSuccess.AddListener(this, owner, OnMoveSuccess, true);
        }

        public void OnSpellCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			PlayAnimation(owner, "Spell2");
			P = AddParticleTarget(owner, owner, "LeBlanc_Base_W_mis.troy", owner);
			AddParticle(owner, null, "LeBlanc_Base_W_cas.troy", owner.Position);
			SetStatus(owner, StatusFlags.Ghosted, true);
            AddBuff("LeblancSlide", 4.0f, 1, spell, owner, owner);
			AddBuff("LeblancSlideMove", 3.5f, 1, spell, owner, owner);
			IMinion Leblanc = AddMinion(owner, "TestCube", "TestCube", owner.Position, owner.Team, owner.SkinID, true, false);
			AddBuff("LeblancSlideReturn", 4.0f, 1, spell, Leblanc, owner);	
			if (!owner.HasBuff("LeblancSlideM")&&owner.HasBuff("LeblancMimic"))
            {
                owner.SetSpell("LeblancSlideM", 3, true);
			}		
        }

        public void OnSpellPostCast(ISpell spell)
        {
			spell.SetCooldown(0.5f, true);
			var owner = spell.CastInfo.Owner;
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
			FaceDirection(truecoords, spell.CastInfo.Owner, true);
            ForceMovement(owner, null, truecoords, 1450, 0, 0, 0);        
        }
		public void OnMoveEnd(IAttackableUnit owner)
        {
			SetStatus(owner, StatusFlags.Ghosted, false);
            StopAnimation(owner, "Spell2",true,true,true);
			RemoveParticle(P);
        }
		public void OnMoveSuccess(IAttackableUnit owner)
        {
            if (owner is IChampion c)
            {
			    AddParticle(c, null, "LeBlanc_Base_W_aoe_impact_02.troy", c.Position);
                var units = GetUnitsInRange(c.Position, 260f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
						var AP = c.Stats.AbilityPower.Total * 0.65f;
						var damage = 85 + (40 * (c.GetSpell(1).CastInfo.SpellLevel - 1)) + AP;
						var Qdamage = 55 + 25f*(c.GetSpell(0).CastInfo.SpellLevel - 1) + AP;
			            var QMarkdamage = Qdamage + damage;
						var RQdamage = 100f * c.GetSpell(3).CastInfo.SpellLevel + AP;
			            var RQMarkdamage = RQdamage + damage;
						AddParticleTarget(c, units[i], "LeBlanc_Base_W_tar.troy", units[i], 1f);
				        AddParticleTarget(c, units[i], "LeBlanc_Base_W_tar_02.troy", units[i], 1f);
						if (units[i].HasBuff("LeblancChaosOrb"))
                            {
							units[i].TakeDamage(c, QMarkdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, true);					
					        units[i].RemoveBuffsWithName("LeblancChaosOrb");
                            }
						else if (units[i].HasBuff("LeblancChaosOrbM"))
                            {
							units[i].TakeDamage(c, RQMarkdamage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, true);							
					        units[i].RemoveBuffsWithName("LeblancChaosOrbM");
                            }
						else
							{
                            units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);						   
						    }
                    }
                }             
            }			
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }

    }
	public class LeblancSlideReturn: ISpellScript
    {

       public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }

    }

}