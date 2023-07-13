using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGame
{
    public class SunnyDay : AttackMove
    {
        Battle CurrentBattle { get; set; }

        public SunnyDay(string attackName, int attackDamage, int accuracy, bool isPhysicalAttack, Type type, Battle currentBattle, bool isStatusMove = false) : base(attackName, attackDamage, accuracy, isPhysicalAttack, type, isStatusMove)
        {
            CurrentBattle = currentBattle;
        }
        public override void MoveEffect()
        {
            CurrentBattle.ActivateSunnyDay();
            CurrentBattle.PrintDelay("The sunlight turned harsh!");
        }
    }
}
