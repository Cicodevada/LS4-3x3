using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class VorpalSpikes : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            NotSingleTargetSpell = true
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
            AddBuff("VorpalSpikes", 2500000.0f, 1, spell, owner, owner);
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

    public class VorpalSpikesMissle : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
        };
        public List<IAttackableUnit> UnitsHit = new List<IAttackableUnit>();

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            if (target == missile.CastInfo.Owner)
            {
				missile.SetToRemove();
            }
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("VorpalSpikes").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.6f;
            var damage = 30 + 25f * (spellLevel - 1) + ADratio;
            if (!UnitsHit.Contains(target))
            {
				if (target.Team != owner.Team && !(target is IObjBuilding || target is IBaseTurret))
				{
                UnitsHit.Add(target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddBuff("", 2f, 1, spell, target, owner); //TODO: Find Proper Name
                AddParticleTarget(owner, target, ".troy", target, 1f);
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
	public class VorpalSpikesMissle2 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
        };
        public List<IAttackableUnit> UnitsHit = new List<IAttackableUnit>();

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            if (target == missile.CastInfo.Owner)
            {
				missile.SetToRemove();
            }
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("VorpalSpikes").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.6f;
            var damage = 30 + 25f * (spellLevel - 1) + ADratio;
            if (!UnitsHit.Contains(target))
            {
				if (target.Team != owner.Team && !(target is IObjBuilding || target is IBaseTurret))
				{
                UnitsHit.Add(target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddBuff("", 2f, 1, spell, target, owner); //TODO: Find Proper Name
                AddParticleTarget(owner, target, ".troy", target, 1f);
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
	public class VorpalSpikesMissle3 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
        };
        public List<IAttackableUnit> UnitsHit = new List<IAttackableUnit>();

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            if (target == missile.CastInfo.Owner)
            {
				missile.SetToRemove();
            }
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("VorpalSpikes").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.6f;
            var damage = 30 + 25f * (spellLevel - 1) + ADratio;
            if (!UnitsHit.Contains(target))
            {
				if (target.Team != owner.Team && !(target is IObjBuilding || target is IBaseTurret))
				{
                UnitsHit.Add(target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddBuff("", 2f, 1, spell, target, owner); //TODO: Find Proper Name
                AddParticleTarget(owner, target, ".troy", target, 1f);
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
	public class VorpalSpikesMissle4 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
        };
        public List<IAttackableUnit> UnitsHit = new List<IAttackableUnit>();

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            if (target == missile.CastInfo.Owner)
            {
				missile.SetToRemove();
            }
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("VorpalSpikes").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.6f;
            var damage = 30 + 25f * (spellLevel - 1) + ADratio;
            if (!UnitsHit.Contains(target))
            {
				if (target.Team != owner.Team && !(target is IObjBuilding || target is IBaseTurret))
				{
                UnitsHit.Add(target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddBuff("", 2f, 1, spell, target, owner); //TODO: Find Proper Name
                AddParticleTarget(owner, target, ".troy", target, 1f);
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
	public class VorpalSpikesMissle5 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
        };
        public List<IAttackableUnit> UnitsHit = new List<IAttackableUnit>();

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            if (target == missile.CastInfo.Owner)
            {
				missile.SetToRemove();
            }
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("VorpalSpikes").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.6f;
            var damage = 30 + 25f * (spellLevel - 1) + ADratio;
            if (!UnitsHit.Contains(target))
            {
				if (target.Team != owner.Team && !(target is IObjBuilding || target is IBaseTurret))
				{
                UnitsHit.Add(target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddBuff("", 2f, 1, spell, target, owner); //TODO: Find Proper Name
                AddParticleTarget(owner, target, ".troy", target, 1f);
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
	public class VorpalSpikesMissle6 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true,
            TriggersSpellCasts = true

            // TODO
        };
        public List<IAttackableUnit> UnitsHit = new List<IAttackableUnit>();

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, false);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            UnitsHit.Clear();
        }

        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            if (target == missile.CastInfo.Owner)
            {
				missile.SetToRemove();
            }
            var owner = spell.CastInfo.Owner;
            var spellLevel = owner.GetSpell("VorpalSpikes").CastInfo.SpellLevel;
            var ADratio = owner.Stats.AttackDamage.Total * 0.6f;
            var damage = 30 + 25f * (spellLevel - 1) + ADratio;
            if (!UnitsHit.Contains(target))
            {
				if (target.Team != owner.Team && !(target is IObjBuilding || target is IBaseTurret))
				{
                UnitsHit.Add(target);
                target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_ATTACK, false);
                AddBuff("", 2f, 1, spell, target, owner); //TODO: Find Proper Name
                AddParticleTarget(owner, target, ".troy", target, 1f);
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