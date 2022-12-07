using System.Numerics;
using GameServerCore;
using GameServerCore.Domain;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class ZiggsE : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            AutoFaceDirection = false,
            TriggersSpellCasts = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }
		public void OnLevelUp (ISpell spell)
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
			var owner = spell.CastInfo.Owner as IChampion;
            var spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            SpellCast(owner, 2, SpellSlotType.ExtraSlots, spellPos, spellPos, true, Vector2.Zero);
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

    public class ZiggsE2 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        ISpell S;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {         
            S = spell;
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });

            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }

        public void OnMissileEnd(ISpellMissile missile)
        {
			var owner = missile.CastInfo.Owner;
			for (int t = 0; t <= 7; t++)
            {				           
                var w = GetPointFromUnit(missile, 200f, t * 45f);	
				SpellCast(owner, 3, SpellSlotType.ExtraSlots, w, Vector2.Zero, true, missile.Position);					
            }
			for (int t = 0; t <= 2; t++)
            {				           
				var n = GetPointFromUnit(missile, 60f, t * 120f);
				SpellCast(owner, 3, SpellSlotType.ExtraSlots, n, Vector2.Zero, true, missile.Position);					            			
            }
			AddParticle(owner, null, "ZiggsE_mis_groundhit.troy", missile.Position, 10f);
            //T = AddMinion(owner, "TestCube", "TestCube", missile.Position, owner.Team, owner.SkinID, true, false);
			//AddBuff("ZiggsE", 5f, 1, S, T, owner);
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
	public class ZiggsE3 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        ISpell S;
        IMinion T;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {         
            S = spell;
            var missile = spell.CreateSpellMissile(new MissileParameters
            {
                Type = MissileType.Circle,
                OverrideEndPosition = end
            });

            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }

        public void OnMissileEnd(ISpellMissile missile)
        {
			var owner = missile.CastInfo.Owner;
			T = AddMinion(owner, "TestCubeRender", "TestCube", missile.Position, owner.Team, owner.SkinID, true, false);
			AddBuff("ZiggsE", 10f, 1, S, T, owner);
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
