using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGame
{
    internal class StatusMove : AttackMove
    {
        public StatType Status;
        int StageEffect;

        public StatusMove(string attackName, int attackDamage, int accuracy, bool isPhysicalAttack, Type type, StatType statusEffect, int stageEffect, bool isStatusMove = false) : base(attackName, attackDamage, accuracy, isPhysicalAttack, type, isStatusMove)
        {
            Status = statusEffect;
            StageEffect = stageEffect;
        }

        void PrintStageLowered(Pokemon pokemon)
        {

        }

        public void PrintDelay(string phrase)
        {
            for (int i = 0; i < phrase.Length; i++)
            {
                Console.Write(phrase[i]);
                Thread.Sleep(30);
            }
            Console.WriteLine();
        }


        public override void MoveEffect(ref Pokemon pokemon)
        {
            switch (Status)
            {
                case StatType.Attack:
                    pokemon.AttackStage = Math.Clamp(pokemon.AttackStage + StageEffect, -6, 6);
                    break;
                case StatType.Defense:
                    pokemon.DefenseStage = Math.Clamp(pokemon.DefenseStage + StageEffect, -6, 6);
                    PrintDelay(pokemon.Name + "'s Defense fell!");
                    break;
                case StatType.SpecialAttack:
                    pokemon.SpecialAttackStage = Math.Clamp(pokemon.SpecialAttackStage + StageEffect, -6, 6);
                    break;
                case StatType.SpecialDefense:
                    pokemon.SpecialDefenseStage = Math.Clamp(pokemon.SpecialDefenseStage + StageEffect, -6, 6);
                    break;
                case StatType.Speed:
                    pokemon.SpeedStage = Math.Clamp(pokemon.SpeedStage + StageEffect, -6, 6);
                    break;
            }
        }

    }
}
