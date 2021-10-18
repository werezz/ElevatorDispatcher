using Elevator_Dispatcher.Helpers;
using Elevator_Dispatcher.Models;
using System;
using System.Text;

namespace Elevator_Dispatcher.Services
{
    public class ElevatorActionLoggingService : IElevatorActionLoggingService
    {
        public ElevatorActionResult LogEvent(ElevatorModel elevator, string subject)
        {
            if (elevator == null)
                throw new ArgumentNullException();

            elevator.Actions.Add(new ElevatorAction
            {
                Subject = subject,
                TimeStamp = DateTimeHelpers.GetCurrentTimeStamp()
            });

            return ElevatorActionResult.Logged;
        }

        public string GetElevatorActionLog(ElevatorModel elevator)
        {
            if (elevator == null)
                throw new ArgumentNullException();

            var sb = new StringBuilder();

            foreach (var ev in elevator.Actions)
            {
                sb.AppendLine(ev.ToString());
            }

            return sb.ToString();
        }

        public string GetActionTimeStampLog(ElevatorModel elevator, long timesStamp)
        {
            if (elevator == null)
                throw new ArgumentNullException();

            var sb = new StringBuilder();

            foreach (var ev in elevator.Actions)
            {
                if (ev.TimeStamp.Equals(timesStamp))
                {
                    sb.AppendLine(ev.ToString());
                }
            }

            return sb.ToString();
        }
    }
}
