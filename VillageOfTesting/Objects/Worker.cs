using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VillageOfTesting.Interfaces;

namespace VillageOfTesting.Objects
{
    public class Worker
    {
        public static int daysUntilStarvation = 5;
        public string Name { get; set; }
        public string Occupation { get; set; }
        public IOccupationAction OccupationAction { get; set; }
        public bool Hungry { get; set; }
        public bool Alive { get; set; }
        public int DaysHungry { get; set; }

        public Worker(string name, string occupation, IOccupationAction occupationAction, bool hungry, bool alive, int daysHungry)
        {
            Name = name;
            Occupation = occupation;
            OccupationAction = occupationAction;
            Hungry = hungry;
            Alive = alive;
            DaysHungry = daysHungry;
        }

        public Worker(string name, string occupation, IOccupationAction occupationAction)
        {
            Name = name;
            Occupation = occupation;
            OccupationAction = occupationAction;
            Hungry = false;
            Alive = true;
            DaysHungry = 0;
        }

        public void DoWork()
        {
            if (!Alive)
            {
                Console.WriteLine(Name + " is not alive and cannot work...");
                return;
            }
            if (!Hungry)
            {
                OccupationAction.Work(Name);
                Hungry = true;
            }
            else
            {
                DaysHungry++;
                if (DaysHungry >= daysUntilStarvation)
                {
                    Alive = false;
                    Console.WriteLine(Name + " has died of hunger!");
                }
            }
        }

        public void Feed()
        {
            if (Alive)
            {
                DaysHungry = 0;
                Hungry = false;
            }
        }
    }
}
