using Elevator_Dispatcher.Models;

namespace Elevator_Dispatcher.Services
{
    public class ElevatorRoutingService : IElevatorRoutingService
    {
        private readonly IElevatorPoolService _elevatorPoolService;
        private readonly IElevatorControlService _elevatorControlService;
        private readonly IElevatorRoutingValidationService _elevatorRoutingValidationService;

        public ElevatorRoutingService(
            IElevatorPoolService elevatorPoolService,
            IElevatorControlService elevatorControlService,
            IElevatorRoutingValidationService elevatorRoutingValidationService)
        {
            _elevatorPoolService = elevatorPoolService;
            _elevatorControlService = elevatorControlService;
            _elevatorRoutingValidationService = elevatorRoutingValidationService;
        }

        public ElevatorActionResult InitiateRoute(int startFloor, int targetFloor)
        {
            if (!_elevatorRoutingValidationService.IsRouteCorrect(startFloor, targetFloor))
            {
                return ElevatorActionResult.NotMove;
            }

            if (startFloor == targetFloor)
                return  ElevatorActionResult.Idle;

            var elevatorModel = _elevatorPoolService.TakeClosestElevator(startFloor);

            if (elevatorModel == null)
            {
                return ElevatorActionResult.NotMove;
            }

            PerformRoute(elevatorModel, startFloor, targetFloor);

            return ElevatorActionResult.Move;
        }

        private ElevatorActionResult PerformRoute(ElevatorModel elevator, int startFloor, int targetFloor)
        {
            PerformSingleWayRoute(elevator, startFloor);
            PerformSingleWayRoute(elevator, targetFloor);

            _elevatorPoolService.ReleaseElevator(elevator);

            return ElevatorActionResult.FinishedRoute;
        }

        private ElevatorActionResult PerformSingleWayRoute(ElevatorModel elevator, int targetFloor)
        {
            if (elevator.CurrentFloor == targetFloor)
                return ElevatorActionResult.FinishedRoute;

             _elevatorControlService.LockDoor(elevator);

             _elevatorControlService.GoToFloor(elevator, targetFloor);

             _elevatorControlService.UnlockDoor(elevator);

            return ElevatorActionResult.FinishedRoute;
        }
    }
}
