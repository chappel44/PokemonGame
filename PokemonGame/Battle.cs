using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGame
{
    public class Battle
    {
        bool isRaining = false;
        int turnsRained = 5;

        bool isSunny = false;
        int turnsSunny = 5;

        int textTypeSpeed = 50; //orig 50

        int DisplayAttacks(AttackMove[] pokemonAttacks, Trainer trainer)
        {
            int x = 0;
            int y = 11;
            for (int i = 0; i < pokemonAttacks.Count(); i++)
            {
                Console.SetCursorPosition(x, y);

                Console.Write(i + 1 + ") ");

                if (pokemonAttacks[i] != null)
                {
                    Console.WriteLine(pokemonAttacks[i].AttackName);
                    x += pokemonAttacks[i].AttackName.Length + 5;
                }
                else
                {
                    Console.WriteLine("------");
                    x += 6 + 5;
                }

                if (i == 1)
                {
                    x = 0;
                    y += 1;
                }
            }

            Console.WriteLine(trainer.Name + ", which move would you like to select? 1 - 4.");
            int choice = 0;
            bool correctInput = false;

            while (!correctInput)
            {
                try
                {
                    choice = int.Parse(Console.ReadLine());
                }
                catch
                {
                }

                if (choice >= 1 && choice <= 4)
                    correctInput = true;
                else
                    Console.WriteLine("You must enter 1-4");
            }

            return choice - 1;
        }

        int CalculateDamage(Pokemon poke1, Pokemon poke2, AttackMove attack)
        {
            attack.MoveEffect();

            if (attack.IsStatusMove)
                return 0;

            float damage = 0;

            float targets = 1f;

            float weather = 1f;

            if (isSunny)
            {
                if (attack.Type.Name == "fire")
                    weather = 1.5f;
                if (attack.Type.Name == "water")
                    weather = .5f;
            }

            if (isRaining)
            {
                if (attack.Type.Name == "water")
                    weather = 1.5f;
                if (attack.Type.Name == "fire")
                    weather = .5f;
            }

            float critical = 1f;

            float random = 1f;

            float stab = 1f;

            if (poke1.Type.Name == attack.Type.Name) //Applies the stab bonus
                stab = 1.5f;

            float burn = 1f;

            if (poke1.IsBurned && attack.IsPhysicalAttack) //Reduces the physical attack when burned 
                burn = .5f;

            float typeEffectiveness = FindEffectiveness(attack, poke2);

            float other = 1f;

            if (attack.IsPhysicalAttack)
            {
                damage = (((2 * poke1.Level / 5 + 2) * attack.AttackDamage * poke1.Attack / poke2.Defense) / 50 + 2) * targets * weather * critical * random * stab * typeEffectiveness * burn * other;
            }
            else
            {
                damage = (((2 * poke1.Level / 5 + 2) * attack.AttackDamage * poke1.SpecialAttack / poke2.SpecialDefense) / 50 + 2) * targets * weather * critical * random * stab * typeEffectiveness * burn * other;
            }

            if (typeEffectiveness > 1)
            {
                PrintDelay("It's super effective!");
            }
            else if (typeEffectiveness < 1)
            {
                PrintDelay("It's not very effective.");
            }

            return (int)damage;
        }

        float FindEffectiveness(AttackMove move, Pokemon poke2)
        {
            float typeEffectiveness = 1f;

            for (int i = 0; i < move.Type.TypeResistance.Length; i++)
            {
                if (poke2.Type.Name == move.Type.TypeResistance[i])
                {
                    typeEffectiveness = .5f;
                }
            }

            for (int i = 0; i < move.Type.StrongAgainst.Length; i++)
            {
                if (poke2.Type.Name == move.Type.StrongAgainst[i])
                {
                    typeEffectiveness = 2f;
                }
            }

            if (poke2.Type.ImmuneType != null)
            {
                if (poke2.Type.ImmuneType[0] == move.Type.Name)
                {
                    typeEffectiveness = 0f;
                }
            }

            return typeEffectiveness;
        }

        void PrintHealthStatus(Pokemon poke)
        {
            ConsoleColor color = ConsoleColor.Green;

            if (poke.Health < poke.OriginalHealth / 3)
            {
                color = ConsoleColor.Red;
            }
            else if (poke.Health < poke.OriginalHealth / 2)
            {
                color = ConsoleColor.Yellow;
            }

            Console.ForegroundColor = color;

            Console.Write(poke.Health + "/" + poke.OriginalHealth);

            Console.ForegroundColor = ConsoleColor.White;
        }

        void DisplayNames(Pokemon poke1, Pokemon poke2, Trainer trainer1, Trainer trainer2)
        {
            Console.Write(trainer1.Name + ": " + poke1.Name + "   HP: ");
            PrintHealthStatus(poke1);

            Console.SetCursorPosition(0, 10);
            Console.Write(trainer2.Name + ": " + poke2.Name + "   HP: ");
            PrintHealthStatus(poke2);
        }

        Pokemon GetPokemonChoice(Trainer trainer)
        {
            Pokemon poke1;

            int x = 0;
            int y = 0;

            for (int i = 0; i < trainer.Team.Count; i++)
            {
                Console.WriteLine(i + 1 + ") " + trainer.Team[i].Name);
            }

            int choice = 0;
            bool correctInput = false;

            while(!correctInput)
            {
                Console.WriteLine(trainer.Name + ", Which pokemon would you like to select?");

                choice = int.Parse(Console.ReadLine());

                if (choice > 0 && choice <= trainer.Team.Count)
                    correctInput = true;

            }

            Console.Clear();
            return trainer.Team[choice - 1];
        }

        public void PrintDelay(string phrase)
        {
            for (int i = 0; i < phrase.Length; i++)
            {
                Console.Write(phrase[i]);
                Thread.Sleep(textTypeSpeed);
            }
            Console.WriteLine();
        }

        void DisplayCurrentWeather()
        {
            if (isRaining)
            {
                
                turnsRained--;

                if (turnsRained <= 0)
                {
                    isRaining = false;
                    PrintDelay("The rain stopped.");
                }
                else
                    PrintDelay("Rain continues to fall.");
            }
            if (isSunny)
            {
                turnsSunny--;

                if (turnsSunny <= 0)
                {
                    isSunny = false;
                    PrintDelay("The sunlight faded.");
                }
                else
                    PrintDelay("The sunlight is strong.");
            }
        }

        public void Start(/*Pokemon poke1, Pokemon poke2, */Trainer trainer1, Trainer trainer2)
        {
            Pokemon poke1 = GetPokemonChoice(trainer1);

            Pokemon poke2 = GetPokemonChoice(trainer2);


            while (trainer1.AlivePokemon > 0 && trainer2.AlivePokemon > 0)
            {
                float effect = 0;

                DisplayNames(poke2, poke1, trainer2, trainer1);
                int choice1 = DisplayAttacks(poke1.MoveSet, trainer1);

                Console.ReadKey();
                Console.Clear();

                DisplayNames(poke1, poke2, trainer1, trainer2);
                int choice2 = DisplayAttacks(poke2.MoveSet, trainer2);

                if (poke1.Speed > poke2.Speed) 
                {
                    PrintDelay(poke1.Name + " used " + poke1.MoveSet[choice1].AttackName + ".");
                    int damage1 = CalculateDamage(poke1, poke2, poke1.MoveSet[choice1]);

                    if (damage1 > 0)
                        PrintDelay("It dealt " + damage1 + " damage.");

                    poke2.Health -= damage1;

                    if (poke2.Health <= 0)
                    {
                        break;
                    }

                    Console.WriteLine();
                    Console.ReadKey();

                    PrintDelay(poke2.Name + " used " + poke2.MoveSet[choice2].AttackName + ".");
                    int damage2 = CalculateDamage(poke2, poke1, poke2.MoveSet[choice2]);

                    if (damage2 > 0)
                        PrintDelay("It dealt " + damage2 + " damage.");

                    poke1.Health -= damage2;
                }
                else
                {
                    PrintDelay(poke2.Name + " used " + poke2.MoveSet[choice2].AttackName + ".");
                    int damage2 = CalculateDamage(poke2, poke1, poke2.MoveSet[choice2]);

                    if (damage2 > 0)
                        PrintDelay("It dealt " + damage2 + " damage.");

                    poke1.Health -= damage2;

                    if (poke1.Health <= 0)
                    {
                        break;
                    }

                    Console.WriteLine();
                    Console.ReadKey();

                    PrintDelay(poke1.Name + " used " + poke1.MoveSet[choice1].AttackName + ".");
                    int damage1 = CalculateDamage(poke1, poke2, poke1.MoveSet[choice1]);

                    if (damage1 > 0)
                        PrintDelay("It dealt " + damage1 + " damage.");

                    poke2.Health -= damage1;
                }

                DisplayCurrentWeather();

                Console.ReadKey();
                Console.Clear();

                if (poke1.Health <= 0)
                {
                    //PrintDelay(poke1.Name + " fainted...");
                    poke1 = GetPokemonChoice(trainer1);
                    trainer1.AlivePokemon--;
                }

                if (poke2.Health <= 0)
                {
                    //PrintDelay(poke2.Name + " fainted...");
                    poke2 = GetPokemonChoice(trainer2);
                    trainer2.AlivePokemon--;
                }
            }

            //if (poke1.Health <= 0)
            //{
            //    //PrintDelay(poke1.Name + " fainted...");
            //    poke1 = GetPokemonChoice(trainer1);
            //}

            //if (poke2.Health <= 0)
            //{
            //    //PrintDelay(poke2.Name + " fainted...");
            //    poke2 = GetPokemonChoice(trainer2);
            //}
        }

        public void ActivateSunnyDay()
        {
            isSunny = true;
            turnsSunny = 5;
            isRaining = false;
        }

        public void ActivateRainyDay()
        {
            isRaining = true;
            turnsRained = 5;
            isSunny = false;
        }
    }
}
