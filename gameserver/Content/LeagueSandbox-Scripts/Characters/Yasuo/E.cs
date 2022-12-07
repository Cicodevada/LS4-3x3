using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using GameServerLib.GameObjects.AttackableUnits;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.GameObjects.Stats;
using GameServerCore.Enums;

namespace Spells
{
    public class YasuoDashWrapper : ISpellScript
    {
		IObjAiBase Owner;
		IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
            // TODO
        };
        IParticle P;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			Owner = owner;
			Target = target; 
			owner.SetTargetUnit(null, true);
			owner.CancelAutoAttack(false, false);
			SetStatus(owner, StatusFlags.Ghosted, true);
            ApiEventManager.OnMoveEnd.AddListener(this, owner, OnMoveEnd, true);			
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
		   if (!Target.HasBuff("YasuoEBlockFIX"))
		   {
		    PlayAnimation(owner, "SPELL3", 0.34f);
            //var target = spell.CastInfo.Targets[0].Unit;
            var current = new Vector2(owner.Position.X, owner.Position.Y);
            var to = Vector2.Normalize(new Vector2(Target.Position.X, Target.Position.Y) - current);
            var range = to * 475;

            var trueCoords = current + range;
            P = AddParticleTarget(owner, owner, "Yasuo_Base_E_Dash.troy", owner);
            ForceMovement(owner, null, trueCoords, 1400, 0, 0, 0);
			var bonusAd = owner.Stats.AttackDamage.Total - owner.Stats.AttackDamage.BaseValue;
			var Ad = bonusAd * 0.2f;
            var damage = 50 + spell.CastInfo.SpellLevel * 10 + Ad;
            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_MAGICAL,DamageSource.DAMAGE_SOURCE_SPELL, false);
            AddBuff("YasuoE", 0.5f, 1, spell, owner, owner);
			//AddBuff("YasuoEBlockFIX", 3f, 1, spell, Target, owner);
            AddParticleTarget(owner, Target, "Yasuo_Base_E_dash_hit.troy", Target, 10f,1,"CHEST");
		   }		 	
        }
		public void OnMoveEnd(IAttackableUnit owner)
        {
			//Owner.SetTargetUnit(Target, true);
			SetStatus(Owner, StatusFlags.Ghosted, false);         
			RemoveParticle(P);		
        }
        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
