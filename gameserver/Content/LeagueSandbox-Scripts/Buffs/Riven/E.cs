using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
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
    class RivenFeint : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.REPLACE_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        string pmodelname;
        IParticle pmodel;
		IParticle pbuff;
		IParticle pbuff1;
		IParticle pbuff2;
		IParticle pbuff3;
		IBuff thisBuff;

        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			var owner = ownerSpell.CastInfo.Owner as IChampion;
			pbuff = AddParticleTarget(owner, owner, "Riven_Base_E_Shield.troy", owner, buff.Duration);
			pbuff1 = AddParticleTarget(owner, owner, "Riven_Base_E_Mis.troy", owner, buff.Duration);
			pbuff2 = AddParticleTarget(owner, owner, "Riven_Base_E_Interupt.troy", owner, buff.Duration);       
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            RemoveParticle(pbuff);
			RemoveParticle(pbuff1);
			RemoveParticle(pbuff2);
        }
		public void OnLaunchAttack(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {

        }
    }
}