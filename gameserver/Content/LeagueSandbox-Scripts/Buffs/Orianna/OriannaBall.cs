using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;

//*=========================================
/*
 * ValkyrieHorns
 * Lastupdated: 3/21/2022
 * 
 * TODOS:
 * Decide if this should be attached to the ball or Orianna as an internal script
 * Known Issues:
*/
//*========================================

namespace Buffs
{
    class OriannaBall : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1
        };

        IObjAiBase _owner;
        IBuff ThisBuff;
        Buffs.OriannaBallHandler BallHandler;
        IMinion _ball;
        IParticle currentIndicator;
        int previousIndicatorState;
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier ();

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            _owner = ownerSpell.CastInfo.Owner;
            ThisBuff = buff;
            BallHandler = (_owner.GetBuffWithName("OriannaBallHandler").BuffScript as Buffs.OriannaBallHandler);
            _ball = unit as IMinion;

            buff.SetStatusEffect(StatusFlags.Targetable, false);
            buff.SetStatusEffect(StatusFlags.Ghosted, true);

            AddParticleTarget(_ball.Owner, _ball, "zed_base_w_tar", _ball);

            currentIndicator = AddParticleTarget(_ball.Owner, _ball.Owner, "OrianaBallIndicatorFar", _ball, 5f, flags: FXFlags.TargetDirection);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            if (_ball != null && !_ball.IsDead)
            {
                if (currentIndicator != null)
                {
                    currentIndicator.SetToRemove();
                }

                SetStatus(_ball, StatusFlags.NoRender, true);
                AddParticle(_ball.Owner, null, "zed_base_clonedeath", _ball.Position);
                //Ball.TakeDamage(Ball.Owner, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
            }
        }

        public int GetIndicatorState()
        {
            var dist = Vector2.Distance(_ball.Owner.Position, _ball.Position);
            var state = 0;

            if (!_ball.Owner.HasBuff("TheBall"))
            {
                return state;
            }

            if (dist >= 1290.0f)
            {
                state = 0;
            }
            else if (dist >= 1200.0f)
            {
                state = 1;
            }
            else if (dist >= 1000.0f)
            {
                state = 2;
            }
            else if (dist >= 0f)
            {
                state = 3;
            }

            return state;
        }

        public string GetIndicatorName(int state)
        {
            switch (state)
            {
                case 1:
                    {
                        return "OrianaBallIndicatorFar";
                    }
                case 2:
                    {
                        return "OrianaBallIndicatorMedium";
                    }
                case 3:
                    {
                        return "OrianaBallIndicatorNear";
                    }
                default:
                    {
                        return "OrianaBallIndicatorFar";
                    }
            }
        }

        int state;
        public void OnUpdate(float diff)
        {
            state = GetIndicatorState();

            if (!BallHandler.GetIsAttached()) 
            {
                if (state == 0)
                {
                    //SpellCast(_owner,3,SpellSlotType.ExtraSlots,true,Ball,Ball.Position);
                }
                else
                {
                    if (state != previousIndicatorState)
                    {
                        previousIndicatorState = state;
                        if (currentIndicator != null)
                        {
                            currentIndicator.SetToRemove();
                        }

                        currentIndicator = AddParticleTarget(_ball.Owner, _ball.Owner, GetIndicatorName(state), _ball, ThisBuff.Duration - ThisBuff.TimeElapsed, flags: FXFlags.TargetDirection);
                    }
                }
            }
            
        }
    }
}
