using System.Linq;
using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class NasusE : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
            IsDamagingSpell = false
        };

        private IObjAiBase Owner;
        private ISpell Spell;

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            Owner = owner;
            Spell = spell;
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			AddParticle(owner, null, "Nasus_Base_E_Warning.troy", end,5f,1);
			AddParticle(owner, null, "Nasus_Base_E_Staff_Swirl.troy", end,5f,1);
			AddParticle(owner, null, "Nasus_Base_E_Clynder_Scroll.troy", end,5f,1);
        }

        public void OnSpellCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;
        }

        public void OnSpellPostCast(ISpell spell)
        {
			var owner = spell.CastInfo.Owner;
            if (owner is IObjAiBase c)
            {
                var ownerSkinID = c.SkinID;
                var targetPos = new Vector2(spell.CastInfo.TargetPosition.X, spell.CastInfo.TargetPosition.Z);
                var ownerPos = c.Position;
                var distance = Vector2.Distance(ownerPos, targetPos);
                FaceDirection(targetPos, c);
                if (distance > 1200.0)
                {
                    targetPos = GetPointFromUnit(c, 1150.0f);
                }         				
                var units = GetUnitsInRange(targetPos, 270f, true);
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].Team != c.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {
					        var ELevel = c.GetSpell(2).CastInfo.SpellLevel;
							var damage = 55 + (40 * (ELevel - 1)) + (c.Stats.AbilityPower.Total * 0.6f);
                            units[i].TakeDamage(c, damage, DamageType.DAMAGE_TYPE_MAGICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
                    }
                }
                IMinion NA = AddMinion(c, "TestCube", "TestCube", targetPos, c.Team, c.SkinID, true, false);
			    AddBuff("NasusE", 5f, 1, spell, NA, NA, false);				
            }
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
