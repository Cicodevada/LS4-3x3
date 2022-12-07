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
    public class GragasE: ISpellScript
    {

        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
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
			var Cursor = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var distance = Cursor - current;
            Vector2 truecoords;
            distance = Vector2.Normalize(distance);
            var range = distance * 500f;
            truecoords = current + range;
			var dist = System.Math.Abs(Vector2.Distance(truecoords, owner.Position));
            var time = dist / 900f;
			PlayAnimation(owner, "Spell3");
			owner.Stats.SetActionState(ActionState.CAN_MOVE, false);	 
			AddParticleTarget(owner, owner, "Gragas_Base_E_Cas.troy", owner, time);
			//AddParticleTarget(owner, owner, "Gragas_Base_E_Tar_Chest.troy", owner, time,1,"CHEST");
			//AddParticleTarget(owner, owner, "Gragas_Base_E_Tar_Ground.troy", owner, time);
			AddParticle(owner, null, ".troy", owner.Position);
			FaceDirection(truecoords, spell.CastInfo.Owner, true);
            ForceMovement(owner, null, truecoords, 900f, 0, 0, 0);
			CreateTimer((float) time , () =>
            {        
	        StopAnimation(owner, "Spell3");
			});	
        }

        public void OnSpellCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
        }

        public void OnSpellPostCast(ISpell spell)
        {	
        }
		public void OnUnitUpdateMove(ISpell spell)
        {
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