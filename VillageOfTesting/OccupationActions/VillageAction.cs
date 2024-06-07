using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillageOfTesting.Interfaces;

namespace VillageOfTesting.OccupationActions
{
    public abstract class VillageAction : IOccupationAction
    {
        protected Village village;

        public VillageAction(Village village)
        {
            this.village = village;
        }

        public abstract void Work(string name);
    }
}
