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
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class JinxE : ISpellScript
    {
        IObjAiBase Owner;
		Vector2 spellPos;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        { 
			FaceDirection(end, owner,true);
			spellPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
			//AddParticleTarget(owner, owner, "Jinx_W_Beam.troy", owner,10,1);
            //AddParticleTarget(owner, owner, "Jinx_W_Cas.troy", owner,10,1,"L_HAND");			
        }
        public void OnSpellCast(ISpell spell)
        {
			AddBuff("DontM", 0.5f, 1, spell, spell.CastInfo.Owner , spell.CastInfo.Owner,false);
        }
        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
            var ownerSkinID = owner.SkinID;	
			var dist= System.Math.Abs(Vector2.Distance(spellPos, owner.Position));
            for (int bladeCount = 0; bladeCount <= 2; bladeCount++)
            {
                var targetPos = GetPointFromUnit(owner, dist, (-20f + (bladeCount * 20f)));
                SpellCast(owner, 4, SpellSlotType.ExtraSlots, targetPos, targetPos, false, Vector2.Zero);
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

    public class JinxEHit : ISpellScript
    {
		IObjAiBase Owner;
		ISpell spell;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {       
        };
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
            ApiEventManager.OnSpellMissileEnd.AddListener(this, missile, OnMissileEnd, true);
        }
        public void OnMissileEnd(ISpellMissile missile)
        {
            var owner = missile.CastInfo.Owner;	
            IMinion Blade = AddMinion(owner, "JinxMine", "JinxMine", missile.Position, owner.Team, owner.SkinID, true, false);
        }
        public void TargetExecute(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
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