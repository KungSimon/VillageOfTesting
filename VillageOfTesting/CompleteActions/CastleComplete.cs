using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageOfTesting.CompleteActions
{
    public class CastleComplete : VillageCompleteAction
    {
        public CastleComplete(Village village) : base(village)
        {
        }

        public override void UponCompletion()
        {
            Console.WriteLine("Castle complete! It took " + village.DaysGone + " days!");
            village.GameOver = true;
        }
    }
}
