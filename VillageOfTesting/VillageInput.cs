using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using VillageOfTesting.Interfaces;
using VillageOfTesting.Objects;

namespace VillageOfTesting
{
    public class VillageInput
    {
        private delegate void Action();

        DatabaseConnection databaseConnection = new DatabaseConnection();
        Village village = new Village();
        private Dictionary<string, Action> actions = new Dictionary<string, Action>();
        List<string> options = new List<string>();


        public VillageInput(Village village, DatabaseConnection databaseConnection)
        {
            MakeDelegates();
            this.village = village;
            this.databaseConnection = databaseConnection;
        }
        public VillageInput()
        {
            MakeDelegates();
        }

        private void MakeDelegates()
        {
            actions.Add("1", () => AddWorker());
            actions.Add("2", () => AddProject());
            actions.Add("3", () => village.Day());
            actions.Add("4", () => Load());
            actions.Add("5", () => Save());
            actions.Add("6", () => village.GameOver = true);

            options.Add("1: Add Worker.");
            options.Add("2: Add Project.");
            options.Add("3: Proceed to next day.");
            options.Add("4: Load saved game.");
            options.Add("5: Save progress.");
            options.Add("6: Quit.");
        }

        public void Run()
        {
            Console.WriteLine("Welcome to the Village of Testing!");

            while (!village.GameOver)
            {
                Console.WriteLine("Your village looks like...");

                village.PrintInfo();

                while (true)
                {
                    Console.WriteLine();
                    Console.WriteLine("Day " + village.DaysGone);
                    Console.WriteLine("What would you like to do?");
                    foreach (string option in options)
                    {
                        Console.WriteLine(option);
                    }

                    string choice = Console.ReadLine();
                    if (actions.ContainsKey(choice))
                    {
                        actions[choice]();
                        break;
                    }
                    Console.WriteLine("That's not an option.");
                }
            }
        }
        private void AddWorker()
        {
            Console.WriteLine("What will be the worker's name?");
            string name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Please do write a name.");
                return;
            }
            Console.WriteLine("What's their job? The options are Farmer, Lumberjack, Miner or Builder.");
            string occupation = Console.ReadLine().ToLower();
            village.AddWorker(name, occupation);
            Console.WriteLine();
        }
        private void AddProject()
        {
            Console.WriteLine("Which project? Possible choices are: ");
            foreach (PossibleProject possibleProject in village.PossibleProjects.Values)
            {
                Console.WriteLine(possibleProject.Name + ": " + possibleProject.WoodCost + " wood, " + possibleProject.MetalCost + " metal");
            }
            string name = Console.ReadLine();
            village.AddProject(name);
            Console.WriteLine();
        }

        public void Save()
        {
            Console.WriteLine("What name do you wish to save the village under? Current villages are: ");
            List<string> villages = databaseConnection.GetTownNames();

            foreach (string villageName in villages)
            {
                Console.Write(villageName + " ");
            }
            Console.WriteLine();

            string choice = Console.ReadLine();

            if (villages.Contains(choice))
            {
                Console.WriteLine("Are you sure you want to overwrite " + choice + "? Write \"y\" for yes. Anything else for no.");
                string yes = Console.ReadLine().ToLower();
                if (!yes.Equals("y"))
                {
                    Console.WriteLine("Cancelling load.");
                    return;
                }
            }

            bool success = databaseConnection.SaveVillage(village, choice);

            if (success)
            {
                Console.WriteLine("Village " + choice + " successfully saved.");
            }
            else
            {
                Console.WriteLine("Error, something went wrong. Could not save.");
            }
        }

        public void Load()
        {
            Console.WriteLine("Which village would you like to load? The choices are: ");
            List<string> villages = databaseConnection.GetTownNames();

            foreach (string villageName in villages)
            {
                Console.Write(villageName + " ");
            }
            Console.WriteLine();

            string choice = Console.ReadLine();

            if (!villages.Contains(choice))
            {
                Console.WriteLine("That's not one of the choices.");
                return;
            }

            Village loadedVillage = databaseConnection.LoadVillage(choice);
            if (loadedVillage != null)
            {
                Console.WriteLine("Village " + choice + " successfully loaded.");
                village = loadedVillage;
            }
            else
            {
                Console.WriteLine("Load failed.");
            }
        }
    }
}
