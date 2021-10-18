using Elevator_Dispatcher.Models;
using System;

namespace Elevator_Dispatcher.Services
{
    public class ElevatorStatusService : IElevatorStatusService
    {
        public string GetStatus(ElevatorModel elevator)
        {
            if (elevator is null)
                throw new ArgumentNullException(nameof(elevator));

            return $"ElevatorId = {elevator.Id} \n IsMoving = {elevator.IsMoving} \n CurrentFloor {elevator.CurrentFloor} \n Direction : {elevator.MovementDirection}";
        }
    }
}
