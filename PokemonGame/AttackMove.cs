using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGame
{
    public class AttackMove
    {
        public string AttackName { get; set; }

        public int AttackDamage { get; set; }

        public bool IsPhysicalAttack { get; set; }

        public bool IsStatusMove { get; set; }

        public int Accuracy { get; set; }

        public Type Type { get; set; }

        public AttackMove(string attackName, int attackDamage, int accuracy, bool isPhysicalAttack, Type type, bool isStatusMove = false)
        {
            AttackName = attackName;
            AttackDamage = attackDamage;
            IsPhysicalAttack = isPhysicalAttack;
            Accuracy = accuracy;
            Type = type;
            IsStatusMove = isStatusMove;
        }

        public virtual void MoveEffect(ref Pokemon pokemon)
        {
        }
    }
}
