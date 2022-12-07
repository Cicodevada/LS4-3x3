using System.Collections.Generic;
using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class ZiggsQ : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = false
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
        }

        public void OnSpellCast(ISpell spell)
        {		
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner as IChampion;
            var ownerSkinID = owner.SkinID;
            var targetPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var ownerPos = owner.Position;
            var distance = Vector2.Distance(ownerPos, targetPos);
            FaceDirection(targetPos, owner);

            if (distance > 800.0)
            {
                targetPos = GetPointFromUnit(owner, 800.0f);
            }         
            SpellCast(owner, 4, SpellSlotType.ExtraSlots, targetPos, targetPos, false, Vector2.Zero);
			AddParticle(owner, null, ".troy", targetPos,10f);		
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
    public class ZiggsQSpell : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        Vector2 POS;
        ISpell Spell;
		IObjAiBase missile;
		IObjAiBase Owner;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {        
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			Spell = spell;
			var P = owner.Position;
			POS = P;
			Owner = spell.CastInfo.Owner as IChampion;
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Arc,
                OverrideEndPosition = end
            });
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }
        public void OnMissileEnd(ISpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
			var dist = System.Math.Abs(Vector2.Distance(POS, missile.Position));
			AddParticle(owner, null, "ZiggsQBounce.troy", missile.Position,10f);
			AddParticle(owner, null, ".troy", missile.Position,10f);
			SpellCast(owner, 5, SpellSlotType.ExtraSlots, GetPointFromUnit(missile, dist/2), Vector2.Zero, true, missile.Position);
        }

        public void OnSpellCast(ISpell spell)
        {
        }
		public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
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
	public class ZiggsQSpell2 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        Vector2 POS;
        ISpell Spell;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {        
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			Spell = spell;
			var P = owner.Position;
			POS = P;
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Arc,
                OverrideEndPosition = end
            });
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }
        public void OnMissileEnd(ISpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
			var dist = System.Math.Abs(Vector2.Distance(POS, missile.Position));
			AddParticle(owner, null, "ZiggsQBounce2.troy", missile.Position,10f);
			AddParticle(owner, null, ".troy", missile.Position,10f);
			SpellCast(owner, 6, SpellSlotType.ExtraSlots, GetPointFromUnit(missile, dist/6), Vector2.Zero, true, missile.Position);
        }

        public void OnSpellCast(ISpell spell)
        {
        }
		public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
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
	public class ZiggsQSpell3 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        Vector2 POS;
        ISpell Spell;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {        
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			Spell = spell;
			var P = owner.Position;
			POS = P;
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Arc,
                OverrideEndPosition = end
            });
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }
        public void OnMissileEnd(ISpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;
			//var dist = System.Math.Abs(Vector2.Distance(POS, missile.Position));
			//AddParticle(owner, null, "ZiggsQBounce.troy", missile.Position,10f);
			AddParticle(owner, null, "ZiggsQExplosion.troy", missile.Position,10f);
			//SpellCast(owner, 6, SpellSlotType.ExtraSlots, GetPointFromUnit(missile, dist/6), Vector2.Zero, true, missile.Position);
        }

        public void OnSpellCast(ISpell spell)
        {
        }
		public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
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