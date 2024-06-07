using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageOfTesting.OccupationActions
{
    public class LumberjackAction : VillageAction
    {
        public LumberjackAction(Village village) : base(village)
        {
        }

        public override void Work(string name)
        {
            village.Wood += village.WoodPerDay;
            Console.WriteLine(name + " gathers " + village.WoodPerDay + " wood!");
        }
    }
}
