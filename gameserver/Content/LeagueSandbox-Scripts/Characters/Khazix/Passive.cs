using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using System.Numerics;
using GameServerCore;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain;

namespace CharScripts
{     
    public class CharScriptKhazix : ICharScript
    {
        ISpell Spell;
        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();
        public void OnActivate(IObjAiBase owner, ISpell spell = null)
        {
            Spell = spell;
            {
				ApiEventManager.OnLevelUp.AddListener(this, owner, OnLevelUp, true);
            }
        }
		public void OnLevelUp (IAttackableUnit owner)
        {
			var Owner = Spell.CastInfo.Owner;
			var ownerSkinID = Owner.SkinID;	
            AddParticleTarget(Owner, Owner, "Khazix_Base_P_Buf_Left.troy", Owner, 25000f, 1, "L_HAND");
			AddParticleTarget(Owner, Owner, "Khazix_Base_P_Buf_Right.troy", Owner, 25000f, 1, "R_HAND");
			CreateTimer(0.1f, () =>
            {
			ApiEventManager.OnLevelUp.RemoveListener(this);
			});
        }	 
        public void OnDeactivate(IObjAiBase owner, ISpell spell = null)
        {
        }
        public void OnUpdate(float diff)
        {
        }
    }
}