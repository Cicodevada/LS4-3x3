using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using System.Collections.Generic;
using GameServerCore.Domain.GameObjects.Spell.Missile;

namespace Buffs
{
    class MonkeyKingDecoy : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IObjAiBase Owner;
		IParticle p;
		IParticle p2;
		IBuff thisBuff;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			thisBuff = buff;
            Owner = ownerSpell.CastInfo.Owner;
			IMinion M = AddMinion((IChampion)Owner, "MonkeyKingClone", "MonkeyKingClone", Owner.Position, Owner.Team, Owner.SkinID, false, true);
			AddBuff("MonkeyKingDecoyClone", 3f, 1, ownerSpell, M, Owner);	

        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveParticle(p);
			RemoveBuff(thisBuff);
            RemoveParticle(p2);
        }
        public void OnUpdate(float diff)
        {

        }
    }
}