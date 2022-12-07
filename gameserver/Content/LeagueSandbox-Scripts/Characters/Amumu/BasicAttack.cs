using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using System.Collections.Generic;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Scripting.CSharp;
using System;

namespace Spells

{
    public class AmumuBasicAttack : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {

        };
        IObjAiBase daowner;
        ISpell daspell;
		IAttackableUnit Target;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			Target = target;
            daspell = spell;
            ApiEventManager.OnLaunchAttack.AddListener(this, daowner, OnLaunchAttack, false);
        }

        public void OnLaunchAttack(ISpell spell)
        {

            AddBuff("CursedTouch", 4f, 1, daspell, Target, spell.CastInfo.Owner);
        }

        private void SpellCast(IObjAiBase owner, int v1, SpellSlotType extraSlots, bool v2, IAttackableUnit target, Vector2 position)
        {
        }

        private void AddParticleTarget(IObjAiBase owner, IAttackableUnit target1, string v, IAttackableUnit target2, string bone)
        {
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
        }
    }
	public class AmumuBasicAttack2 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {

        };
        IObjAiBase daowner;
        ISpell daspell;
		IAttackableUnit Target;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
			Target = target;
            daspell = spell;
            ApiEventManager.OnLaunchAttack.AddListener(this, daowner, OnLaunchAttack, false);
        }

        public void OnLaunchAttack(ISpell spell)
        {

            AddBuff("CursedTouch", 4f, 1, daspell, Target, spell.CastInfo.Owner);
        }

        private void SpellCast(IObjAiBase owner, int v1, SpellSlotType extraSlots, bool v2, IAttackableUnit target, Vector2 position)
        {
        }

        private void AddParticleTarget(IObjAiBase owner, IAttackableUnit target1, string v, IAttackableUnit target2, string bone)
        {
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
        }
    }
}