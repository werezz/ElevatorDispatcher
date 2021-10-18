using Elevator_Dispatcher.Controllers;
using Elevator_Dispatcher.Models;
using Elevator_Dispatcher.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;


namespace Elevator_Dispatcher.Tests.Controllers
{
    class ElevatorDispatcherControllerTests
    {
        private string _exceptionMessage = "test purposes exception";

        private Mock<ILogger<ElevatorDispatcherController>> _loggerMock;
        private Mock<IElevatorRoutingService> _elevatorRoutingService;
        private Mock<IElevatorPoolService> _elevatorPoolService;
        private Mock<IElevatorActionLoggingService> _elevatorActionLoggingService;
        private Mock<IElevatorStatusService> _elevatorStatusService;

        private ElevatorDispatcherController _sut;

        [SetUp]
        public void Intialize()
        {
            _loggerMock = new Mock<ILogger<ElevatorDispatcherController>>();
            _elevatorRoutingService = new Mock<IElevatorRoutingService>();
            _elevatorPoolService = new Mock<IElevatorPoolService>();
            _elevatorActionLoggingService = new Mock<IElevatorActionLoggingService>();
            _elevatorStatusService = new Mock<IElevatorStatusService>();

            _sut = new ElevatorDispatcherController(
                _loggerMock.Object,
                _elevatorRoutingService.Object,
                _elevatorPoolService.Object,
                _elevatorActionLoggingService.Object,
                _elevatorStatusService.Object);
        }

        [Test]
        public void CallElevator_ReturnsBadRequest_WhenInitiateRouteThrows()
        {
            // Arrange
            _elevatorRoutingService
                .Setup(x => x.InitiateRoute(It.IsAny<int>(), It.IsAny<int>()))
                .Throws(new InvalidOperationException(_exceptionMessage));

            // Act
            var result = _sut.CallElevator(1, 2) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void CallElevator_ReturnsBadRequest_WhenInitiateRouteResultIsNotStarted()
        {
            // Arrange
            _elevatorRoutingService
                .Setup(x => x.InitiateRoute(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(ElevatorActionResult.NotMove);

            // Act
            var result = _sut.CallElevator(1, 2) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void CallElevator_ReturnsBadRequest_WhenInitiateRouteResultIsNotNeeded()
        {
            // Arrange
            _elevatorRoutingService
                .Setup(x => x.InitiateRoute(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(ElevatorActionResult.NotMove);

            // Act
            var result = _sut.CallElevator(1, 2) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void CallElevator_ReturnsOk_WhenInitiateRouteResultIsStarted()
        {
            // Arrange
            _elevatorRoutingService
                .Setup(x => x.InitiateRoute(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(ElevatorActionResult.Move);

            // Act
            var result = _sut.CallElevator(1, 2) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetElevatorStatus_ReturnsBadRequest_WhenPoolServiceThrows()
        {
            // Arrange
            _elevatorPoolService
                .Setup(x => x.GetElevator(It.IsAny<int>()))
                .Throws(new InvalidOperationException(_exceptionMessage));

            // Act
            var result = _sut.GetElevatorStatus(1) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetElevatorStatus_ReturnsBadRequest_WhenStatusServiceThrows()
        {
            // Arrange
            _elevatorStatusService
                .Setup(x => x.GetStatus(It.IsAny<ElevatorModel>()))
                .Throws(new InvalidOperationException(_exceptionMessage));

            // Act
            var result = _sut.GetElevatorStatus(1) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetElevatorStatus_ReturnsOk_WhenStatusServiceReturns()
        {
            // Arrange
            _elevatorStatusService
                .Setup(x => x.GetStatus(It.IsAny<ElevatorModel>()))
                .Returns("status string");

            // Act
            var result = _sut.GetElevatorStatus(1) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("status string", result.Value);
        }

        [Test]
        public void GetElevatorEventLog_ReturnsBadRequest_WhenPoolServiceThrows()
        {
            // Arrange
            _elevatorPoolService
                .Setup(x => x.GetElevator(It.IsAny<int>()))
                .Throws(new InvalidOperationException(_exceptionMessage));

            // Act
            var result = _sut.GetElevatorEventLog(1) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetElevatorEventLog_ReturnsBadRequest_WhenEventServiceThrows()
        {
            // Arrange
            _elevatorActionLoggingService
                .Setup(x => x.GetElevatorActionLog(It.IsAny<ElevatorModel>()))
                .Throws(new InvalidOperationException(_exceptionMessage));

            // Act
            var result = _sut.GetElevatorEventLog(1) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetElevatorEventLog_ReturnsOk_WhenEventServiceReturns()
        {
            // Arrange
            _elevatorActionLoggingService
                .Setup(x => x.GetElevatorActionLog(It.IsAny<ElevatorModel>()))
                .Returns("action log");

            // Act
            var result = _sut.GetElevatorEventLog(1) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("action log", result.Value);
        }

        [Test]
        public void GetElevatorEventLog_WhithTimeStamp_ReturnsOk_WhenEventServiceReturns()
        {
            // Arrange
            _elevatorActionLoggingService
                .Setup(x => x.GetActionTimeStampLog(It.IsAny<ElevatorModel>(), It.IsAny<long>()))
                .Returns("action timestamp log");

            // Act
            var result = _sut.GetElevatorEventLog(1,1) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("action timestamp log", result.Value);
        }
    }
}
