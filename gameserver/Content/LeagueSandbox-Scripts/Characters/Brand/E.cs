using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using System.Linq;
using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Scripting.CSharp;


namespace Spells
{
    public class BrandConflagration : ISpellScript
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
            var owner = spell.CastInfo.Owner as IChampion;
            var APratio = owner.Stats.AbilityPower.Total * 0.55f;
            var damage = 35 + spell.CastInfo.SpellLevel * 35 + APratio;
            AddParticleTarget(owner, target, "BrandConflagration_tar.troy", target, 1f);
            target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);





            if (target.HasBuff("BrandPassive"))
            {
                foreach (var enemy in GetUnitsInRange(target.Position, 550, true)
                 .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
                {

                    if (enemy is IObjAiBase)
                    {
                        enemy.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                        AddParticleTarget(owner, enemy, "BrandConflagration_tar.troy", enemy, 1f);
                        SpellCast(owner, 2, SpellSlotType.ExtraSlots, true, enemy, target.Position);
                        //AddParticlePos(owner, "BrandConflagration_mis.troy", target.Position, enemy.Position);
                        AddBuff("BrandPassive", 4f, 1, spell, enemy, owner);
                    }
                }
                AddBuff("BrandPassive", 4f, 1, spell, target, owner);
            }
            else
            {

                AddBuff("BrandPassive", 4f, 1, spell, target, owner);
            }

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



    public class BrandConflagrationMissile : ISpellScript
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
            var owner = spell.CastInfo.Owner as IChampion;

            foreach (var enemy in GetUnitsInRange(target.Position, 550, true)
             .Where(x => x.Team == CustomConvert.GetEnemyTeam(owner.Team)))
            {
                //SpellCast(owner, 2, SpellSlotType.ExtraSlots, true, enemy, target.Position);
            }

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