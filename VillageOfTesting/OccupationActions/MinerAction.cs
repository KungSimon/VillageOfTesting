using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageOfTesting.OccupationActions
{
    public class MinerAction : VillageAction
    {
        public MinerAction(Village village) : base(village)
        {
        }

        public override void Work(string name)
        {
            village.Metal += village.MetalPerDay;
            Console.WriteLine(name + " gathers " + village.MetalPerDay + " metal!");
        }
    }
}
