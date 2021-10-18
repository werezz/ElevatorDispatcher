using Elevator_Dispatcher.Models;
using Elevator_Dispatcher.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Elevator_Dispatcher.Controllers
{
    [ApiController]
    [Route("elevator")]
    public class ElevatorDispatcherController : ControllerBase
    {
        private readonly ILogger<ElevatorDispatcherController> _logger;
        private readonly IElevatorRoutingService _elevatorRoutingService;
        private readonly IElevatorPoolService _elevatorPoolService;
        private readonly IElevatorActionLoggingService _elevatorActionLoggingService;
        private readonly IElevatorStatusService _elevatorStatusService;

        public ElevatorDispatcherController(ILogger<ElevatorDispatcherController> logger,
            IElevatorRoutingService elevatorRoutingService,
            IElevatorPoolService elevatorPoolService,
            IElevatorActionLoggingService elevatorActionLoggingService,
            IElevatorStatusService elevatorStatusService)
        {
            _logger = logger;
            _elevatorRoutingService = elevatorRoutingService;
            _elevatorPoolService = elevatorPoolService;
            _elevatorActionLoggingService = elevatorActionLoggingService;
            _elevatorStatusService = elevatorStatusService;
        }

        [Route("call")]
        [HttpGet]
        public IActionResult CallElevator(int start, int end)
        {
            ElevatorActionResult result;
            try
            {
                result = _elevatorRoutingService.InitiateRoute(start, end);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to call elevator");
                return BadRequest("Unable to call elevator");
            }

            if ((result.Message.Equals(ElevatorActionResult.NotMove.Message)))
                return BadRequest(result?.Message ?? "Unexpected result");

            return Ok(result.Message);
        }

        [Route("status")]
        [HttpGet]
        public IActionResult GetElevatorStatus(int id)
        {
            string status;
            try
            {
                var elevator = _elevatorPoolService.GetElevator(id);
                status = _elevatorStatusService.GetStatus(elevator);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get elevator status");
                return BadRequest("Unable to get elevator status");
            }

            return Ok(status);
        }

        [Route("events")]
        [HttpGet]
        public IActionResult GetElevatorEventLog(int id)
        {
            string eventLog;
            try
            {
                var elevator = _elevatorPoolService.GetElevator(id);
                eventLog = _elevatorActionLoggingService.GetElevatorActionLog(elevator);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get elevator events");
                return BadRequest("Unable to get elevator events");
            }

            return Ok(eventLog);
        }

        [Route("stamp")]
        [HttpGet]
        public IActionResult GetElevatorEventLog(int id, long timeStamp)
        {
            string eventLog;
            try
            {
                var elevator = _elevatorPoolService.GetElevator(id);
                eventLog = _elevatorActionLoggingService.GetActionTimeStampLog(elevator, timeStamp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get elevator events");
                return BadRequest("Unable to get elevator events");
            }

            return Ok(eventLog);
        }
    }
}
