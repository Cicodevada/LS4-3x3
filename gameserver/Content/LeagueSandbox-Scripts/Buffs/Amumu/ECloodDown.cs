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
    internal class Tantrum : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; }

        IObjAiBase Owner;
        IAttackableUnit Target;
        ISpell rspell;
        bool didcast = false;
        float findamage;

        private readonly IAttackableUnit target;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;
            Owner = owner;
            Target = unit;
            rspell = ownerSpell;
            ApiEventManager.OnTakeDamage.AddListener(this, unit, TakeDamage, false);
        }
        public void TakeDamage(IDamageData damageData)
        {
			var owner = rspell.CastInfo.Owner;
			owner.GetSpell(2).LowerCooldown(1);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
         
        }
        public void OnUpdate(float diff)
        {         
        }
    }
}
