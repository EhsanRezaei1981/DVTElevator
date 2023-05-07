using DTVElevator.Dto.Model;
using DTVElevator.Service.Elevator;

namespace DVTElevator.Repository
{
    public interface IElevatorRepository
    {
        List<ElevatorService> Elevators { get; set; }
        ErrorHandling AddElevator(ElevatorService elevator);
        (ErrorHandling ErrorHandling, ElevatorService? Elevator) RequestElevator(int floor);
    }
}