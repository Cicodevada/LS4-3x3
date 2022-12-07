using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System.Linq;

namespace Spells
{
    public class EkkoW: ISpellScript
    {
        ISpell spell;
		IParticle P;
		Vector2 truecoords;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
			AutoFaceDirection = false,
            CastingBreaksStealth = true,
            DoesntBreakShields = true,
            TriggersSpellCasts = true,
            IsDamagingSpell = false,
            NotSingleTargetSpell = true
        };

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
			var owner = spell.CastInfo.Owner;
			var Cursor = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var distance = Cursor - current;
            if (distance.Length() > 1600)
            {
                distance = Vector2.Normalize(distance);
                var range = distance * 1600;
                truecoords = current + range;
            }
            else
            {
                truecoords = Cursor;
            }
			AddParticle(owner, null, "Ekko_Base_W_Cas.troy", truecoords);
            CreateTimer((float) 3f , () =>{AOE(spell);});					
			//AddBuff("LeblancSlideReturn", 4.0f, 1, spell, owner, owner);		
        }

        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			var targetPos = GetPointFromUnit(owner,-125f);
			CreateTimer((float) 1.75f , () =>{SpellCast(owner, 4, SpellSlotType.ExtraSlots, truecoords, Vector2.Zero, true, targetPos);});	
            AddParticle(owner, null, "Ekko_Base_W_Indicator.troy", truecoords,10);			
			AddParticleTarget(owner, owner, "Ekko_Base_W_Branch_Timeline.troy", owner,10);					
			            
        }
		public void AOE(ISpell spell)
        {
			if (spell.CastInfo.Owner is IChampion c)
            {
                IMinion W = AddMinion(c, "TestCube", "TestCube", truecoords, c.Team, c.SkinID, true, false);
			    AddBuff("EkkoW", 2f, 1, spell, W, c, false);					
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
	public class EkkoWMis : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Circle
            },
            IsDamagingSpell = true
            // TODO
        };

        //Vector2 direction;
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
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });
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