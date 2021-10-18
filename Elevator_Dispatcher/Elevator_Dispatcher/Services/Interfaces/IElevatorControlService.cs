using Elevator_Dispatcher.Models;

namespace Elevator_Dispatcher.Services
{
    public interface IElevatorControlService
    {
        ElevatorActionResult LockDoor(ElevatorModel elevator);
        ElevatorActionResult UnlockDoor(ElevatorModel elevator);
        ElevatorActionResult GoToFloor(ElevatorModel elevator, int targetFloor);
    }
}