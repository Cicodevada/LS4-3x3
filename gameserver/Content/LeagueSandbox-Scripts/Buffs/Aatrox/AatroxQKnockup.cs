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
    internal class AatroxQKnockup : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
			BuffType = BuffType.COMBAT_ENCHANCER,
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
			unit.StopMovement();
            spell = ownerSpell;
			SetStatus(unit, StatusFlags.Ghosted, true);
			ForceMovement(unit, "RUN", new Vector2(unit.Position.X + 5f, unit.Position.Y + 5f), 13f, 0, 16.5f, 0);	    
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }	
        public void OnUpdate(float diff)
        {
         
        }
    }
}