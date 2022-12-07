using System.Numerics;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class DianaPassiveDeathRecap : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IParticle pbuff;
        IParticle pbuff2;
        IBuff thisBuff;
		IObjAiBase owner;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisBuff = buff;
			if (unit is IObjAiBase ai)
            {
            var owner = ownerSpell.CastInfo.Owner as IChampion;
            pbuff = AddParticleTarget(ai, ai, "Diana_Base_P_Buf.troy", ai,buff.Duration, 1f);			
			ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
			}
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			var owner = ownerSpell.CastInfo.Owner as IChampion;
			RemoveParticle(pbuff);
            RemoveParticle(pbuff2);
			RemoveBuff(thisBuff);
			CreateTimer(0.5f, () =>
            {
			//SetAnimStates(owner, new Dictionary<string, string> { { "Spell1", "Attack1" } });
			});
			if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }		
        }

        public void OnLaunchAttack(ISpell spell)
        {
			
			if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {                       
            spell.CastInfo.Owner.RemoveBuff(thisBuff);
            var owner = spell.CastInfo.Owner as IChampion;
            spell.CastInfo.Owner.SkipNextAutoAttack();
            SpellCast(spell.CastInfo.Owner, 2, SpellSlotType.ExtraSlots, false, spell.CastInfo.Owner.TargetUnit, Vector2.Zero);          
            thisBuff.DeactivateBuff();
			}
        }
        public void OnUpdate(float diff)
        {
        }
    }
}