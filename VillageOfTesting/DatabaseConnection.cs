using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageOfTesting
{
    public class DatabaseConnection
    {
        // This class exists to be mocked using Moq.
        // It should not be changed.

        public List<string> GetTownNames()
        {
            return new List<string>() { "These", "are", "placeholders", "to", "make", "sure", "it", "works" };
        }

        public Village LoadVillage(string choice)
        {
            return null;
        }

        public bool SaveVillage(Village village, string choice)
        {
            return false;
        }
    }
}
