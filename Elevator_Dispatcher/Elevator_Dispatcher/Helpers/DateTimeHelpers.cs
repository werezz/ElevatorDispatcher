using System;

namespace Elevator_Dispatcher.Helpers
{
    public static class DateTimeHelpers
    {
        public static long GetCurrentTimeStamp()
        {
            return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }
    }
}
