using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    internal class AatroxQ : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IBuff thisBuff;
        private ISpell spell;
        private IObjAiBase owner;
		IParticle P;
		string pcastname;
        string phitname;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisBuff = buff;
            owner = ownerSpell.CastInfo.Owner;
			owner.StopMovement();
            spell = ownerSpell;
			SetStatus(owner, StatusFlags.Ghosted, true);
			var Cursor = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var distance = Cursor - current;
            Vector2 truecoords;
            if (distance.Length() > 600f)
            {
                distance = Vector2.Normalize(distance);
                var range = distance * 600f;
                truecoords = current + range;
            }
            else
            {
                truecoords = Cursor;
            }
			PlayAnimation(owner, "Spell1");
			AddParticleTarget(owner, owner, "Aatrox_Base_Q_Cast.troy", owner, 10f);
			AddParticle(owner, null, "Aatrox_Base_Q_Tar_Green.troy", truecoords);
			var randPoint1 = new Vector2(owner.Position.X + (40.0f), owner.Position.Y + 40.0f);	
			ForceMovement(owner, null, randPoint1, 110, 0, 80, 0);
            FaceDirection(truecoords, spell.CastInfo.Owner, true);
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			AddBuff("AatroxQDescent", 5f, 1, spell, owner, owner);	
        }	
        public void OnUpdate(float diff)
        {
         
        }
    }
}