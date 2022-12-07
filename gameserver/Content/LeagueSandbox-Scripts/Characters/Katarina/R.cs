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
    public class KatarinaR : ISpellScript
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
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile swag, ISpellSector sector)
        {


        }


        public void OnSpellChannel(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            AddBuff("KatarinaR", 2.5f, 1, spell, owner, owner);
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
        {
			var owner = spell.CastInfo.Owner;
            owner.RemoveBuffsWithName("KatarinaR");
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }






    public class KatarinaRMis : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true,
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            }
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
			ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
			var owner = spell.CastInfo.Owner;
            var AP = owner.Stats.AbilityPower.Total * 0.25f;
            var AD = owner.Stats.AttackDamage.FlatBonus * 0.375f;
            float damage = 15f + ( 20f * spell.CastInfo.SpellLevel) + AP + AD;
			target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
			AddParticleTarget(owner, target, "katarina_deathLotus_tar.troy", target);         	
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