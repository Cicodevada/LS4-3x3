using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using GameServerLib.GameObjects.AttackableUnits;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Enums;

namespace Spells
{
    public class XenZhaoSweep : ISpellScript
    {
		IAttackableUnit Target;
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
			Target = target;         
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
            var ap = spell.CastInfo.Owner.Stats.AbilityPower.Total*0.6f;
            var damage = 70*spell.CastInfo.SpellLevel + ap;
			var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
			var distt = dist - 125f;
			var time = distt/2200f;
			var truepos = GetPointFromUnit(owner,distt);
			PlayAnimation(owner, "Spell3",0.3f);
			AddParticle(owner, null, ".troy", owner.Position, lifetime: 10f);
			AddParticleTarget(owner, owner, "xenZiou_AudaciousCharge_self_trail_01.troy", owner, time);
			ForceMovement(owner, null, truepos, 2200, 0, 0, 0);
			CreateTimer((float) time , () =>
            {  
			AddParticleTarget(owner, Target, "xenZiou_AudaciousCharge_tar_unit_instant.troy", owner);
            var units = GetUnitsInRange(Target.Position, 250f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == owner.Team || units[i] is IBaseTurret || units[i] is IObjBuilding || units[i] is IInhibitor))
                {
                    units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                    AddParticleTarget(owner, units[i], "xenZiou_AudaciousCharge_tar_03_unit_tar.troy", owner);               	
                    AddParticleTarget(owner, units[i], ".troy", owner, 10f);					
                }
            }
			});			
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
