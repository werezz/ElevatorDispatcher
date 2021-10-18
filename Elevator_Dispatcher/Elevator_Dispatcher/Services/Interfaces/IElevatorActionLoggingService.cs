using Elevator_Dispatcher.Models;

namespace Elevator_Dispatcher.Services
{
    public interface IElevatorActionLoggingService
    {
        public ElevatorActionResult LogEvent(ElevatorModel elevator, string subject);
        public string GetElevatorActionLog(ElevatorModel elevator);
        public string GetActionTimeStampLog(ElevatorModel elevator, long timeStamp);
    }
}