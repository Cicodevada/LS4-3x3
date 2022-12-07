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
    internal class PantheonPassiveShield : IBuffGameScript
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
		IBuff thisBuff;
        bool didcast = false;
		IParticle p;
        float findamage;

        private readonly IAttackableUnit target;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			thisBuff = buff;
            var owner = ownerSpell.CastInfo.Owner;
            Owner = owner;
            Target = unit;
            rspell = ownerSpell;
			p = AddParticleTarget(owner, owner, "Pantheon_Base_W_buf.troy", owner, buff.Duration,1,"C_BuffBone_Glb_Center_Loc");
            ApiEventManager.OnTakeDamage.AddListener(this, unit, TakeDamage, false);
        }
        public void TakeDamage(IDamageData damageData)
        {
			thisBuff.DeactivateBuff();
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveBuff(thisBuff);
			RemoveParticle(p);
			AddParticleTarget(Owner, Owner, "Fiora_Parry_block.troy", Owner);
			AddBuff("PantheonPassiveCounter", 25000f, 1, ownerSpell, Owner, Owner);
        }
        public void OnUpdate(float diff)
        {         
        }
    }
}
