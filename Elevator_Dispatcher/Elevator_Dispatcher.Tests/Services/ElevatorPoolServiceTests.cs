using Elevator_Dispatcher.Models;
using Elevator_Dispatcher.Services;
using Moq;
using NUnit.Framework;
using System;

namespace Elevator_Dispatcher.Tests
{
    class ElevatorPoolServiceTests
    {
        private Mock<IElevatorActionLoggingService> _elevatorActionLoggingService;
        private Mock<IElevatorRoutingValidationService> _elevatorRoutingValidationService;
        private ElevatorPoolService _sut;

        [SetUp]
        public void Setup()
        {
            _elevatorActionLoggingService = new Mock<IElevatorActionLoggingService>();

            _elevatorRoutingValidationService = new Mock<IElevatorRoutingValidationService>();

            _sut = new ElevatorPoolService(
                _elevatorActionLoggingService.Object,
                _elevatorRoutingValidationService.Object);

        }

        [Test]
        public void GetElevator_WhenInvalidIdProvided_ThrowsException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _sut.GetElevator(10));
        }

        [Test]
        public void GetElevator_WhenValidIdProvided_ReturnsElevator()
        {
            // Act
            var result = _sut.GetElevator(1);

            // Assert
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public void TakeClosestElevator_WhenInvalidFloorProvided_ThrowsException()
        {
            // Arrange
            _elevatorRoutingValidationService
                .Setup(x => x.IsFloorNumberCorrect(It.IsAny<int>()))
                .Returns(false);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _sut.TakeClosestElevator(1));
        }

        [Test]
        public void TakeClosestElevator_WhenElevatorAvailable_ReturnsElevator()
        {
            // Arrange
            _elevatorRoutingValidationService
                .Setup(x => x.IsFloorNumberCorrect(It.IsAny<int>()))
                .Returns(true);

            // Act
            var result = _sut.TakeClosestElevator(1);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]

        public void ReleaseElevator_WhenIncorrectIdProvided_ThrowsException()
        {
            // Act
            var elevator = new ElevatorModel
            {
                Id = 1000000000
            };

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _sut.ReleaseElevator(elevator));
        }

        [Test]
        public void ReleaseElevator_WhenElevatorIsOccupied_ReleasesElevator()
        {
            // Arrange
            _elevatorRoutingValidationService
                .Setup(x => x.IsFloorNumberCorrect(It.IsAny<int>()))
                .Returns(true);

            // Act
            var elevator = new ElevatorModel
            {
                Id = 1
            };

            
            _sut.TakeClosestElevator(1);

            // Assert
            Assert.AreEqual(ElevatorActionResult.Realesed.Message, _sut.ReleaseElevator(elevator).Message);
        }
    }
}
