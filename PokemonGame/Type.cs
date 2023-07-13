using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGame
{
    public class Type
    {
        public string Name { get; set; }

        public string[] StrongAgainst { get; set; }

        public string[] TypeResistance { get; set; }

        public string[] ImmuneType { get; set; }

        public Type(string name, string[] strongAgainst, string[] typeResistance, string[] immuneType = null)
        {
            Name = name;
            StrongAgainst = strongAgainst;
            TypeResistance = typeResistance;
            ImmuneType = immuneType;
        }
    }
}
