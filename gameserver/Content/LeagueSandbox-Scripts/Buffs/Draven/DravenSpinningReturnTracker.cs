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
    class DravenSpinningReturnTracker : IBuffGameScript
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
        Vector2 O;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            thisBuff = buff;
			if (unit is IObjAiBase ai)
            {
			var owner = ownerSpell.CastInfo.Owner as IChampion;
			O = GetPointFromUnit(owner, 125.0f);
			SpellCast(owner, 3, SpellSlotType.ExtraSlots, O, Vector2.Zero, true, ai.Position); 
            pbuff = AddParticle(owner, null, "Draven_Base_Q_reticle", O,2f, 1f);
			IMinion A = AddMinion(owner, "TestCube", "TestCube", O, owner.Team, owner.SkinID, true, false);
			AddBuff("GladiatorDravenLeftAxeReturn", 3f, 1, ownerSpell, A, owner);
			}
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			var owner = ownerSpell.CastInfo.Owner as IChampion;
			RemoveParticle(pbuff);
            RemoveParticle(pbuff2);
			RemoveBuff(thisBuff);
			if (buff.TimeElapsed >= buff.Duration)
            {
            }			
        }    
        public void OnUpdate(float diff)
        {
        }
    }
}