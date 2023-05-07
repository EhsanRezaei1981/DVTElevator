
using DTVElevator.Dto;
using DVTElevator.Repository;

Console.WriteLine("Welcome to DVT elevator challenge");
Console.WriteLine("Please use the menu to command the DVElevator");
var isEnd = false;
var command= "";
var isExit = false;
var repo = new ElevatorRepository();
int? elevatorRequestedFromFloor = null;
List<Menu> menus;
loadMenus();

while (!isEnd) {
    generateMainMenu();
    command = Console.ReadLine();
    switch (command)
    {
        case "1":
            createElevator();
            break;
        case "2":
            listOfElavators();
            break;
        case "3":
            takeAnElevator();
            break;
        case "4":
            moveElevator();
            break;
        case "5":
            getOff();
            break;
        case "6":
            Console.Clear();
            break;
        case "0":
            isExit = true;
            break;
    }
    if (isExit) break;
}

Console.WriteLine("Bye");

void generateMainMenu()
{
    Console.WriteLine();
    Console.WriteLine("-------Menu-------");
    for (int i = 0; i < menus.Count; i++)
    {
        var menu = menus[i];
        Console.WriteLine($"{menu.Command}. {menu.Name}");
    }
    Console.WriteLine();
    Console.WriteLine();
}

void createElevator() {
    try
    {
        Console.Write("Elevator Name: ");
        var name = Console.ReadLine();
        Console.Write("Working Floors (Comma sepration): ");
        var _workingFloors = Console.ReadLine();
        var workingFloors = new List<int>();
        var split = _workingFloors.Split(",");
        foreach (var item in split)
        {
            workingFloors.Add(int.Parse(item.Trim()));
        }
        Console.Write("Max Weight: ");
        var maxWeight =int.Parse(Console.ReadLine());
        var currentFloor = workingFloors.OrderBy(x => x).FirstOrDefault();
        var result = repo.AddElevator(new DTVElevator.Service.Elevator.ElevatorService(name, workingFloors, maxWeight, currentFloor));
        WriteMessage(result.Message, !result.Successful);
    }
    catch (Exception ex)
    {
        WriteMessage(ex.Message, true);
    }
}

void WriteMessage(string message,bool isError) {
    Console.ForegroundColor = isError ? ConsoleColor.Red : ConsoleColor.Green;
    Console.WriteLine(message);
    Console.ForegroundColor = ConsoleColor.White;
}

void listOfElavators() {
    if (repo.Elevators.Count == 0) {
        WriteMessage("No elevators specified",true);
        return;
    }
    Console.WriteLine($"-----List of Elevators-----");
    foreach (var item in repo.Elevators)
    {
        Console.WriteLine($"Name: {item.Name}");
        Console.WriteLine($"Direction: {item.GetDirection()}");
        Console.WriteLine($"Current Floor: {item.CurrentFloor}");
        Console.WriteLine($"Max Weight: {item.MaxWeight}");
        Console.WriteLine($"Working Floors: {string.Join(",", item.WorkingFloors)}");
        Console.WriteLine($"People : {item.People.Count}");
        
        for (int i = 0; i < item.People.Count; i++)
        {
            var person = item.People[i];
            Console.WriteLine($"    {i + 1}. {person.Weight}(Kg) person wants to go to floor {person.FloorToGo}");
        }
        
        Console.WriteLine($"Total Weight : {item.People.Sum(x => x.Weight)}");
        Console.WriteLine();
    }
}
void takeAnElevator() {
    try
    {
        if (repo.Elevators.Count == 0)
        {
            WriteMessage("No elevators specified", true);
            return;
        }

        Console.Write("Take an elevator from floor: ");
        var floor =int.Parse( Console.ReadLine());
        
        Console.Write("Weight of the passenger: ");
        var weight= int.Parse(Console.ReadLine());
        
        Console.Write("Floor to go: ");
        var floorToGo = int.Parse(Console.ReadLine());

        var errorHandling = repo.TakeAnElevator(floor, new DTVElevator.Dto.Model.Person { Weight = weight, FloorToGo = floorToGo });
        if (!errorHandling.Successful) {
            WriteMessage(errorHandling.Message, true);
            return;
        }
        elevatorRequestedFromFloor= floor;
    }
    catch (Exception ex) {
        WriteMessage(ex.Message, true);
    }
}

void moveElevator()
{
    var result=repo.Move();
    WriteMessage(result.Message, !result.Successful);
}

void getOff()
{
    var result = repo.GetOff();
    WriteMessage(result.Message, !result.Successful);
}

void loadMenus()
{
    menus = new List<Menu>{
    new Menu{
        Command="1",
        Name="Create Elevator"
    },
    new Menu{
        Command="2",
        Name="List of Elevators"
    },
    new Menu{
        Command="3",
        Name="Take an Elevator"
    },
    new Menu{
        Command="4",
        Name="Move"
    },
    new Menu{
        Command="5",
        Name="Get Off"
    },
    new Menu{
        Command="6",
        Name="Clear Console"
    },
    new Menu{
        Command="0",
        Name="Exit"
    }
};
}
