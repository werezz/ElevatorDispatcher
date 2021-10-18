using Elevator_Dispatcher.Models;

namespace Elevator_Dispatcher.Services
{
    public interface IElevatorRoutingService
    {
        ElevatorActionResult InitiateRoute(int startFloor, int targetFloor);
    }
}