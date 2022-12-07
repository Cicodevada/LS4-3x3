using LeagueSandbox.GameServer.API;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System;
using GameServerCore.Enums;

//*=========================================
/*
 * ValkyrieHorns
 * Lastupdated: 3/20/2022
 * 
 * TODOS:
 * Move her passive calls to a better location than they currently are.
 * Figure out how to package her passive(s)
 * 
 * Known Issues:
 * 
*/
//*=========================================

namespace CharScripts
{
    public class CharScriptOrianna : ICharScript
    {
        IObjAiBase _orianna;
        IMinion _ball;
        private IAttackableUnit _passiveTarget = null;
        private IAttackableUnit _currentTarget = null;
        ISpell _spell;
        Buffs.OriannaBallHandler BallHandler;
        public void OnActivate(IObjAiBase owner, ISpell spell = null)
        {
            _orianna = owner;
            _spell = spell;
            
            AddBuff("ClockworkWinding", 1f, 1, spell, owner, owner, true);

            BallHandler = (AddBuff("OriannaBallHandler", 1.0f, 1, spell, owner, owner, true).BuffScript as Buffs.OriannaBallHandler);
            BallHandler.SetAttachedChampion((IChampion)owner);

            ApiEventManager.OnDeath.AddListener(owner, owner, OnDeath, false);
            ApiEventManager.OnHitUnit.AddListener(this, _orianna, TargetExecute, false);
        }

        private void TargetExecute(IDamageData data)
        {
            _currentTarget = data.Target;

            if (_passiveTarget == _currentTarget)
            {
                AddBuff("OrianaPowerDagger", 4f, 1, _spell, _orianna, _orianna);
            }
            else
            {
                _orianna.RemoveBuffsWithName("OrianaPowerDagger");
                _passiveTarget = _currentTarget;
            }
        }

        private void OnDeath(IDeathData death)
        {
        }
        public void OnDeactivate(IObjAiBase owner, ISpell spell = null)
        {
        }

        public void OnUpdate(float diff)
        {
            if (BallHandler.GetBall() == null)
            {
                BallHandler.SpawnBall(_orianna.Position);
            }
        }
    }

    public class CharScriptOriannaNoBall : ICharScript
    {
        IObjAiBase _owner;
        IMinion oriannaBall;
        public void OnActivate(IObjAiBase owner, ISpell spell = null)
        {
            _owner = owner;
            ApiEventManager.OnDeath.AddListener(owner,owner, OnDeath, false);
            AddBuff("TheBall", 1f, 1, spell, owner, owner);
            AddBuff("ClockworkWinding", 1f, 1, spell, owner, owner, true);
        }

        private void OnDeath(IDeathData death)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell = null)
        {
        }
        public void OnUpdate(float diff)
        {

            foreach (var unit in GetUnitsInRange(_owner.Position, 1290f, true))
            {
                if (unit.HasBuff("OriannaBall") && unit.Team == _owner.Team)
                {
                    oriannaBall = (IMinion)unit;
                    oriannaBall.Owner.GetBuffWithName("TheBall").Update(diff);
                    //SetStatus(oriannaBall, StatusFlags.NoRender, true);
                    //oriannaBall.TakeDamage(oriannaBall.Owner, 10000f, DamageType.DAMAGE_TYPE_TRUE, DamageSource.DAMAGE_SOURCE_INTERNALRAW, DamageResultType.RESULT_NORMAL);
                }
            }

        }
    }
}

