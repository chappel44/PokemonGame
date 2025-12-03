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

        public Type(string typeName, string[] strongAgainst, string[] typeResistance, string[] immuneType = null)
        {
            Name = typeName;
            StrongAgainst = strongAgainst;
            TypeResistance = typeResistance;
            ImmuneType = immuneType;
        }
    }
}
