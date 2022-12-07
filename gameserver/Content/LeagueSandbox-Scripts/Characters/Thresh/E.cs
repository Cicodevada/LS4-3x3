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
    public class ThreshE : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
			NotSingleTargetSpell = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        private IObjAiBase _owner;
        private ISpell _spell;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            _spell = spell;
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			//FaceDirection(end, owner,true);
			//PlayAnimation(owner, "Spell1_in");
        }

        public void OnSpellCast(ISpell spell)
        {
            //var owner = spell.CastInfo.Owner;
            //AddParticleTarget(owner, owner, "", owner, bone: "");
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner as IChampion;
			//PlayAnimation(owner, "Thresh_spell1_out");
            var ownerSkinID = owner.SkinID;
			var Start = GetPointFromUnit(owner, -450f);
            var End = GetPointFromUnit(owner, 450f);
			FaceDirection(End, owner,true);
            var ownerPos = owner.Position;
            SpellCast(owner, 5, SpellSlotType.ExtraSlots, End, Vector2.Zero, true, Start);   
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
            //SetSpellToolTipVar(_owner, 2, _owner.Stats.AttackDamage.Total * _spell.SpellData.AttackDamageCoefficient, SpellbookType.SPELLBOOK_CHAMPION, 0, SpellSlotType.SpellSlots);
        }
    }

    public class ThreshEMissile1 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
			TriggersSpellCasts = true,
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
            var owner = spell.CastInfo.Owner;
            var ap = owner.Stats.AbilityPower.Total * 0.65f;
            var damage = 40 + spell.CastInfo.Owner.GetSpell(2).CastInfo.SpellLevel * 40 + ap;
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
            ForceMovement(target, null, GetPointFromUnit(missile, 200), 160, 0, 10, 0);        	
            AddParticleTarget(owner, target, "", target);

            // SpellBuffAdd EzrealRisingSpellForce
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