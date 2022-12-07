using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using LeagueSandbox.GameServer.Scripting.CSharp;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Scripting.CSharp;



namespace Buffs
{
    internal class KennenMarkOfStorm : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_DEHANCER,
            BuffAddType = BuffAddType.STACKS_AND_RENEWS,
            MaxStacks = 3
        };

        public IStatsModifier StatsModifier { get; private set; }

        IParticle mos;
        IParticle mos2;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            var owner = ownerSpell.CastInfo.Owner;

            switch (unit.GetBuffWithName("KennenMarkOfStorm").StackCount) //switch using enemy target mark of storm buff stack count
            {
                case 1:
                    mos = AddParticleTarget(owner, unit, "kennen_mos1.troy", unit, buff.Duration);
                    break;
                case 2:
                    RemoveParticle(mos); //remove mark of storm1 particle and replace with mos2 particle
                    mos2 = AddParticleTarget(owner, unit, "kennen_mos2.troy", unit, buff.Duration);
                    break;
                case 3:
                    RemoveParticle(mos2);
                    AddParticleTarget(owner, unit, "kennen_mos_tar.troy", unit, buff.Duration);

                    //adding stun here doesnt give unit stun buff for some reason, but particle effects still apply
                    if (unit.HasBuff("KennenMoSDiminish"))
                    {
                        AddBuff("Stun", 0.5f, 1, ownerSpell, unit, owner); //stun target for 0.5 seconds
                    }
                    else
                    {
                        AddBuff("Stun", 1f, 1, ownerSpell, unit, owner); //stun target for 1 second after 3 stacks
                    }
                    AddBuff("KennenMoSDiminish", 7f, 1, ownerSpell, unit, owner); //apply mos diminish buff
                    owner.Stats.CurrentMana += 25f; //kennen receives 25 energy upon 3 marks of storm
                    buff.DeactivateBuff();
                    break;
            }
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        { 
            RemoveParticle(mos); //remove all particles upon deactivation
            RemoveParticle(mos2);
        }

        public void OnPreAttack(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
