using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillageOfTesting.Interfaces;

namespace VillageOfTesting.CompleteActions
{
    public abstract class VillageCompleteAction : ICompleteAction
    {
        protected Village village;
        public VillageCompleteAction(Village village)
        {
            this.village = village;
        }
        public abstract void UponCompletion();
    }

}
