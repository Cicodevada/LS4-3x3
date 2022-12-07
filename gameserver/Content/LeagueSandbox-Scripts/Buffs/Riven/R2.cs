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
    public class RivenR2 : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        IBuff ThisBuff;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			ThisBuff = buff;
            if (unit is IObjAiBase owner)
            {
              owner.SetSpell("RivenIzunaBlade", 3, true);
			  ApiEventManager.OnSpellPostCast.AddListener(this, owner.GetSpell("RivenIzunaBlade"), R2OnSpellCast);
            }
        }
		public void R2OnSpellCast(ISpell spell)
        {       
            ThisBuff.DeactivateBuff();
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
             (unit as IObjAiBase).SetSpell("RivenFengShuiEngine", 3, true);
        }

        public void OnUpdate(float diff)
        {
        }
    }
}