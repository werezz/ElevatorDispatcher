using Elevator_Dispatcher.Models;

namespace Elevator_Dispatcher.Services
{
    public interface IElevatorPoolService
    {
        ElevatorModel TakeClosestElevator(int floor);
        ElevatorActionResult ReleaseElevator(ElevatorModel elevator);
        ElevatorModel GetElevator(int id);
    }
}