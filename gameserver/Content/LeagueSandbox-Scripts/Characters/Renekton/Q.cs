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
    public class RenektonCleave : ISpellScript
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
        }

        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			var ownerSkinID = owner.SkinID;
			var Blood = owner.Stats.ManaPoints.Total * 0.5f;
			var Health = owner.Stats.CurrentMana;        
            if (Health >= Blood)
			{
				AddParticleTarget(owner, owner, "Renekton_Base_Q_cas_rage.troy", owner, 1f,1,"C_BuffBone_Glb_Center_Loc");
				owner.Stats.CurrentMana -= 50f;
			}			
			else
			{
				AddParticleTarget(owner, owner, "Renekton_Base_Q_cas.troy", owner, 1f,1,"C_BuffBone_Glb_Center_Loc");
			}
            PlayAnimation(owner, "Spell1", 0.8f);        
            spell.CreateSpellSector(new SectorParameters
            {
                Length = 260f,
                SingleTick = true,
                Type = SectorType.Area
            });
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
            if (owner != target)
            {
                var AD = spell.CastInfo.Owner.Stats.AttackDamage.Total * 0.6f;
                var damage = 30 + spell.CastInfo.SpellLevel * 30 + AD;
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELLAOE, false);
                AddParticleTarget(owner, null, "Renekton_Base_Q_tar.troy", target); 
                owner.Stats.CurrentMana += 10f;				
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