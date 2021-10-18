using Elevator_Dispatcher.Models;

namespace Elevator_Dispatcher.Services
{
    public class ElevatorRoutingValidationService : IElevatorRoutingValidationService
    {
        public bool IsFloorNumberCorrect(int floorNumber)
        {
            var numberOfFloors = Constants.NumberOfFloors;

            if (floorNumber < Constants.FirstFloorNumber || floorNumber > numberOfFloors)
                return false;

            return true;
        }

        public bool IsRouteCorrect(int start, int end)
        {
            var numberOfFloors = Constants.NumberOfFloors;

            if (start < Constants.FirstFloorNumber || start > numberOfFloors)
                return false;

            if (end < Constants.FirstFloorNumber || end > numberOfFloors)
                return false;

            return true;
        }
    }
}
