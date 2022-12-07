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
    public class XerathArcaneBarrage2: ISpellScript
    {

        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
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
			AddParticle(owner, null, "Xerath_Base_W_cas.troy", truecoords);
			IParticle p = AddParticle(owner, null, "Xerath_Base_W_aoe_green.troy", truecoords);
			FaceDirection(truecoords, spell.CastInfo.Owner, true);
			CreateTimer((float) 0.625f , () =>
            {
            RemoveParticle(p);				
			if (spell.CastInfo.Owner is IChampion c)
            {
				//c.GetSpell(1).LowerCooldown(20);
			    AddParticle(c, null, "Xerath_Base_W_aoe_explosion.troy", truecoords);
				AddParticle(c, null, ".troy", truecoords);
                var units = GetUnitsInRange(truecoords, 250f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
				
                        var damage = 75 + (45 * (spell.CastInfo.SpellLevel - 1)) + (c.Stats.AbilityPower.Total * 0.6f);
						units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);                     
						AddParticleTarget(c, units[i], "Xerath_Base_W_tar.troy", units[i], 1f);
				        AddParticleTarget(c, units[i], "Xerath_base_w_tar_directhit.troy", units[i], 1f);
                    }
                }             
            }
			});		
        }

        public void OnSpellPostCast(ISpell spell)
        {	
        }
		public void OnUnitUpdateMove(ISpell spell)
        {
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