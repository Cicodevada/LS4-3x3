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
using System.Numerics;
using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class RengarR : IBuffGameScript
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
        IAttackableUnit Target;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisBuff = buff;
            var owner = ownerSpell.CastInfo.Owner as IChampion;
			StatsModifier.MoveSpeed.PercentBonus += 0.4f;
			StatsModifier.Range.FlatBonus = 700.0f;
			unit.AddStatModifier(StatsModifier);
			pbuff = AddParticleTarget(unit, unit, "Rengar_Base_R_Buf.troy", unit, buff.Duration);
			AddParticleTarget(unit, unit, "Rengar_Base_R_Alert.troy", unit, buff.Duration);
			AddParticleTarget(unit, unit, "Rengar_Base_R_Alert_Sound.troy", unit, buff.Duration);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, true);
            if(unit is IObjAiBase ai)
            {             
                ai.SkipNextAutoAttack();
				ai.CancelAutoAttack(true, true);
            }
            ApiEventManager.OnPreAttack.AddListener(this, owner, OnPreAttack, true);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveParticle(pbuff);
			var owner = ownerSpell.CastInfo.Owner as IChampion;
			SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);
			if (buff.TimeElapsed >= buff.Duration)
            {
                ApiEventManager.OnPreAttack.RemoveListener(this);
            }
        }

        public void OnPreAttack(ISpell spell)
        {

            var owner = spell.CastInfo.Owner as IChampion;
			owner.SkipNextAutoAttack();
			owner.CancelAutoAttack(true, true);
			AddBuff("RengarPassiveBuffDash", 2.0f, 1, spell, owner, owner);
            //SpellCast(spell.CastInfo.Owner, 0, SpellSlotType.ExtraSlots, false, spell.CastInfo.Owner.TargetUnit, Vector2.Zero);
            SealSpellSlot(owner, SpellSlotType.SpellSlots, 3, SpellbookType.SPELLBOOK_CHAMPION, false);

            if (thisBuff != null)
            {
                thisBuff.DeactivateBuff();
            }
        }
        public void OnUpdate(float diff)
        {
        }
    }
}