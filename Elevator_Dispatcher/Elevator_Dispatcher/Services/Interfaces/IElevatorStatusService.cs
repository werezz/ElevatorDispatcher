using Elevator_Dispatcher.Models;

namespace Elevator_Dispatcher.Services
{
    public interface IElevatorStatusService
    {
        public string GetStatus(ElevatorModel elevator);
    }
}