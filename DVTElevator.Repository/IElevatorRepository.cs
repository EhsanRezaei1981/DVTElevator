using DTVElevator.Dto.Model;
using DTVElevator.Service.Elevator;

namespace DVTElevator.Repository
{
    public interface IElevatorRepository
    {
        ErrorHandling AddElevator(ElevatorService elevator);
        ErrorHandling TakeAnElevator(int floor, Person person);
        ErrorHandling Move();
        ErrorHandling GetOff();
    }
}