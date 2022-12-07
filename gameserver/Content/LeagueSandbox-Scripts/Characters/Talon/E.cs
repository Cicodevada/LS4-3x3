using System.Collections.Generic;
using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;

namespace Spells
{
    public class TalonCutthroat : ISpellScript
    {
        IAttackableUnit Target;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = true
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
			string particles;
			string particles2;
			string particles3;
			switch ((owner as IObjAiBase).SkinID)
            {          
				case 5:
                    particles = "Talon_Skin05_E_Cas.troy";
					particles2 = "Talon_Skin05_E_cas_trail.troy";
					particles3 = "Talon_Skin05_E_cas_trail_long.troy";
                    break;

                default:
                    particles = "talon_E_cast.troy";
					particles2 = ".troy";
					particles3 = ".troy";
                    break;
            }	
			AddParticle(owner, null, particles, owner.Position, lifetime: 10f,0.5f);
			AddParticle(owner, null, particles2, owner.Position, lifetime: 1f);
			AddParticle(owner, null, particles3, owner.Position, lifetime: 1f);
        }

        public void OnSpellCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
        }

        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
			var dist = System.Math.Abs(Vector2.Distance(Target.Position, owner.Position));
			var distt = dist + 125;
			var targetPos = GetPointFromUnit(owner,distt);      
            TeleportTo(owner, targetPos.X, targetPos.Y);
            AddBuff("TalonESlow", 0.25f, 1, spell, Target, owner);
			AddBuff("TalonDamageAmp", 3f, 1, spell, Target, owner);
            AddParticleTarget(owner, Target, "talon_E_tar.troy", Target, 10f);
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
