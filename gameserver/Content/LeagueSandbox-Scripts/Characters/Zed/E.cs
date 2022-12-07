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
    public class ZedPBAOEDummy : ISpellScript
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
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
			var ownerSkinID = owner.SkinID;
            AddParticleTarget(owner, null, "Zed_Base_E_cas.troy", owner);         
            PlayAnimation(owner, "Spell3", 0.5f);        
            spell.CreateSpellSector(new SectorParameters
            {
                Length = 250f,
                SingleTick = true,
                Type = SectorType.Area
            });
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            if (owner != target)
            {
                var AD = spell.CastInfo.Owner.Stats.AttackDamage.Total * 0.6f;
                var damage = 40 + spell.CastInfo.SpellLevel * 30 + AD;
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                AddParticleTarget(owner, null, "Zed_Base_E_tar.troy", target);

                owner.GetSpell(2).LowerCooldown(2);
                if (target.HasBuff("ZedSlow"))
                {
                    AddBuff("ZedSlow", 1.5f, 1, spell, target, owner);
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
    public class ZedPBAOE : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
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
            var ownerSkinID = owner.SkinID;
            AddParticleTarget(owner, null, "Zed_Base_E_cas.troy", owner);     
            PlayAnimation(owner, "Spell3", 0.5f);        
            spell.CreateSpellSector(new SectorParameters
            {
                Length = 250f,
                SingleTick = true,
                Type = SectorType.Area
            });
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            if (owner != target)
            {
                var AD = spell.CastInfo.Owner.Stats.AttackDamage.Total * 0.6f;
                var damage = 40 + spell.CastInfo.SpellLevel * 30 + AD;

                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                AddParticleTarget(owner, null, "Zed_Base_E_tar.troy", target);
                if (!target.HasBuff("ZedSlow"))
                {
                    AddBuff("ZedSlow", 1.5f, 1, spell, target, owner);
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
