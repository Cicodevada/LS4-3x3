using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using System;
using System.Numerics;

namespace Buffs
{
    internal class BlindMonkETwo : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; }

        IObjAiBase owner;
        ISpell originSpell;
        IBuff thisBuff;
        IAttackableUnit target;
        IParticle selfParticle;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            owner = ownerSpell.CastInfo.Owner;
            thisBuff = buff;
            originSpell = ownerSpell;
            target = buff.SourceUnit;
            AddBuff("BlindMonkETwoMissile", 4f, 1, originSpell, target, owner);				
                            
        }      
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(selfParticle);
        }

        public void OnUpdate(float diff)
        {         
        }
    }
}