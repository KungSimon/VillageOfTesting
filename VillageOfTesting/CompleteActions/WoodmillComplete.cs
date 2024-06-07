 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageOfTesting.CompleteActions
{
    public class WoodmillComplete : VillageCompleteAction
    {
        public WoodmillComplete(Village village) : base(village)
        {
        }

        public override void UponCompletion()
        {
            village.WoodPerDay += 1;
        }
    }
}
