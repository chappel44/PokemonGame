using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGame
{
    public class Pokemon
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public Type Type2 { get; set; }

        public int Level { get; set; }
        public int Health { get; set; }
        public int OriginalHealth { get; set; }
        
        public int Attack { get; set; }
        public int Defense { get; set; }
        
        public int SpecialAttack { get; set; }
        public int SpecialDefense { get; set; }
        
        public int Speed { get; set; }

        public int AttackStage;
        public int SpecialAttackStage;
        public int DefenseStage;
        public int SpecialDefenseStage;
        public int SpeedStage;


        public bool IsBurned { get; set; } = false;

        public AttackMove[] MoveSet { get; set; }

        public Pokemon(string name, Type type, int health, int level, int attack, int defense, int specialAttack, int specialDefense, int speed, AttackMove[] moveSet, Type type2 = null)
        {
            Name = name;
            Type = type;
            Level = level;
            Health = health;
            OriginalHealth = health;
            Attack = attack;
            Defense = defense;
            SpecialAttack = specialAttack;
            SpecialDefense = specialDefense;
            Speed = speed;
            MoveSet = moveSet;
            Type2 = type2;
            AttackStage = 0;
            SpecialAttackStage = 0;
            DefenseStage = 0 ;
            SpecialDefenseStage = 0;
            SpeedStage = 0;
        }

        public void ClearStatus()
        {
            AttackStage = 0;
            SpecialAttackStage = 0;
            DefenseStage = 0;
            SpecialDefenseStage = 0;
            SpeedStage = 0;
        }

        public Pokemon Clone()
        {
            return new Pokemon(Name, Type, Health, Level, Attack, Defense, SpecialAttack, SpecialDefense, Speed, MoveSet, Type2);
        }
    }
}
