using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGame
{
    public class Trainer
    {
        public string Name;

        public List<Pokemon> Team;

        public int AlivePokemon { get; set; }

        public Trainer(string name, List<Pokemon> team)
        {
            Name = name;
            Team = team;
            AlivePokemon = team.Count;
        }
    }
}
