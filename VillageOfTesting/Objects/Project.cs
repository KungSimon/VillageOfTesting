using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillageOfTesting.Interfaces;

namespace VillageOfTesting.Objects
{
    public class Project : Building
    {
        public int DaysLeft { get; set; }
        public ICompleteAction CompleteAction { get; set; }

        public Project(string name, int daysLeft, ICompleteAction completeAction) : base(name)
        {
            DaysLeft = daysLeft;
            CompleteAction = completeAction;
        }

        public void Complete()
        {
            CompleteAction.UponCompletion();
        }

        public bool BuildOn()
        {
            DaysLeft--;
            return DaysLeft < 1;
        }
    }
}
