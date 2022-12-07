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
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class YasuoWMovingWall : ISpellScript
    {
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
			FaceDirection(end, owner,true);
        }

        public void OnSpellCast(ISpell spell)
        {       
        }

        public void OnSpellPostCast(ISpell spell)
        {
             var owner = spell.CastInfo.Owner as IChampion;              
			 var end = GetPointFromUnit(owner, 1200f);
             AddParticle(owner, null, "Yasuo_Base_W_windwall1.troy", end,4);   
             AddParticle(owner, null, "Yasuo_Base_W_windwall2.troy", end,4);
             AddParticle(owner, null, "Yasuo_Base_W_windwall3.troy", end,4);   
             AddParticle(owner, null, "Yasuo_Base_W_windwall4.troy", end,4);
             AddParticle(owner, null, "Yasuo_Base_W_windwall5.troy", end,4); 			 
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
