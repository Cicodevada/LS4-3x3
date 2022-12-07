using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
namespace Spells
{
    public class RivenTriCleave : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata => new SpellScriptMetadata()
        {
            TriggersSpellCasts = true
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {          
        }     
        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void DashFin(IAttackableUnit unit)
        {
			string particles;
			string particles2;
			string particles3;
            string particles4;
            string particles5;
			string particles6;
            string particles7;
			string particles8;
            string particles9;
            string particles10;
			string particles11;			
			if (unit is IObjAiBase _owner)
            {
			switch ((_owner as IObjAiBase).SkinID)
            {
                case 3:
                    particles = "exile_bunny_Q_01_detonate.troy";
					particles2 = "exile_bunny_Q_02_detonate.troy";
					particles3 = "exile_bunny_Q_03_detonate.troy";
					particles4 = "RivenQ_tar.troy";
					particles5 = "exile_Q_tar_01.troy";
					particles6 = "exile_Q_tar_02.troy";
					particles7 = "exile_Q_tar_03.troy";
					particles8 = "exile_Q_tar_04.troy";
					particles9 = "exile_bunny_Q_01_detonate_ult.troy";
					particles10 = "exile_bunny_Q_02_detonate.troy";
					particles11 = "exile_bunny_Q_03_detonate.troy";
                    break;

                case 4:
                    particles = "Riven_S2_Q_01_detonate.troy";
					particles2 = "Riven_S2_Q_02_detonate.troy";
					particles3 = "Riven_S2_Q_03_detonate.troy";
					particles4 = "RivenQ_tar.troy";
					particles5 = "Riven_S2_Q_tar_01.troy";
					particles6 = "Riven_S2_Q_tar_02.troy";
					particles7 = "Riven_S2_Q_tar_03.troy";
					particles8 = "Riven_S2_Q_tar_04.troy";
					particles9 = "Riven_S2_Q_01_detonate_ult.troy";
					particles10 = "Riven_S2_Q_02_detonate_ult.troy";
					particles11 = "Riven_S2_Q_03_detonate_ult.troy";
                    break;
				case 5:
                    particles = "Riven_Skin05_Q_01_Detonate.troy";
					particles2 = "Riven_Skin05_Q_02_Detonate.troy";
					particles3 = "Riven_Skin05_Q_03_Detonate.troy";
					particles4 = "RivenQ_tar.troy";
					particles5 = "Riven_Skin05_Q_Tar_01.troy";
					particles6 = "Riven_Skin05_Q_Tar_01.troy";
					particles7 = "Riven_Skin05_Q_Tar_01.troy";
					particles8 = "Riven_Skin05_Q_Tar_01.troy";
					particles9 = "Riven_Skin05_Q_01_Detonate_Ult.troy";
					particles10 = "Riven_Skin05_Q_02_Detonate_Ult.troy";
					particles11 = "Riven_Skin05_Q_03_Detonate_Ult.troy";
                    break;
				case 6:
                    particles = "Riven_Skin06_Q_01_Detonate.troy";
					particles2 = "Riven_Skin06_Q_02_Detonate.troy";
					particles3 = "Riven_Skin06_Q_03_Detonate.troy";
					particles4 = "RivenQ_tar.troy";
					particles5 = "Riven_Skin06_Q_Tar_01.troy";
					particles6 = "Riven_Skin06_Q_Tar_02.troy";
					particles7 = "Riven_Skin05_Q_Tar_03.troy";
					particles8 = "Riven_Skin05_Q_Tar_04.troy";
					particles9 = "Riven_Skin06_Q_01_Detonate.troy";
					particles10 = "Riven_Skin06_Q_02_detonate_ult.troy";
					particles11 = "Riven_Skin06_Q_03_detonate_ult.troy";
                    break;
				case 16:
                    particles = "Riven_Skin16_Q_01_Detonate.troy";
					particles2 = "Riven_Skin16_Q_02_Detonate.troy";
					particles3 = "Riven_Skin16_Q_03_Detonate.troy";
					particles4 = "RivenQ_tar.troy";
					particles5 = "Riven_Skin16_Q_Tar_01.troy";
					particles6 = "Riven_Skin16_Q_Tar_02.troy";
					particles7 = "Riven_Skin16_Q_Tar_03.troy";
					particles8 = "Riven_Skin16_Q_Tar_04.troy";
					particles9 = "Riven_Skin16_Q_01_Detonate.troy";
					particles10 = "Riven_Skin16_Q_02_detonate_ult.troy";
					particles11 = "Riven_Skin16_Q_03_detonate_ult.troy";
                    break;

                default:
                    particles = "exile_Q_01_detonate.troy";
					particles2 = "exile_Q_02_detonate.troy";
					particles3 = "exile_Q_03_detonate.troy";
					particles4 = "RivenQ_tar.troy";
					particles5 = "exile_Q_tar_01.troy";
					particles6 = "exile_Q_tar_02.troy";
					particles7 = "exile_Q_tar_03.troy";
					particles8 = "exile_Q_tar_04.troy";
					particles9 = "exile_Q_01_detonate_ult.troy";
					particles10 = "exile_Q_02_detonate_ult.troy";
					particles11 = "exile_Q_03_detonate_ult.troy";
                    break;
            }	
			var QLevel = _owner.GetSpell(0).CastInfo.SpellLevel;
			var damage = 10 + (20 * (QLevel - 1)) + (_owner.Stats.AttackDamage.Total * 0.6f);
			if(dash == 1)
            {
				_owner.SkipNextAutoAttack();
				if (_owner.HasBuff("RivenFengShuiEngine"))
                {
				     AddParticle(_owner, null, particles9, GetPointFromUnit(_owner, 125f));
			    }
			    else
			    {
                     AddParticle(_owner, null, particles, GetPointFromUnit(_owner, 125f));
			    }
				var units = GetUnitsInRange(GetPointFromUnit(_owner, 80f), 260f, true);
                for (int i = 0; i < units.Count; i++)
                {
                if (units[i].Team != _owner.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {					     
                         units[i].TakeDamage(_owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
						 AddParticleTarget(_owner, units[i], particles4, units[i], 10f,1,"");
				         AddParticleTarget(_owner, units[i], particles5, units[i], 10f,1,"");
						 AddParticleTarget(_owner, units[i], particles8, units[i], 10f,1,"");
                    }	
                }
            }
			if(dash == 2)
            {
				_owner.SkipNextAutoAttack();
                if (_owner.HasBuff("RivenFengShuiEngine"))
                {
				     AddParticle(_owner, null, particles10, GetPointFromUnit(_owner, 125f));
			    }
			    else
			    {
                     AddParticle(_owner, null, particles2, GetPointFromUnit(_owner, 125f));
			    }
				var units = GetUnitsInRange(GetPointFromUnit(_owner, 80f), 260f, true);
                for (int i = 0; i < units.Count; i++)
                {
                if (units[i].Team != _owner.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {					     
                         units[i].TakeDamage(_owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
						 AddParticleTarget(_owner, units[i], particles4, units[i], 10f,1,"");
				         AddParticleTarget(_owner, units[i], particles6, units[i], 10f,1,"");
						 AddParticleTarget(_owner, units[i], particles8, units[i], 10f,1,"");
                    }	
                }
            }
            if(dash == 3)
            {
				_owner.SkipNextAutoAttack();
				if (_owner.HasBuff("RivenFengShuiEngine"))
                {
				     AddParticle(_owner, null, particles11, GetPointFromUnit(_owner, 125f));
			    }
			    else
			    {
                     AddParticle(_owner, null, particles3, GetPointFromUnit(_owner, 125f));
			    }
                var units = GetUnitsInRange(GetPointFromUnit(_owner, 80f), 300f, true);
                for (int i = 0; i < units.Count; i++)
                {
                if (units[i].Team != _owner.Team && !(units[i] is IObjBuilding || units[i] is IBaseTurret))
                    {					     
                         units[i].TakeDamage(_owner, damage, DamageType.DAMAGE_TYPE_PHYSICAL, DamageSource.DAMAGE_SOURCE_SPELL, false);
						 ForceMovement(units[i], "RUN", new Vector2(units[i].Position.X + 10f, units[i].Position.Y + 10f), 20f, 0, 5.5f, 0);
						 AddBuff("Pulverize", 0.75f, 1, _spell, units[i], _owner);
						 AddParticleTarget(_owner, units[i], particles4, units[i], 10f,1,"");
				         AddParticleTarget(_owner, units[i], particles7, units[i], 10f,1,"");
						 AddParticleTarget(_owner, units[i], particles8, units[i], 10f,1,"");
                    }	
                }				
            }
			}
        }
        ISpellSector DamageSector;
        int q = 0;
        IObjAiBase _owner;
        ISpell _spell;
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            _owner = owner;
            _spell = spell;
			owner.SetTargetUnit(null, true);
			owner.CancelAutoAttack(false, false);
            var x = GetChampionsInRange(end, 200, true);
            foreach(var champ in x)
            {
                if(champ.Team != owner.Team)
                {
                    FaceDirection(champ.Position, owner);
                }
            }
        }

        public void OnSpellCast(ISpell spell)
        {
        }
        int dash = 0;
        public void OnSpellPostCast(ISpell spell)
        {
            var owner = spell.CastInfo.Owner;		
            AddBuff("RivenTriCleave", 4.0f, 1, spell, owner, owner as IObjAiBase);
			ApiEventManager.OnMoveEnd.AddListener(this, owner, DashFin, true);
            var getbuff = owner.GetBuffWithName("RivenTriCleave");
            switch (getbuff.StackCount)
            {
                case 1:
                    dash = 1;
                    PlayAnimation(owner, "Spell1A", 0.75f);
                    ForceMovement(owner, "Spell1A", GetPointFromUnit(owner, 225), 700, 0, 0, 0);
					if (owner.HasBuff("RivenFengShuiEngine"))
                    {
				       AddParticle(owner, owner, "Riven_Base_Q_01_Wpn_Trail_Ult.troy", owner.Position, bone: "chest"); 
			        }
			        else
			        {
                       AddParticle(owner, owner, "Riven_Base_Q_01_Wpn_Trail.troy", owner.Position, bone: "chest"); 
			        }                              
                    return;
                case 2:
                    dash = 2;
                    PlayAnimation(owner, "Spell1B", 0.75f);
                    ForceMovement(owner, "Spell1B", GetPointFromUnit(owner, 225), 700, 0, 0, 0);
                    if (owner.HasBuff("RivenFengShuiEngine"))
                    {
				       AddParticle(owner, owner, "Riven_Base_Q_02_Wpn_Trail_Ult.troy", owner.Position, bone: "chest"); 
			        }
			        else
			        {
                       AddParticle(owner, owner, "Riven_Base_Q_02_Wpn_Trail.troy", owner.Position, bone: "chest"); 
			        }            
                    return;
                case 3:
                    dash = 3;
                    PlayAnimation(owner, "Spell1C", 0.75f);
                    ForceMovement(owner, "Spell1C", GetPointFromUnit(owner, 250), 700, 0, 50, 0);
					if (owner.HasBuff("RivenFengShuiEngine"))
                    {
				       AddParticle(owner, owner, "Riven_Base_Q_03_Wpn_Trail_Ult.troy", owner.Position, size: -1);
			        }
			        else
			        {
                       AddParticle(owner, owner, "Riven_Base_Q_03_Wpn_Trail.troy", owner.Position, size: -1);
			        }   
					getbuff.DeactivateBuff();				
                    return;
            }
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource source)
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