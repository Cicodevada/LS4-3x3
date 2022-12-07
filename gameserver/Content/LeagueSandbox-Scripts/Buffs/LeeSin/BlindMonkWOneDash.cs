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
    internal class BlindMonkWOneDash : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; }

        IObjAiBase owner;
        ISpell originSpell;
        IBuff thisBuff;
        private readonly IAttackableUnit Target = Spells.BlindMonkWOne.Target;
        bool toRemove;
        IParticle selfParticle;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            owner = ownerSpell.CastInfo.Owner;
            thisBuff = buff;
            originSpell = ownerSpell;
            ApiEventManager.OnMoveEnd.AddListener(this, unit, OnMoveEnd, true);
            var dashSpeed = 1700f + owner.Stats.MoveSpeed.Total;
            ForceMovement(owner, null, Target.Position, dashSpeed, 0, 0, 0);
            AddParticle(owner, null, "blindMonk_W_cas_01.troy", owner.Position, lifetime: 10f);			
            selfParticle = AddParticleTarget(owner, owner, "blindMonk_Q_resonatingStrike_mis", owner, flags: 0);
            PlayAnimation(owner, "spell2");
            toRemove = false;
            SetStatus(owner, StatusFlags.Ghosted, true);
        }

        public void OnMoveEnd(IAttackableUnit unit)
        {
            toRemove = true;
			StopAnimation(unit, "spell2", true, true, true);
        }      

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(selfParticle);
            SetStatus(owner, StatusFlags.Ghosted, false);
        }

        public void OnUpdate(float diff)
        {
            if (thisBuff != null && toRemove)
            {
                RemoveBuff(thisBuff);
            }
        }
    }
}