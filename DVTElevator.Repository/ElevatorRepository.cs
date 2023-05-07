using DTVElevator.Dto.Model;
using DTVElevator.Service.Elevator;
using System.Security.Cryptography.X509Certificates;

namespace DVTElevator.Repository
{
    public class ElevatorRepository: IElevatorRepository
    {
        public List<ElevatorService> Elevators = new();

        public  ErrorHandling AddElevator(ElevatorService elevator) {
            if (string.IsNullOrWhiteSpace(elevator?.Name))
                return new ErrorHandling
                {
                    Message = "Please provide the name for your elevator"
                };
            
            if (Elevators.Any(x => x.Name.Equals(elevator.Name, StringComparison.OrdinalIgnoreCase)))
                return new ErrorHandling
                {
                    Message = "Please select another name for your elevator as the name provided exists."
                };

            Elevators.Add(elevator);
            return new ErrorHandling
            {
                Message = $"Elevator {elevator.Name} is added successfully.",
                Successful = true
            };
        }

        public  ErrorHandling TakeAnElevator(int floor, Person person) {
            if (Elevators.Count == 0)
                return new ErrorHandling
                {
                    Message = "No elevator is specified."
                };

            var availableElevators = new List<ElevatorService>();
            foreach (var elevator in Elevators)
            {
                var totalWeight = elevator.People.Sum(x => x.Weight);
                if (elevator.WorkingFloors.Any(x => x == floor) && totalWeight <= elevator.MaxWeight)
                    availableElevators.Add(elevator);
            }

            if (availableElevators.Count == 0)
                return new ErrorHandling
                {
                    Message = $"There is no available elevator for floor {floor}."
                };

            var tmpList = availableElevators.Where(
                x =>
                    (x.Direction == DTVElevator.Dto.Enums.ElevatorDirection.Up && x.CurrentFloor <= floor) ||
                    (x.Direction == DTVElevator.Dto.Enums.ElevatorDirection.Down && x.CurrentFloor >= floor)
            ).ToList();

            if (tmpList.Count == 0)
            {
                tmpList = availableElevators.ToList();
            }

            foreach (var item in tmpList)
            {
                item.Difference = Math.Abs((item.CurrentFloor ?? (floor+1)) - floor);
            }

            var elev = tmpList.OrderBy(x => x.Difference).FirstOrDefault();

            if (!elev.WorkingFloors.Any(x => x == person.FloorToGo))
                return new ErrorHandling
                {
                    Message = $"Limited floor. There is no elevator to be used to go to floor {person.FloorToGo} or the floor does not exist.",
                };

            //elev.SetCurrentFloor( floor);
            elev.People.Add(person);
            return new ErrorHandling
            {
                Successful = true,
                Message = $"The elevator<{elev.Name}> has been selected"
            };

        }

        public ErrorHandling Move()
        {
            var result = new ErrorHandling
            {
                Successful = true
            };

            if (Elevators.Count == 0)
                return new ErrorHandling
                {
                    Message = "No elevator is specified."
                };

            foreach (var elevator in Elevators)
            {
                var res=elevator.Move();
                if (!res.Successful)
                    result = res;
            }

            return result;

        }

        public ErrorHandling GetOff()
        {
            var result = new ErrorHandling
            {
                Successful = true
            };

            if (Elevators.Count == 0)
                return new ErrorHandling
                {
                    Message = "No elevator is specified."
                };

            foreach (var elevator in Elevators)
            {
                var res = elevator.GetOff();
                if (!res.Successful)
                    result = res;
            }

            return result;

        }
    }
}