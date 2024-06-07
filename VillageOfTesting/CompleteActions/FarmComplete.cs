using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageOfTesting.CompleteActions
{
    public class FarmComplete : VillageCompleteAction
    {
        public FarmComplete(Village village) : base(village)
        {
        }
        public override void UponCompletion()
        {
            village.FoodPerDay += 5;
        }
    }
}
