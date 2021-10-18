using Elevator_Dispatcher.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Elevator_Dispatcher.Services
{
    public class ElevatorPoolService : IElevatorPoolService
    {
        private readonly Dictionary<int, ElevatorModel> allElevators = new Dictionary<int, ElevatorModel>();
        private readonly ConcurrentDictionary<int, ElevatorModel> freeElevators = new ConcurrentDictionary<int, ElevatorModel>();
        private readonly ConcurrentDictionary<int, ElevatorModel> occupiedElevators = new ConcurrentDictionary<int, ElevatorModel>();

        private readonly IElevatorActionLoggingService _elevatorActionLoggingService;
        private readonly IElevatorRoutingValidationService _elevatorRoutingValidationService;

        public ElevatorPoolService(
            IElevatorActionLoggingService elevatorActionLoggingService,
            IElevatorRoutingValidationService elevatorRoutingValidationService)
        {
            InitializeElevators(Constants.NumberOfElevators);
            _elevatorActionLoggingService = elevatorActionLoggingService;
            _elevatorRoutingValidationService = elevatorRoutingValidationService;
        }

        public ElevatorModel GetElevator(int id)
        {
            if (!allElevators.ContainsKey(id))
                throw new ArgumentOutOfRangeException(nameof(id));

            return new ElevatorModel(allElevators[id]);
        }

        public ElevatorModel TakeClosestElevator(int floor)
        {
            if (!_elevatorRoutingValidationService.IsFloorNumberCorrect(floor))
                throw new ArgumentOutOfRangeException(nameof(floor));

                var orderedElevators = freeElevators
                    .OrderBy(p => Math.Abs(p.Value.CurrentFloor - floor));

                if (!orderedElevators.Any())
                    return null;

                var closestElevatorEntry = orderedElevators.First();

                freeElevators.TryRemove(closestElevatorEntry.Key, out var closestElevator);
                occupiedElevators.TryAdd(closestElevatorEntry.Key, closestElevator);

                _elevatorActionLoggingService.LogEvent(closestElevator, "Called elevator");

                return closestElevator;
        }

        public ElevatorActionResult ReleaseElevator(ElevatorModel elevator)
        {
            if (!allElevators.ContainsKey(elevator.Id))
                throw new ArgumentOutOfRangeException(nameof(elevator));

                var result = true;

                result &= occupiedElevators.TryRemove(elevator.Id, out elevator);
                result &= freeElevators.TryAdd(elevator.Id, elevator);

                if (result)
                    _elevatorActionLoggingService.LogEvent(elevator, "Elevator is free");

            return ElevatorActionResult.Realesed;
        }

       private ElevatorActionResult InitializeElevators(int numberOfElevators)
        {
            for (var i = 1; i <= numberOfElevators; i++)
            {
                var elevator = new ElevatorModel
                {
                    Id = i
                };

                allElevators.Add(elevator.Id, elevator);
                freeElevators.TryAdd(elevator.Id, elevator);
            }

            return ElevatorActionResult.Created;
        }
    }
}
