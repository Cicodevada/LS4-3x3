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
    public class FizzMarinerDoom : ISpellScript
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
            var owner = spell.CastInfo.Owner;
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner as IChampion;
            var ownerSkinID = owner.SkinID;
            var targetPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var ownerPos = owner.Position;
            var distance = Vector2.Distance(ownerPos, targetPos);
            FaceDirection(targetPos, owner);

            if (distance > 1200.0)
            {
                targetPos = GetPointFromUnit(owner, 1150.0f);
            }         
            SpellCast(owner, 4, SpellSlotType.ExtraSlots, targetPos, targetPos, true, Vector2.Zero);
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

    public class FizzMarinerDoomMissile : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {     
		    TriggersSpellCasts = true,
            IsDamagingSpell = true
            // TODO
        };

        //Vector2 direction;
        ISpell spell;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {            		
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });      
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, false);
			ApiEventManager.OnSpellHit.AddListener(this, spell, TargetExecute, true);
        }
        public void OnMissileEnd(ISpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;		
            IMinion FizzBait = AddMinion(owner, "FizzBait", "FizzBait", missile.Position, owner.Team, owner.SkinID, true, false);
			IMinion FizzShark = AddMinion(FizzBait.Owner, "FizzShark", "FizzShark", FizzBait.Position, FizzBait.Owner.Team, FizzBait.Owner.SkinID, true, false);
			AddBuff("FizzSharkBuff", 1.5f, 1, spell, FizzShark, FizzShark, false);
			//PlayAnimation(FizzShark, "Spell4");
			ApiEventManager.OnSpellHit.RemoveListener(this);
			AddBuff("FizzChurnTheWatersCling", 1.5f, 1, spell, FizzBait, FizzBait, false);
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            var owner = spell.CastInfo.Owner;
			if(target is IChampion)
			{		
                AddBuff("FizzMarinerDoom", 1.5f, 1, spell, target, owner,false);
                AddParticleTarget(owner, target, "", target);
			    ApiEventManager.OnSpellMissileEnd.RemoveListener(this);
                missile.SetToRemove();
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
