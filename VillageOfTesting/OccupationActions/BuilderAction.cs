using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using VillageOfTesting.Objects;

namespace VillageOfTesting.OccupationActions
{
    public class BuilderAction : VillageAction
    {
        public BuilderAction(Village village) : base(village)
        {
        }

        public override void Work(string name)
        {
            List<Project> projects = village.Projects;

            if (projects.Count > 0)
            {
                Project currentProject = projects[0];
                Console.WriteLine(name + " builds on " + currentProject.Name + "!");
                bool complete = currentProject.BuildOn();
                if (complete)
                {
                    projects.Remove(currentProject);
                    village.Buildings.Add(new Building(currentProject.Name));
                    Console.WriteLine(currentProject.Name + " was completed!");
                    currentProject.Complete();
                }
            }
            else
            {
                Console.WriteLine("No buildings for " + name + " to work on!");
            }
        }
    }
}
