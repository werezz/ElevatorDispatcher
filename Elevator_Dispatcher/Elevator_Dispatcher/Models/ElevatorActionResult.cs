
namespace Elevator_Dispatcher.Models
{
    public class ElevatorActionResult
    {
        public string Message { get; private set; }

        private ElevatorActionResult(string message) { Message = message; }

        public static ElevatorActionResult Move { get { return new ElevatorActionResult("Elevator is moving"); } }
        public static ElevatorActionResult NotMove { get { return new ElevatorActionResult("Elevator is not moving"); } }
        public static ElevatorActionResult Idle { get { return new ElevatorActionResult("Elevator is idle"); } }
        public static ElevatorActionResult FinishedRoute { get { return new ElevatorActionResult("Elevator finished route"); } }
        public static ElevatorActionResult Locked { get { return new ElevatorActionResult("Elevator door locked"); } }
        public static ElevatorActionResult Unlocked { get { return new ElevatorActionResult("Elevator door unlocked"); } }
        public static ElevatorActionResult Realesed { get { return new ElevatorActionResult("Elevator is not used anymore"); } }
        public static ElevatorActionResult Created { get { return new ElevatorActionResult("All elevators are created"); } }
        public static ElevatorActionResult Logged { get { return new ElevatorActionResult("Elevator action logged"); } }
        public static ElevatorActionResult InvalidFloor { get { return new ElevatorActionResult("Invalid floor provided"); } }
    }
}
