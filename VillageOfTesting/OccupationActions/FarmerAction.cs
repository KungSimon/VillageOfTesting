using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageOfTesting.OccupationActions
{
    public class FarmerAction : VillageAction
    {
        public FarmerAction(Village village) : base(village)
        {
        }

        public override void Work(string name)
        {
            village.Food += village.FoodPerDay;
            Console.WriteLine(name + " gathers " + village.FoodPerDay + " food!");
        }
    }
}
