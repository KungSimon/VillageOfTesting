using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillageOfTesting.Interfaces;

namespace VillageOfTesting.Objects
{
    public class PossibleProject : Building
    {
        public int WoodCost { get; set; }
        public int MetalCost { get; set; }
        public int DaysToComplete { get; set; }
        public ICompleteAction CompleteAction { get; set; }

        public PossibleProject(string name) : base(name)
        {
        }

        public PossibleProject(string name, int woodCost, int metalCost, int daysToComplete, ICompleteAction completeAction) : base(name)
        {
            WoodCost = woodCost;
            MetalCost = metalCost;
            DaysToComplete = daysToComplete;
            CompleteAction = completeAction;
        }

        public Project GetProject()
        {
            return new Project(Name, DaysToComplete, CompleteAction);
        }
    }
}
