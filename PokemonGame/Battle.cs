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

        int textTypeSpeed = 30; //orig 500

        Dictionary<string, ConsoleColor> color = new Dictionary<string, ConsoleColor>();

        void CreateTypeColor()
        {
            color.Add("water", ConsoleColor.DarkBlue);
            color.Add("fire", ConsoleColor.Red);
            color.Add("grass", ConsoleColor.Green);
            color.Add("normal", ConsoleColor.Gray);
            color.Add("ice", ConsoleColor.Blue);
        }

        int DisplayAttacks(AttackMove[] pokemonAttacks, Trainer trainer)
        {
            int x = 0;
            int y = 12;//11
            for (int i = 0; i < pokemonAttacks.Count(); i++)
            {
                Console.SetCursorPosition(x, y);

                Console.Write(i + 1 + ") ");

                if (pokemonAttacks[i] != null)
                {
                    Console.ForegroundColor = color[pokemonAttacks[i].Type.Name]; 
                    Console.WriteLine(pokemonAttacks[i].AttackName);
                    x += pokemonAttacks[i].AttackName.Length + 8;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.WriteLine("------");
                    x += 6 + 5;
                }

                if (i == 1)
                {
                    x = 0;
                    y += 2;
                }
                Console.WriteLine();
            }

            x += 6;
            Console.SetCursorPosition(x, y);
            Console.WriteLine("5) Pokémon");

            x = 0;
            y += 2;
            Console.SetCursorPosition(x, y);

            Console.WriteLine(trainer.Name + ", which move would you like to select? 1 - " + pokemonAttacks.Count());
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

                if ((choice >= 1 && choice <= pokemonAttacks.Count() && pokemonAttacks[choice-1] != null) || choice == 5)
                {
                    correctInput = true;
                }
                else
                    Console.WriteLine("Invalid choice");
            }

            return choice - 1;
        }

        int CalculateDamage(Pokemon poke1, Pokemon poke2, AttackMove attack)
        {
            attack.MoveEffect(ref poke2);

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
                float attackStat = poke1.Attack * GetStageMultiplier(poke1.AttackStage);
                float defenseStat = poke2.Defense * GetStageMultiplier(poke2.DefenseStage);
                damage = (((2 * poke1.Level / 5 + 2) * attack.AttackDamage * attackStat / defenseStat) / 50 + 2) * targets * weather * critical * random * stab * typeEffectiveness * burn * other;
            }
            else
            {
                float specialAttackStat = poke1.SpecialAttack * GetStageMultiplier(poke1.SpecialAttackStage);
                float specialDefenseStat = poke2.SpecialDefense * GetStageMultiplier(poke2.SpecialDefenseStage);
                damage = (((2 * poke1.Level / 5 + 2) * attack.AttackDamage * specialAttackStat / specialDefenseStat) / 50 + 2) * targets * weather * critical * random * stab * typeEffectiveness * burn * other;
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

        float GetStageMultiplier(int stage)
        {
            return stage >= 0
                ? (2.0f + stage) / 2.0f
                : 2.0f / (2.0f - stage);
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
                Console.WriteLine();
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

        public void Start(Trainer trainer1, Trainer trainer2)
        {
            Pokemon poke1 = GetPokemonChoice(trainer1);

            Pokemon poke2 = GetPokemonChoice(trainer2);

            CreateTypeColor();

            while (trainer1.AlivePokemon > 0 && trainer2.AlivePokemon > 0)
            {
                Pokemon tempPoke1 = poke1;
                Pokemon tempPoke2 = poke2;

                Console.WriteLine(poke2.DefenseStage);

                DisplayNames(poke2, poke1, trainer2, trainer1);
                int choice1 = DisplayAttacks(poke1.MoveSet, trainer1);


                if (choice1 == 4) //Trainer one chose to switch a pokemon out
                {
                    poke1.ClearStatus();
                    tempPoke1 = poke1;
                    poke1 = GetPokemonChoice(trainer1);
                }

                Console.Clear();

                DisplayNames(tempPoke1, poke2, trainer1, trainer2);
                int choice2 = DisplayAttacks(poke2.MoveSet, trainer2);

                if (choice2 == 4) //Trainer two chose to switch a pokemon out
                {
                    poke2.ClearStatus();
                    tempPoke2 = poke2;
                    poke2 = GetPokemonChoice(trainer2);
                }

                Console.Clear();
                DisplayNames(poke2, poke1, trainer2, trainer1);
                Console.WriteLine();
                Console.WriteLine();

                if (poke1.Speed > poke2.Speed) //Trainer 1s pokemon is faster
                {
                    InitiateAttacks(ref trainer1, ref trainer2, ref poke1, ref poke2, choice1, choice2, tempPoke1, tempPoke2);
                }
                else if(poke2.Speed > poke1.Speed) //trainer 2s pokemon is faster
                {
                    InitiateAttacks(ref trainer2, ref trainer1, ref poke2, ref poke1, choice2, choice1, tempPoke2, tempPoke1);
                }
                else //Speed tie choose a random pokemon to go first
                {
                    Random ran = new Random();
                    int num = ran.Next(1, 3);
                    if(num == 1) //Trainer 1 goes first
                    {
                        InitiateAttacks(ref trainer1, ref trainer2, ref poke1, ref poke2, choice1, choice2, tempPoke1, tempPoke2);
                    }
                    else //Trainer 2 goes first
                    {
                        InitiateAttacks(ref trainer2, ref trainer1, ref poke2, ref poke1, choice2, choice1, tempPoke2, tempPoke1);
                    }
                }

                if (poke1.Health <= 0)
                {
                    RemovePokemon(ref trainer1, ref poke1);
                    continue;
                }

                if (poke2.Health <= 0)
                {
                    RemovePokemon(ref trainer2, ref poke2);
                    continue;
                }

                DisplayCurrentWeather();

                Console.ReadKey();
                Console.Clear();
            }
        }

        void InitiateAttacks(ref Trainer firstTrainer, ref Trainer secondTrainer, ref Pokemon firstAttacker, ref Pokemon secondAttacker, int attackerOneChoice, int attackerTwoChoice, Pokemon tempPoke1, Pokemon tempPoke2)
        {
            if (attackerOneChoice == 4)
            {
                PrintDelay(firstTrainer.Name + " Withdrew " + tempPoke1.Name + " and sent out " + firstAttacker.Name);
            }
            if(attackerTwoChoice == 4)
            {
                PrintDelay(secondTrainer.Name + " Withdrew " + tempPoke2.Name + " and sent out " + secondAttacker.Name);
            }

            if (attackerOneChoice != 4)
            {
                PrintDelay(firstAttacker.Name + " used " + firstAttacker.MoveSet[attackerOneChoice].AttackName + ".");
                int damage1 = CalculateDamage(firstAttacker, secondAttacker, firstAttacker.MoveSet[attackerOneChoice]);

                if (damage1 > 0)
                    PrintDelay("It dealt " + damage1 + " damage.");

                secondAttacker.Health -= damage1;
            }
            if (secondAttacker.Health <= 0)
            {
                RemovePokemon(ref secondTrainer, ref secondAttacker);
                return;
            }

            Console.WriteLine();
            Console.ReadKey();

            if (attackerTwoChoice != 4)
            {
                PrintDelay(secondAttacker.Name + " used " + secondAttacker.MoveSet[attackerTwoChoice].AttackName + ".");
                int damage2 = CalculateDamage(secondAttacker, firstAttacker, secondAttacker.MoveSet[attackerTwoChoice]);

                if (damage2 > 0)
                    PrintDelay("It dealt " + damage2 + " damage.");

                firstAttacker.Health -= damage2;
            }
        }


        void RemovePokemon(ref Trainer trainer, ref Pokemon pokemon)
        {
            PrintDelay(pokemon.Name + " has fainted!");
            trainer.Team.Remove(pokemon);
            Console.ReadKey();
            Console.Clear();
            trainer.AlivePokemon--;
            if (trainer.AlivePokemon > 0)
            {
                pokemon = GetPokemonChoice(trainer);
            }
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
