using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;

namespace Spells
{
    public class BlindMonkRKick : ISpellScript
    {
        IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            Target = target;
        }

        public void OnSpellCast(ISpell spell)
        {
        }
        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
			var ad = spell.CastInfo.Owner.Stats.AttackDamage.Total * 2f;
			var spellLevel = owner.GetSpell("BlindMonkRKick").CastInfo.SpellLevel;
            var damage = ad + ((spellLevel-1)*200)+200;
			var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
			var R = 600 ;
			var distt = dist + R;
			var targetPos = GetPointFromUnit(owner,distt);
            FaceDirection(Target.Position, owner);
            Target.TakeDamage(owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
			ForceMovement(Target, "RUN", targetPos, 1400, 0, 15, 0);
			AddParticleTarget(owner, Target, "blindMonk_R_self_mis", Target, bone: "C_BuffBone_Glb_Center_Loc");
            AddParticleTarget(owner, Target, "blind_monk_ult_unit_impact", Target, bone: "C_BuffBone_Glb_Center_Loc");
			AddParticleTarget(owner, Target, "blind_monk_R_kick_cas", Target, bone: "C_BuffBone_Glb_Center_Loc");
            AddParticleTarget(owner, Target, "blind_monk_ult_impact", Target, bone: "C_BuffBone_Glb_Center_Loc");
            //AddBuff("Stun", 1.0f, 1, spell, Target, owner);       
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
