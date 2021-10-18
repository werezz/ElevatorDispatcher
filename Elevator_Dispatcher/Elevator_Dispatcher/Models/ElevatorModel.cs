using System.Collections.Generic;

namespace Elevator_Dispatcher.Models
{
    public class ElevatorModel
    {
        public ElevatorModel(ElevatorModel elevator)
        {
            Id = elevator.Id;
            IsDoorLocked = elevator.IsDoorLocked;
            IsMoving = elevator.IsMoving;
            IsGoingUp = elevator.IsGoingUp;
            CurrentFloor = elevator.CurrentFloor;
            Actions = elevator.Actions;
        }

        public ElevatorModel()
        {
        }

        public int Id { get; set; }
        public bool IsDoorLocked { get; set; }
        public bool IsMoving { get; set; }
        public bool IsGoingUp { get; set; }
        public int CurrentFloor { get; set; } = Constants.FirstFloorNumber;
        public IList<ElevatorAction> Actions { get; set; } = new List<ElevatorAction>();
        public string MovementDirection => IsMoving ? (IsGoingUp ? "Up" : "Down") : "None";
    }
}
