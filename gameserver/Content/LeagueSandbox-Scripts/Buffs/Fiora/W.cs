using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;

namespace Buffs
{
    internal class FioraRiposte : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IBuff thisBuff;
        IParticle highlander;
        string particle;
		float findamage;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			thisBuff = buff;
            if (unit is IObjAiBase owner)
            {
            var ELevel = owner.GetSpell(1).CastInfo.SpellLevel;	
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, true);
            ApiEventManager.OnTakeDamage.AddListener(this, owner, TakeDamage, false);			       
			}
        }
		public void TakeDamage(IDamageData damageData)
        {
            findamage -= damageData.Damage;
			thisBuff.DeactivateBuff();    
        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(highlander);
			if (buff.TimeElapsed >= buff.Duration)
            {
				ApiEventManager.OnTakeDamage.RemoveListener(this);
                //ApiEventManager.OnLaunchAttack.RemoveListener(this);
            }
			if (unit is IObjAiBase ai)
            {
                SealSpellSlot(ai, SpellSlotType.SpellSlots, 1, SpellbookType.SPELLBOOK_CHAMPION, false);
            }
        }

        private void OnAutoAttack(IAttackableUnit target, bool isCrit)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}