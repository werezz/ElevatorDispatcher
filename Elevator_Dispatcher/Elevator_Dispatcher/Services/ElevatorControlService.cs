using Elevator_Dispatcher.Models;
using System;
using System.Threading;

namespace Elevator_Dispatcher.Services
{
    public class ElevatorControlService: IElevatorControlService
    {
        private readonly IElevatorActionLoggingService _elevatorActionLoggingService;
        private readonly IElevatorRoutingValidationService _elevatorRoutingValidationService;

        public ElevatorControlService(
            IElevatorActionLoggingService elevatorEventLogService, 
            IElevatorRoutingValidationService elevatorRoutingValidationService)
        {
            _elevatorActionLoggingService = elevatorEventLogService;
            _elevatorRoutingValidationService = elevatorRoutingValidationService;
        }

        public ElevatorActionResult GoToFloor(ElevatorModel elevator, int targetFloor)
        {
            if (elevator == null)
                throw new ArgumentNullException(nameof(elevator));

            if (!_elevatorRoutingValidationService.IsFloorNumberCorrect(targetFloor))
                return ElevatorActionResult.InvalidFloor;

            var floorsToGo = targetFloor - elevator.CurrentFloor;
            var isGoingUp = floorsToGo > 0;
            elevator.IsMoving = true;
            elevator.IsGoingUp = isGoingUp;

            while (elevator.CurrentFloor != targetFloor)
            {
                Thread.Sleep(Constants.MovementPerFloorTimeInMiliseconds);
                var previousFloor = elevator.CurrentFloor;
                elevator.CurrentFloor += isGoingUp ? 1 : -1;

                _elevatorActionLoggingService.LogEvent(elevator, $"Changed floor from {previousFloor} to {elevator.CurrentFloor}");
            }

            elevator.IsMoving = false;

            return ElevatorActionResult.FinishedRoute;
        }

        public ElevatorActionResult LockDoor(ElevatorModel elevator)
        {
            if (elevator == null)
                throw new ArgumentNullException(nameof(elevator));

            elevator.IsDoorLocked = true;
            _elevatorActionLoggingService.LogEvent(elevator, "Door locked");

            Thread.Sleep(Constants.DoorOperationTimeInMiliseconds);

            return ElevatorActionResult.Locked;
        }

        public ElevatorActionResult UnlockDoor(ElevatorModel elevator)
        {
            if (elevator == null)
                throw new ArgumentNullException(nameof(elevator));

            Thread.Sleep(Constants.DoorOperationTimeInMiliseconds);
            elevator.IsDoorLocked = false;

            _elevatorActionLoggingService.LogEvent(elevator, "Door unlocked");

            return ElevatorActionResult.Unlocked;
        }
    }
}
