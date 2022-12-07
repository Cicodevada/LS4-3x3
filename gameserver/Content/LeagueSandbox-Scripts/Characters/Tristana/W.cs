using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using System.Linq;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class RocketJump : ISpellScript
    {
        IBuff Buff;
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
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
        }

        public void OnSpellCast(ISpell spell)
        {	
        }
        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			var Cursor = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var distance = Cursor - current;
            Vector2 truecoords;
            if (distance.Length() > 900f)
            {
                distance = Vector2.Normalize(distance);
                var range = distance * 900f;
                truecoords = current + range;
            }
            else
            {
                truecoords = Cursor;
            }
			var dist = System.Math.Abs(Vector2.Distance(truecoords, owner.Position));
            var time = dist / 1100;
			PlayAnimation(owner, "spell2",time);
            FaceDirection(truecoords, spell.CastInfo.Owner, true);
            ForceMovement(spell.CastInfo.Owner, "", truecoords, 1100, 700, 35, 700, movementOrdersFacing: GameServerCore.Enums.ForceMovementOrdersFacing.KEEP_CURRENT_FACING);
			AddParticle(owner, null, "tristana_rocketJump_cas.troy", owner.Position);
			AddParticleTarget(owner, owner, "tristana_rocketJump_cas_sparks.troy", owner);
            AddParticleTarget(spell.CastInfo.Owner, spell.CastInfo.Owner, "", spell.CastInfo.Owner, 1f);
			AddParticleTarget(owner, owner, ".troy", owner, size:1, bone: "Weapon");
			//var time1 = dist + 0.2f;
			CreateTimer((float) time , () =>
            {
            if (spell.CastInfo.Owner is IChampion c)
            {
				StopAnimation(c, "spell3");
				PlayAnimation(c, "spell3_landing",0.2f);
			    AddParticle(c, null, "tristana_rocketJump_land.troy", c.Position);
				AddParticleTarget(c, c, ".troy", c, 10f);
                var damage = 70 + (45 * (spell.CastInfo.SpellLevel - 1)) + (c.Stats.AbilityPower.Total * 0.8f);
                var units = GetUnitsInRange(c.Position, 350f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
                            units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
						    AddParticleTarget(c, units[i], "tristana_rocketJump_unit_tar.troy", units[i], 1f);
				            AddParticleTarget(c, units[i], ".troy", units[i], 1f);
                    }
                }             
            }
           });
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
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