using Elevator_Dispatcher.Models;
using Elevator_Dispatcher.Services;
using Moq;
using NUnit.Framework;

namespace Elevator_Dispatcher.Tests
{
    class ElevatorRoutingServiceTests
    {
        private Mock<IElevatorPoolService> _elevatorPoolService;
        private Mock<IElevatorControlService> _elevatorControlService;
        private Mock<IElevatorRoutingValidationService> _elevatorRoutingValidationService;
        private ElevatorRoutingService _sut;

        [SetUp]
        public void Setup()
        {
            _elevatorPoolService = new Mock<IElevatorPoolService>();
            _elevatorControlService = new Mock<IElevatorControlService>();
            _elevatorRoutingValidationService = new Mock<IElevatorRoutingValidationService>();

            _sut = new ElevatorRoutingService(
                 _elevatorPoolService.Object,
                 _elevatorControlService.Object,
                 _elevatorRoutingValidationService.Object);
        }

        [Test]
        public void InitiateRoute_WhenRouteIncorrect_ReturnsMovementNotStartedResult()
        {
            // Arrange
            _elevatorRoutingValidationService
                .Setup(x => x.IsRouteCorrect(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(false);

            // Assert
            Assert.AreEqual(ElevatorActionResult.NotMove.Message, _sut.InitiateRoute(500, 600).Message);
        }

        [Test]
        public void InitiateRoute_WhenRouteIsRedundant_ReturnElevatorActionIdle()
        {
            // Arrange
            _elevatorRoutingValidationService
                .Setup(x => x.IsRouteCorrect(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);
;
            // Assert
            Assert.AreEqual(ElevatorActionResult.Idle.Message, _sut.InitiateRoute(1, 1).Message);
        }

        [Test]
        public void InitiateRoute_WhenNoElevatorAvailable_ReturnElevatorActionNotMove()
        {
            // Arrange
            _elevatorRoutingValidationService
                .Setup(x => x.IsRouteCorrect(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);

            _elevatorPoolService
                .Setup(x => x.TakeClosestElevator(It.IsAny<int>()))
                .Returns((ElevatorModel)null);

            // Assert
            Assert.AreEqual(ElevatorActionResult.NotMove.Message, _sut.InitiateRoute(1, 2).Message);
        }

        [Test]
        public void InitiateRoute_WhenElevatorAvailable_ReturnElevatorActionMove()
        {
            // Arrange
            _elevatorRoutingValidationService
                .Setup(x => x.IsRouteCorrect(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);

            _elevatorPoolService
                .Setup(x => x.TakeClosestElevator(It.IsAny<int>()))
                .Returns(new ElevatorModel());
        
            // Assert
            Assert.AreEqual(ElevatorActionResult.Move.Message, _sut.InitiateRoute(1, 2).Message);
        }


    }
}
