using VillageOfTesting;
using VillageOfTesting.Interfaces;
using VillageOfTesting.Objects;

namespace VillageTest
{
    public class WorkerTest
    {
        private Village village;

        public WorkerTest()
        {
            village = new Village();
        }


        [Fact]
        public void AddWorker_ShouldAddWorkerToList()
        {
            village = new Village();
            village.AddWorker("Worker1", "farmer");
            Assert.Single(village.Workers);
            Assert.Equal("Worker1", village.Workers[0].Name);
            Assert.Equal("farmer", village.Workers[0].Occupation);
            Assert.True(village.Workers[0].Alive);
            Assert.Equal(0, village.Workers[0].DaysHungry);
        }

        [Fact]
        public void AddWorker_ShouldAddMultipleWorkers()
        {
            village = new Village();
            int numWorkers = 3;
            string[] workerNames = { "Simon", "Simme", "Mike" };

            for (int i = 0; i < numWorkers; i++)
            {
                village.AddWorker(workerNames[i], "farmer");
            }

            Assert.Equal(numWorkers, village.Workers.Count);
            for (int i = 0; i < numWorkers; i++)
            {
                Assert.Equal(workerNames[i], village.Workers[i].Name);
                Assert.Equal("farmer", village.Workers[i].Occupation);
            }
        }

        [Fact]
        public void AddWorker_ShouldNotAddWhenNoSpace()
        {
            village = new Village();
            for (int i = 0; i < village.MaxWorkers; i++)
            {
                village.AddWorker($"Worker{i + 1}", "farmer");
            }
            var initialCount = village.Workers.Count;
            village.AddWorker("ExtraWorker", "farmer");
            Assert.Equal(initialCount, village.Workers.Count);
        }

        [Fact]
        public void NextDay_NoWorkers_ShouldProgressDay()
        {
            village = new Village();
            var initialDay = village.DaysGone;
            village.Day();
            Assert.Equal(initialDay + 1, village.DaysGone);
        }

        [Theory]
        [InlineData(0, 0, 0, 1)] 
        [InlineData(10, 0, 9, 0)] 
        public void NextDay_WithAndWithoutFood(int initialFood, int initialDaysHungry, int expectedFood, int expectedDaysHungry)
        {
            village = new Village();
            village.AddWorker("Worker1", "miner");
            village.Food = initialFood;
            village.Workers[0].DaysHungry = initialDaysHungry;

            village.Day();

            Assert.Equal(expectedFood, village.Food);
            Assert.Equal(expectedDaysHungry, village.Workers[0].DaysHungry);
        }


        [Theory]
        [InlineData("Woodmill", 0, 0)]
        [InlineData("House", 1, 1)]
        [InlineData("Quarry", 1, 1)]
        [InlineData("Farm", 1, 1)]
        [InlineData("Castle", 1, 1)]
        public void AddProject_ShouldNotAddProject_WhenResourcesAreInsufficient(string projectName, int initialWood, int initialMetal)
        {
            village = new Village();
            village.Wood = initialWood;
            village.Metal = initialMetal;

            int initialWoodBefore = village.Wood;
            int initialMetalBefore = village.Metal;
            int initialProjectsCount = village.Projects.Count;

            village.AddProject(projectName);

            Assert.DoesNotContain(village.Projects, p => p.Name == projectName);
            Assert.Equal(initialProjectsCount, village.Projects.Count);
            Assert.Equal(initialWoodBefore, village.Wood);
            Assert.Equal(initialMetalBefore, village.Metal);
        }
       

        [Theory]
        [InlineData("Woodmill", 5, 1, 5, "Woodmill", true)]
        [InlineData("House", 5, 0, 3, "House", true)]
        [InlineData("Quarry", 3, 5, 7, "Quarry", true)]
        [InlineData("Farm", 5, 2, 5, "Farm", true)]
        [InlineData("Castle", 50, 50, 50, "Castle", true)]
        public void AddProject_ShouldAddProject_WhenResourcesAreSufficient(string projectName, int wood, int metal, int daysToComplete, string expectedProjectName, bool isExpectedToBeAdded)
        {
            village = new Village();
            village.Wood = wood;
            village.Metal = metal;

            village.AddProject(projectName);

            if (isExpectedToBeAdded)
            {
                Assert.Contains(village.Projects, p => p.Name == expectedProjectName);
                Assert.Equal(0, village.Wood);
                Assert.Equal(0, village.Metal);
            }
            else
            {
                Assert.DoesNotContain(village.Projects, p => p.Name == expectedProjectName);
                Assert.Equal(wood, village.Wood);
                Assert.Equal(metal, village.Metal);
            }
        }


        //[Theory]
        //[InlineData("Woodmill", 5, 1, "Woodmill", 1, 0, 0, 0)]  
        //[InlineData("Quarry", 3, 5, "Quarry", 0, 1, 0, 0)]      
        //[InlineData("Farm", 5, 2, "Farm", 0, 0, 5, 0)]          
        //[InlineData("House", 6, 6, "House", 0, 0, 0, 2)]        
        //public void AddProject_ShouldIncreaseProductionOrCapacity(
        //    string projectName,
        //    int initialWood,
        //    int initialMetal,
        //    string expectedBuildingName,
        //    int expectedWoodPerDay,
        //    int expectedMetalPerDay,
        //    int expectedFoodPerDay,
        //    int expectedMaxWorkers)
        //{
        //    village.Wood = initialWood;
        //    village.Metal = initialMetal;

        //    village.AddProject(projectName);

        //    while (village.Projects.Count > 0)
        //    {
        //        village.Day();
        //    }

        //    //Assert.Contains(village.Buildings, b => b.Name == expectedBuildingName);
        //    Assert.Equal(expectedWoodPerDay, village.WoodPerDay);
        //    Assert.Equal(expectedMetalPerDay, village.MetalPerDay);
        //    Assert.Equal(expectedFoodPerDay, village.FoodPerDay);
        //    Assert.Equal(expectedMaxWorkers, village.MaxWorkers);
        //}


        [Fact]
        public void BuildersShouldWorkOnProjects()
        {
            Village village = new Village();
            village.Wood = 5;
            village.Metal = 1;

            village.AddWorker("Builder1", "builder");
            village.AddProject("Woodmill");

            Assert.Single(village.Projects);
            var project = village.Projects[0];
            int initialDaysLeft = project.DaysLeft;

            while (village.Projects.Count > 0)
            {
                village.Day();
            }

            Assert.Empty(village.Projects);
            Assert.Equal(initialDaysLeft, project.DaysLeft + initialDaysLeft);
            Assert.Contains(village.Buildings, b => b.Name == "Woodmill");
            Assert.Equal(2, village.WoodPerDay);
        }


        [Fact]
        public void Day_ShouldKillWorkerAfterStarvation()
        {
            village = new Village();
            village.AddWorker("Worker1", "miner");
            village.Food = 0;

            int starvationDays = Worker.daysUntilStarvation;

            for (int i = 0; i <= starvationDays; i++)
            {
                village.Day();
            }

            Assert.Equal(0, village.Workers.Count);
            Assert.True(village.GameOver); 
        }

        [Fact]
        public void GameOverAfterAllWorkersDead()
        {
            var village = new Village();
            village.AddWorker("Worker1", "miner");
            village.Food = 0;

            while (village.Workers.Any(worker => worker.Alive))
            {
                village.Day();
            }

            village.Day();

            Assert.Equal(0, village.Workers.Count);
            Assert.True(village.GameOver, "Game should be over after all workers are dead");
        }

        [Fact]
        public void StartToFinish_GameFlowTest()
        {
            village = new Village();
            village.Wood = 50;
            village.Metal = 50;

            village.AddWorker("Farmer1", "farmer");
            village.AddWorker("Builder1", "builder");

            village.AddProject("Castle");

            while (!village.GameOver)
            {
                village.Day();
            }
            Assert.Contains(village.Buildings, b => b.Name == "Castle");
            Assert.True(village.GameOver);
        }


    }
}
