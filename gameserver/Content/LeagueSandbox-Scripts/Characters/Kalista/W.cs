using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using GameServerLib.GameObjects.AttackableUnits;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Enums;

namespace Spells
{
    public class KalistaW : ISpellScript
    {
		IAttackableUnit Target;
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
			Target = target;         
        }

        public void OnSpellCast(ISpell spell)
        {	
        }

        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
            var targetPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
			var targetPos2 = owner.Position;
			var dist = System.Math.Abs(Vector2.Distance(targetPos, owner.Position));
            //var time = dist / 535f;
            FaceDirection(targetPos, owner);       		
			IMinion M = AddMinion((IChampion)owner, "KalistaSpawn", "KalistaSpawn", GetPointFromUnit(owner, 175f), owner.Team, owner.SkinID, false, true);
			M.SetWaypoints(GetPath(M.Position, targetPos));
			//FaceDirection(targetPos, M); 
            //ForceMovement(M, null, targetPos, 535, 0, 0, 0);			
			AddBuff("KalistaW", int.MaxValue, 1, spell, M, owner);
            //CreateTimer((float) time , () =>
            //{  
               //ForceMovement(M, null, targetPos2, 535, 0, 0, 0);
               //PlayAnimation(M, "RUN");			   
            //});		
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
