using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using LeagueSandbox.GameServer.API;

namespace Spells
{
    public class PantheonE : ISpellScript
    {

        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            NotSingleTargetSpell = true,
            TriggersSpellCasts = true,
            //ChannelDuration = 2.5f,
        };

        private Vector2 basepos;
        public ISpellSector DamageSector;
        IObjAiBase Owner;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {

        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            SpellCast(owner, 0, SpellSlotType.ExtraSlots, false, owner, Vector2.Zero);
        }

        public void OnSpellCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
            var ownerSkinID = owner.SkinID;

            var targetPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            FaceDirection(targetPos, owner);

        }

        public void OnSpellPostCast(ISpell spell)
        {
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile swag, ISpellSector sector)
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
	public class PantheonEChannel : ISpellScript
    {

        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            NotSingleTargetSpell = true,
            TriggersSpellCasts = true,
            ChannelDuration = 2.5f,
        };

        private Vector2 basepos;
        public ISpellSector DamageSector;
        IObjAiBase Owner;

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
            var owner = spell.CastInfo.Owner;
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            FaceDirection(spellPos, owner, false);

            var sector = spell.CreateSpellSector(new SectorParameters
            {
                Length = 625f,
                SingleTick = true,
                ConeAngle = 24.76f,
                Type = SectorType.Cone
            });
            AddParticleTarget(owner, owner, "Pantheon_Base_E_cas", owner);		
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;

            var ap = owner.Stats.AttackDamage.Total * 0.8f;
            var damage = 70 + spell.CastInfo.SpellLevel * 45 + ap;

            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
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