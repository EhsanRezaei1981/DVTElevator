using DVTElevator.Repository;

namespace DVTElevator.xUnitTest
{
    public class ElevatorUnitTest
    {
        ElevatorRepository repo;
        public ElevatorUnitTest()
        {
            repo = new ElevatorRepository();
            repo.AddElevator(new DTVElevator.Service.Elevator.ElevatorService("EL1", new List<int> { { 1 }, { 3 }, { 5 }, { 7 } }, 200, 1));
            repo.AddElevator(new DTVElevator.Service.Elevator.ElevatorService("EL2", new List<int> { { 1 }, { 3 }, { 5 }, { 7 } }, 200, 1));
            repo.AddElevator(new DTVElevator.Service.Elevator.ElevatorService("EL3", new List<int> { { 2 }, { 4 }, { 6 }, { 8 } }, 200, 2));
            repo.AddElevator(new DTVElevator.Service.Elevator.ElevatorService("EL4", new List<int> { { 2 }, { 4 }, { 6 }, { 8 } }, 200, 2));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(9)]
        public void TakeAnElevator_UnsuportFloor_MustReturnFalse(int floor)
        {
            //Arrange

            //Act
            var (result, _) = repo.RequestElevator(floor);

            //Assert
            Assert.False(result.Successful);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public void TakeAnElevator_Floor_1_3_5_MustReturn_EL1OrEL2(int floor)
        {
            //Arrange

            //Act
            var (result, elevator) = repo.RequestElevator(floor);

            //Assert
            Assert.True(result.Successful && (elevator.Name.Equals("EL1") || elevator.Name.Equals("EL2")));
        }
    }
}