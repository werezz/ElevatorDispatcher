
namespace Elevator_Dispatcher.Models
{
    public class ElevatorAction
    {
        public long TimeStamp { get; set; }
        public string Subject { get; set; }

        public override string ToString()
        {
            return $"{TimeStamp}   {Subject}";
        }
    }
}
