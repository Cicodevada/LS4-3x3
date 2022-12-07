using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using System.Collections.Generic;
using GameServerCore.Domain.GameObjects.Spell.Missile;

namespace Buffs
{
    class KalistaW : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffAddType = BuffAddType.RENEW_EXISTING
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        IObjAiBase Owner;
		IParticle p;
		IParticle p2;
		IBuff thisBuff;
		float T;
		float S;
		IAttackableUnit Unit;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			Unit = unit;
			thisBuff = buff;
			Owner = ownerSpell.CastInfo.Owner;
			S = Unit.Stats.MoveSpeed.Total;
			PlayAnimation(Owner, "Spawn");
			OverrideAnimation(unit, "Run_ULT", "RUN");
            AddParticleTarget(Owner, unit, "Kalista_Base_W_Alerted.troy", unit, int.MaxValue,1,"HEAD");
		    AddParticleTarget(Owner, unit, "Kalista_Base_W_Avatar.troy", unit, int.MaxValue,1,"HEAD");
		    AddParticleTarget(Owner, unit, "Kalista_Base_W_Glow.troy", unit, int.MaxValue,1,"HEAD");
		    AddParticleTarget(Owner, unit, "Kalista_Base_W_Glow2.troy", unit, int.MaxValue,1,"HEAD");
		    AddParticleTarget(Owner, unit, "Kalista_Base_W_Glow.troy", unit, int.MaxValue,1,"HEAD");
		    AddParticleTarget(Owner, unit, "Kalista_Base_W_ViewCone.troy", unit, int.MaxValue,1,"HEAD");

        }
        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
			RemoveBuff(thisBuff);
        }
        public void OnUpdate(float diff)
        {
			T += diff;
			//if (T >= 4000.0f && Unit != null) { Unit.SetWaypoints(GetPath(Unit.Position, P)); }

        }
    }
}