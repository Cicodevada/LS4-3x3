using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System.Collections.Generic;
using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using System.Linq;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Buffs
{
    internal class RivenTriCleave : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.DAMAGE,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
			MaxStacks = 3
        };
        public IStatsModifier StatsModifier { get; private set; }
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			buff.SetStatusEffect(StatusFlags.Ghosted, true);
            LogDebug("activate");
            if (unit.HasBuff("RivenTriCleave"))
            {
                var getbuff = unit.GetBuffWithName("RivenTriCleave");
                LogDebug(getbuff.StackCount.ToString());
                if (getbuff.StackCount < 3)
                {
                    ownerSpell.SetCooldown(0.5f, true);
                }
            }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			ApiEventManager.OnLaunchAttack.RemoveListener(this);
			buff.SetStatusEffect(StatusFlags.Ghosted, false);
            LogDebug("deactivate");
        }

        public void OnUpdate(float diff)
        {
        }
    }
}