namespace Elevator_Dispatcher.Services
{
    public interface IElevatorRoutingValidationService
    {
        bool IsRouteCorrect(int start, int end);
        bool IsFloorNumberCorrect(int floorNumber);
    }
}