using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain;

namespace Spells
{
    public class MoveQuick : ISpellScript
    {
        bool takeDamage = true;
        float timer = 0f;
        IAttackableUnit Unit;
        ISpell Spell;
        IObjAiBase Owner;
        bool OnFirstSpellLevelUp = false;
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            Spell = spell;
            Owner = owner;
            ApiEventManager.OnTakeDamage.AddListener(this, owner, TakeDamage, false);
        }
        public void TakeDamage(IDamageData damageData)
        {
            Unit = damageData.Target;
            if (Owner != null && !Owner.HasBuff("MoveQuick") && (damageData.Attacker is IChampion || damageData.Attacker is IBaseTurret || damageData.Attacker is ILaneTurret))
            {
                RemoveBuff(Owner, "MoveQuickPassive");
                takeDamage = true;
            }
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            AddBuff("MoveQuick", 3f, 1, spell, owner, owner);
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {

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

            if (takeDamage == true && Unit != null && Spell.CastInfo.SpellLevel >= 1)
            {
                timer += diff;
                if (timer >= 5000.0)
                {
                    AddBuff("MoveQuickPassive", 1, 1, Spell, Unit, Owner, true);
                    takeDamage = false;
                    timer = 0;
                }
            }

            //Checks if the skill already got Leveled up to 1, activating the buff instantly. (Ideally we probably would use listeners for this)
            if (Spell.CastInfo.SpellLevel == 1 && OnFirstSpellLevelUp == false)
            {
                AddBuff("MoveQuickPassive", 1, 1, Spell, Owner, Owner, true);
                OnFirstSpellLevelUp = true;
            }
        }
    }
}
