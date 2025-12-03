using System;
using System.ComponentModel;
using System.Reflection.Emit;

namespace PokemonGame
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Battle battle = new Battle();

            //          TYPE DATA               //

            //Creates the fire type
            string[] fireStrengths = { "bug", "steel", "grass", "ice" };
            string[] fireResist = { "dragon", "rock", "fire", "water" };
            Type fire = new Type("fire", fireStrengths, fireResist);

            //Creates the water type
            string[] waterStrengths = { "ground", "rock", "fire" };
            string[] waterResist = { "water", "grass", "dragon" };
            Type water = new Type("water", waterStrengths, waterResist);

            //Create the grass type
            string[] grassStrengths = { "rock", "ground", "water" };
            string[] grassResists = { "dragon", "fire", "grass", "steel", "poison", "flying" };
            Type grass = new Type("grass", grassStrengths, grassResists);

            //Create the normal type
            string[] normalStrengths = { };
            string[] normalResists = { "rock", "steel", "ghost" };
            string[] normalImmunity = { "ghost" };
            Type normal = new Type("normal", normalStrengths, normalResists, normalImmunity);

            string[] poisonStrengths = { "fairy", "grass" };
            string[] poisonResists = { "fighting", "poison", "bug", "ground", "rock" };
            string[] poisonImmunity = { "steel" };
            Type poison = new Type("poison", poisonStrengths, poisonResists, poisonImmunity);

            string[] iceStrengths = { "dragon", "grass", "flying", "ground" };
            string[] iceResists = { "steel", "rock", "fighting", "fire", "water" };
            Type ice = new Type("ice", iceStrengths, iceResists);

            //////////////////////////////////////


            //          POKEMON ATTACK DATA            //

            AttackMove waterGun = new AttackMove("water gun", 40, 100, false, water);
            AttackMove surf = new AttackMove("surf", 90, 100, false, water);
            RainyDay rainDance = new RainyDay("rain dance", 0, 0, false, water, battle, true);

            AttackMove tackle = new AttackMove("tackle", 40, 100, true, normal);
            AttackMove explosion = new AttackMove("explosion", 250, 100, true, normal);

            AttackMove ember = new AttackMove("ember", 40, 100, false, fire);
            AttackMove flamethrower = new AttackMove("flamethrower", 90, 100, false, fire);
            SunnyDay sunnyDay = new SunnyDay("sunny day", 0, 0, false, fire, battle, true);

            AttackMove vineWhip = new AttackMove("vine whip", 0, 100, true, grass);
            AttackMove energyBall = new AttackMove("energy ball", 90, 100, false, grass);

            AttackMove iceBeam = new AttackMove("ice beam", 90, 100, false, ice);

            StatusMove tailWhip = new StatusMove("tail whip", 0, 100, false, normal, StatType.Defense, -1, true);

            //Creates squirtles attacks
            AttackMove[] squirtleAttacks = new AttackMove[4];
            squirtleAttacks[0] = rainDance;
            squirtleAttacks[1] = surf;
            squirtleAttacks[2] = tackle;
            squirtleAttacks[3] = iceBeam;

            //Creates charmanders attacks
            AttackMove[] charmanderAttacks = new AttackMove[4];
            charmanderAttacks[0] = ember;
            charmanderAttacks[1] = flamethrower;
            charmanderAttacks[2] = tackle;
            charmanderAttacks[3] = tailWhip;

            //Create bulbasaur attacks
            AttackMove[] bulbasaurAttacks = new AttackMove[4];
            bulbasaurAttacks[0] = vineWhip;
            bulbasaurAttacks[1] = tackle;
            bulbasaurAttacks[2] = energyBall;
            bulbasaurAttacks[3] = explosion;

            //////////////////////////////////////


            //          CREATES THE POKEMON             //

            Pokemon squirtle = new Pokemon("Squirtle", water, 25, 5, 15, 15, 15, 15, 18, squirtleAttacks);

            Pokemon charmander = new Pokemon("Charmander", fire, 25, 5, 15, 15, 15, 15, 19, charmanderAttacks);

            Pokemon bulbasaur = new Pokemon("Bulbasaur", grass, 25, 5, 15, 15, 15, 15, 17, bulbasaurAttacks);

            List<Pokemon> team1 = new List<Pokemon>(6);
            team1.Add(squirtle.Clone());
            team1.Add(charmander.Clone());
            team1.Add(bulbasaur.Clone());

            List<Pokemon> team2 = new List<Pokemon>(6);
            team2.Add(squirtle.Clone());
            team2.Add(charmander.Clone());
            team2.Add(bulbasaur.Clone());

            Trainer trainer1 = new Trainer("Chris", team1);

            Trainer trainer2 = new Trainer("Bryan", team2);

            //////////////////////////////////////
            
            battle.Start(trainer1, trainer2);
        }
    }
}