using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Spells
{
    public class JinxR : ISpellScript
    {
		ISpellMissile m;
		IAttackableUnit t;
		float d;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true
            // TODO
        };

        //Vector2 direction;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
			m = missile;
			t = target;
            var owner = spell.CastInfo.Owner;
			var Blood = (target.Stats.HealthPoints.Total - target.Stats.CurrentHealth)*( (10 + spell.CastInfo.SpellLevel * 15)*0.01f );
			var dist= System.Math.Abs(Vector2.Distance(target.Position, owner.Position));
			var b = dist - ((250 + spell.CastInfo.SpellLevel * 100) + owner.Stats.AttackDamage.Total * 1.5f);
			var c = dist - b;
            var ad = owner.Stats.AttackDamage.Total * 0.5f;
            var damage = 75 + spell.CastInfo.SpellLevel * 50 + ad + Blood;
			d = damage;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
            missile.SetToRemove();
			Boom(spell);
        }
        public void Boom(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
			var D = d * 0.8f;
			if (owner is IChampion c)
            {
			    AddParticle(c, null, "Jinx_R_Tar.troy", t.Position);
                var units = GetUnitsInRange(t.Position, 350f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret) && units[i] != t )
                    {
                            units[i].TakeDamage(c, D, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);						   
                    }
                }             
            }		
            
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
