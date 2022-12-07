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
    public class HecarimRapidSlash : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };
        IObjAiBase Owner;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			Owner = owner;
			owner.CancelAutoAttack(false, false);
        }

        public void OnSpellCast(ISpell spell)
        {
			var ownerSkinID = Owner.SkinID;
            AddParticleTarget(Owner, Owner, "Hecarim_Q.troy", Owner, 1f,1,"C_BuffBone_Glb_Center_Loc");         
            //PlayAnimation(Owner, "Spell1_Fullbody");
            PlayAnimation(Owner, "Spell1_Upperbody");             
        }

        public void OnSpellPostCast(ISpell spell)
        {
			if (spell.CastInfo.Owner is IChampion c)
            {
                var damage = 30 + (35 * spell.CastInfo.SpellLevel ) + (c.Stats.AttackDamage.Total * 0.6f);
				var damageM = damage * 0.66f;

                var units = GetUnitsInRange(c.Position, 350f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
						c.GetSpell(0).LowerCooldown(1);
						AddBuff("", 8f, 1, spell, c, c);
						AddParticleTarget(c, units[i], "Hecarim_Q_tar.troy", units[i]);
                        units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
						if (units[i] is IMonster || units[i] is IMinion){units[i].TakeDamage(c, damageM, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);}
                    }
                }               
            }
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
