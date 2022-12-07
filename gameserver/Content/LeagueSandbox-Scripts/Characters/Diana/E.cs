using System.Linq;
using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class DianaVortex : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			AddParticle(owner, null, "Diana_Base_E_MeshOverlay", owner.Position,10f);
			AddParticle(owner, null, "Diana_Base_E_Precas", owner.Position,10f);
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			AddParticle(owner, null, "Diana_Base_E_Cas", owner.Position,10f);
            var sector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 450f,
                SingleTick = true,
                Type = SectorType.Area
            });
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            var AP = spell.CastInfo.Owner.Stats.AbilityPower.Total * 0.3f;
            var AD = spell.CastInfo.Owner.Stats.AttackDamage.Total * 0.6f;
            var damage = 40 + spell.CastInfo.SpellLevel * 30 + AP + AD;
			var dist = System.Math.Abs(Vector2.Distance(target.Position, owner.Position));
			var distt = dist + 125;
			var targetPos = GetPointFromUnit(owner,distt);
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
			ForceMovement(target, null, owner.Position, 800, 0, 20, 0);
            AddParticleTarget(owner, target, "Diana_Base_E_Tar", target, 10f);
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
