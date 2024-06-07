using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageOfTesting.CompleteActions
{
    public class QuarryComplete : VillageCompleteAction
    {
        public QuarryComplete(Village village) : base(village)
        {
        }

        public override void UponCompletion()
        {
            village.MetalPerDay += 1;
        }
    }
}
