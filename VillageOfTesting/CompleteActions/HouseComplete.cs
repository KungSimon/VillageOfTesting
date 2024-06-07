using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageOfTesting.CompleteActions
{
    public class HouseComplete : VillageCompleteAction
    {
        public HouseComplete(Village village) : base(village)
        {
        }

        public override void UponCompletion()
        {
            village.MaxWorkers += 2;
        }
    }
}
