using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using System.Numerics;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;

namespace Buffs
{
    class FizzTrickSlam : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IBuff ThisBuff;
        private ISpell Spelll;
        private IAttackableUnit Target;
        private IObjAiBase Owner;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ThisBuff = buff;
            Owner = ownerSpell.CastInfo.Owner;
            Spelll = ownerSpell;

            buff.SetStatusEffect(StatusFlags.Targetable, false);
            buff.SetStatusEffect(StatusFlags.Ghosted, true);

            Target = unit;

            //ApiEventManager.OnSpellPostCast.AddListener(this, ownerSpell.CastInfo.Owner.GetSpell("FizzJump"), EOnSpellPostCast);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            buff.SetStatusEffect(StatusFlags.Targetable, true);
            buff.SetStatusEffect(StatusFlags.Ghosted, false);
            //ApiEventManager.OnSpellPostCast.RemoveListener(this);
        }

        public void OnUpdate(float diff)
        {
          
        }
    }
}