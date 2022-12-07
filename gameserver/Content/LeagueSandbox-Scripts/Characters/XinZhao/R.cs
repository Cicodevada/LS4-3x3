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
    public class XenZhaoParry : ISpellScript
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
            var owner = spell.CastInfo.Owner as IChampion;
            
            var ad = spell.CastInfo.Owner.Stats.AttackDamage.FlatBonus*2f;
            
            var damage = 75*spell.CastInfo.SpellLevel + ad;
			var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
			var L = dist + 500;
            var trueCoords = GetPointFromUnit(owner, L);
			//AddParticleTarget(owner, owner, "xenZiou_ult_cas.troy", owner);
			//AddParticleTarget(owner, owner, "xenZiou_ult_cas_02.troy", owner);
			//AddParticleTarget(owner, owner, "xenZiou_ult_cas_02_child.troy", owner);
			//AddParticleTarget(owner, owner, "xenZiou_ult_cas_03.troy", owner);
            var units = GetUnitsInRange(owner.Position, 500f, true);
            for (int i = 0; i < units.Count; i++)
            {
                if (!(units[i].Team == owner.Team || units[i] is IBaseTurret || units[i] is IObjBuilding || units[i] is IInhibitor))
                {
                    units[i].TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
					AddParticleTarget(owner, units[i], "xenZiou_utl_tar.troy", owner);
					AddParticleTarget(owner, units[i], "xenZiou_utl_tar_02.troy", owner);
					AddParticleTarget(owner, units[i], "xenZiou_utl_tar_03.troy", owner);
                    ForceMovement(units[i], "RUN", trueCoords, 2200, 0, 0, 0);
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

}