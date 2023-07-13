using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGame
{
    public class RainyDay : AttackMove
    {
        Battle CurrentBattle { get; set; }

        public RainyDay(string attackName, int attackDamage, int accuracy, bool isPhysicalAttack, Type type, Battle currentBattle, bool isStatusMove = false) : base(attackName, attackDamage, accuracy, isPhysicalAttack, type, isStatusMove)
        {
            CurrentBattle = currentBattle;
        }

        public override void MoveEffect()
        {
            CurrentBattle.ActivateRainyDay();
            CurrentBattle.PrintDelay("It started raining!");
        }
    }
}
