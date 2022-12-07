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
    class TwitchHideInShadows : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IParticle pbuff;
        IParticle pbuff2;
        IBuff thisBuff;
		ISpell Spell;
		IObjAiBase owner;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Spell = ownerSpell;
            thisBuff = buff;
			owner = ownerSpell.CastInfo.Owner as IChampion;
			if (unit is IObjAiBase ai)
            {
			StatsModifier.MoveSpeed.PercentBonus += 0.1f;
			unit.AddStatModifier(StatsModifier);
            SealSpellSlot(ai, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, true);
			pbuff = AddParticleTarget(ai, ai, "Twitch_Base_Q_Haste", ai, lifetime: buff.Duration);
			AddParticle(ai, null, "Twitch_Base_Q_Bamf", ai.Position, lifetime: buff.Duration);
            AddParticle(ai, null, "Twitch_Base_Q_Cas_Invisible", ai.Position, lifetime: buff.Duration);		
			ApiEventManager.OnLaunchAttack.AddListener(this, ai, OnLaunchAttack, false);
            ai.CancelAutoAttack(false, true);
			}
        }
        public void OnLaunchAttack(ISpell spell)
        {
			
			if (thisBuff != null && thisBuff.StackCount != 0 && !thisBuff.Elapsed())
            {                              
            thisBuff.DeactivateBuff();
			}
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveParticle(pbuff);
            RemoveParticle(pbuff2);
			RemoveBuff(thisBuff);
			if (buff.TimeElapsed >= buff.Duration)
            {
				ApiEventManager.OnPreAttack.RemoveListener(this);
                ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
			if (unit is IObjAiBase ai)
            {
				AddBuff("TwitchHideInShadowsBuff", 5f, 1, Spell, ai, ai);
				AddParticle(ai, null, "Twitch_Base_Q_Invisiible_Outro", ai.Position, lifetime: buff.Duration);
                SealSpellSlot(ai, SpellSlotType.SpellSlots, 0, SpellbookType.SPELLBOOK_CHAMPION, false);
            }
        }	       
        public void OnUpdate(float diff)
        {
        }
    }
}
