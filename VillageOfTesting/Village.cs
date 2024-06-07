using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillageOfTesting.CompleteActions;
using VillageOfTesting.Interfaces;
using VillageOfTesting.Objects;
using VillageOfTesting.OccupationActions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VillageOfTesting
{
    public class Village
    {
        public int StarvationDays { get; set; } = 5; // Default starvation days
        public bool GameOver { get; set; } = false;
        public int Food { get; set; } = 0;
        public int Wood { get; set; } = 0;
        public int Metal { get; set; } = 0;
        public List<Worker> Workers { get; set; } = new List<Worker>();
        public List<Building> Buildings { get; set; } = new List<Building>();
        public List<Project> Projects { get; set; } = new List<Project>();
        public Dictionary<string, IOccupationAction> OccupationDictionary { get; set; } = new Dictionary<string, IOccupationAction>();
        public Dictionary<string, PossibleProject> PossibleProjects { get; set; } = new Dictionary<string, PossibleProject>();
        public int MetalPerDay { get; set; } = 1;
        public int WoodPerDay { get; set; } = 1;
        public int FoodPerDay { get; set; } = 5;
        public int MaxWorkers { get; set; } = 6;
        public int DaysGone { get; set; } = 0;

        public Village(bool gameOver, int food, int wood, int metal, List<Worker> workers, List<Building> buildings, List<Project> projects, Dictionary<string, IOccupationAction> occupationDictionary, Dictionary<string, PossibleProject> possibleProjects, int metalPerDay, int woodPerDay, int foodPerDay, int maxWorkers, int daysGone)
        {
            GameOver = gameOver;
            Food = food;
            Wood = wood;
            Metal = metal;
            Workers = workers;
            Buildings = buildings;
            Projects = projects;
            OccupationDictionary = occupationDictionary;
            PossibleProjects = possibleProjects;
            MetalPerDay = metalPerDay;
            WoodPerDay = woodPerDay;
            FoodPerDay = foodPerDay;
            MaxWorkers = maxWorkers;
            DaysGone = daysGone;
        }

        public Village(bool gameOver, int food, int wood, int metal, List<Worker> workers, List<Building> buildings, List<Project> projects, int metalPerDay, int woodPerDay, int foodPerDay, int maxWorkers, int daysGone, int daysUntilStarvation)
        {
            MakeDictionaries();

            GameOver = gameOver;
            Food = food;
            Wood = wood;
            Metal = metal;
            Workers = workers;
            Buildings = buildings;
            Projects = projects;
            FoodPerDay = foodPerDay;
            WoodPerDay = woodPerDay;
            MetalPerDay = metalPerDay;
            MaxWorkers = maxWorkers;
            DaysGone = daysGone;
            Worker.daysUntilStarvation = daysUntilStarvation;
        }

        public Village()
        {
            MakeDictionaries();

            Buildings.Add(new Building("House"));
            Buildings.Add(new Building("House"));
            Buildings.Add(new Building("House"));
            MaxWorkers = 6;
            Food = 10;
        }

        private void MakeDictionaries()
        {
            OccupationDictionary.Add("farmer", new FarmerAction(this));
            OccupationDictionary.Add("lumberjack", new LumberjackAction(this));
            OccupationDictionary.Add("miner", new MinerAction(this));
            OccupationDictionary.Add("builder", new BuilderAction(this));

            PossibleProjects.Add("House", new PossibleProject("House", 5, 0, 3, new HouseComplete(this)));
            PossibleProjects.Add("Woodmill", new PossibleProject("Woodmill", 5, 1, 5, new WoodmillComplete(this)));
            PossibleProjects.Add("Quarry", new PossibleProject("Quarry", 3, 5, 7, new QuarryComplete(this)));
            PossibleProjects.Add("Farm", new PossibleProject("Farm", 5, 2, 5, new FarmComplete(this)));
            PossibleProjects.Add("Castle", new PossibleProject("Castle", 50, 50, 50, new CastleComplete(this)));
        }

        public void Day()
        {
            if (GameOver)
            {
                return;
            }

            FeedWorkers();

            Workers = Workers.Where(worker => worker.Alive).ToList();

            if (!Workers.Any())
            {
                Console.WriteLine("Everyone is dead! You lasted " + DaysGone + " days!");
                GameOver = true;
            }

            foreach (var worker in Workers)
            {
                worker.DoWork();
            }

            DaysGone++;
        }

        public void PrintInfo()
        {
            if (Workers.Count > 0)
            {
                Console.WriteLine("You have " + Workers.Count + " workers. They are: ");
                foreach (Worker worker in Workers)
                {
                    Console.WriteLine(worker.Name + ", " + worker.Occupation + ".");
                    if (worker.Hungry && worker.DaysHungry > 0)
                    {
                        Console.WriteLine(worker.Name + " has been hungry for " + worker.DaysHungry + " days!");
                    }
                }
            }
            else
            {
                Console.WriteLine("You have no workers.");
            }
            Console.WriteLine("Your current buildings are: ");
            foreach (Building building in Buildings)
            {
                Console.Write(building.Name + " ");
            }
            Console.WriteLine();
            Console.WriteLine("You can have " + MaxWorkers + " workers.");
            Console.WriteLine("Your current projects are: ");
            foreach (Project project in Projects)
            {
                Console.Write(project.Name + ", " + project.DaysLeft + " points left until completion.");
            }
            Console.WriteLine();
            Console.WriteLine("Current Food:  " + Food);
            Console.WriteLine("Current Wood:  " + Wood);
            Console.WriteLine("Current Metal: " + Metal);
            Console.WriteLine("Generating " + FoodPerDay + " food per day per worker.");
            Console.WriteLine("Generating " + WoodPerDay + " wood per day per worker.");
            Console.WriteLine("Generating " + MetalPerDay + " metal per day per worker.");
        }


        public void AddWorker(string name, string occupation)
        {

            if(Workers.Count >= MaxWorkers)
            {
                Console.WriteLine("No space for additional workers");
                return;
            }

            if (OccupationDictionary.ContainsKey(occupation))
            {
                IOccupationAction jobInterface = OccupationDictionary[occupation];
                Worker worker = new Worker(name, occupation, jobInterface);
                Workers.Add(worker);
                Console.WriteLine(name + " was successfully added.");
                return;
            }
            Console.WriteLine("There is no such job.");
        }

        public void AddProject(string name)
        {
            if (PossibleProjects.ContainsKey(name))
            {
                PossibleProject possibleProject = PossibleProjects[name];
                Console.WriteLine($"Attempting to add project {name} with costs: Wood={possibleProject.WoodCost}, Metal={possibleProject.MetalCost}");
                Console.WriteLine($"Current resources: Wood={Wood}, Metal={Metal}");

                if (Wood >= possibleProject.WoodCost && Metal >= possibleProject.MetalCost)
                {
                    Wood -= possibleProject.WoodCost;
                    Metal -= possibleProject.MetalCost;

                    Project newProject = possibleProject.GetProject();
                    Projects.Add(newProject);
                    Console.WriteLine($"{newProject.Name} added to the project queue!");
                }
                else
                {
                    Console.WriteLine("Not enough material!");
                }
            }
            else
            {
                Console.WriteLine("That was not one of the options.");
            }
            //if (PossibleProjects.ContainsKey(name))
            //{
            //    PossibleProject possibleProject = PossibleProjects[name];
            //    if (Wood >= possibleProject.WoodCost &&
            //            Metal >= possibleProject.MetalCost)
            //    {
            //        Wood -= possibleProject.WoodCost;
            //        Metal -= possibleProject.MetalCost;

            //        Project newProject = possibleProject.GetProject();
            //        Projects.Add(newProject);
            //        Console.WriteLine(newProject.Name + " added to the project queue!");
            //        return;
            //    }
            //    Console.WriteLine("Not enough material!");
            //    return;
            //}
            //Console.WriteLine("That was not one of the options.");
        }

        private void FeedWorkers()
        {
            foreach (Worker worker in Workers)
            {
                if (Food > 0 && worker.Alive)
                {
                    worker.Feed();
                    Food--;
                }
                else
                {
                    if (worker.Alive)
                    {
                        worker.DaysHungry++;
                        if (worker.DaysHungry >= StarvationDays)
                        {
                            worker.Alive = false;
                            Console.WriteLine(worker.Name + " has died of hunger!");
                        }
                    }
                }
            }
        }
    }
}
