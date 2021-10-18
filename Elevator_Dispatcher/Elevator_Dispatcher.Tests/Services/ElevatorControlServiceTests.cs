using Elevator_Dispatcher.Models;
using Elevator_Dispatcher.Services;
using Moq;
using NUnit.Framework;
using System;

namespace Elevator_Dispatcher.Tests
{
    public class ElevatorControlServiceTests
    {
        private Mock<IElevatorActionLoggingService> _elevatorActionLoggingService;
        private Mock<IElevatorRoutingValidationService> _elevatorRoutingValidationService;
        private ElevatorControlService _sut;

        [SetUp]
        public void Setup()
        {
            _elevatorActionLoggingService = new Mock<IElevatorActionLoggingService>();

            _elevatorRoutingValidationService = new Mock<IElevatorRoutingValidationService>();

            _sut = new ElevatorControlService(_elevatorActionLoggingService.Object, _elevatorRoutingValidationService.Object);
        }

        [Test]
        public void GoToFloor_WhenNullElevatorProvided_Throws_Exception()
        {  
            Assert.Throws<ArgumentNullException>(() => _sut.GoToFloor(null, 0));
        }

        [Test]
        public void GoToFloor_WhenIncorrectFloorRangeProvided_Throws_Exception()
        {
            // Arrange
            _elevatorRoutingValidationService
                .Setup(x => x.IsFloorNumberCorrect(It.IsAny<int>()))
                    .Returns(false);

            // Assert
            Assert.AreEqual(ElevatorActionResult.InvalidFloor.Message, _sut.GoToFloor(new ElevatorModel(), 0).Message);
        }

        [Test]
        public void GoToFloor_WhenArgumentsAreFine_MovesElevatorUp()
        {
            // Arrange
            _elevatorRoutingValidationService
                .Setup(x => x.IsFloorNumberCorrect(It.IsAny<int>()))
                    .Returns(true);

            var elevator = new ElevatorModel
                {
                    CurrentFloor = 1
                };

            // Act
            _sut.GoToFloor(elevator, 2);

            // Assert
            Assert.AreEqual(2, elevator.CurrentFloor);
            _elevatorActionLoggingService.Verify(x => x.LogEvent(It.IsAny<ElevatorModel>(), It.IsAny<string>()), Times.Once);
         }

        [Test]
        public void GoToFloor_WhenArgumentsAreFine_MovesElevatorDown()
        {
            // Arrange
            _elevatorRoutingValidationService
                .Setup(x => x.IsFloorNumberCorrect(It.IsAny<int>()))
                    .Returns(true);

            var elevator = new ElevatorModel
                {
                    CurrentFloor = 2
                };

            // Act
            _sut.GoToFloor(elevator, 1);

            // Assert
            Assert.AreEqual(1, elevator.CurrentFloor);
            _elevatorActionLoggingService.Verify(x => x.LogEvent(It.IsAny<ElevatorModel>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GoToFloor_WhenTargetFloorIsCurrentFloor_DoesNotMoveElevator()
        {
            // Arrange
            _elevatorRoutingValidationService
                .Setup(x => x.IsFloorNumberCorrect(It.IsAny<int>()))
                    .Returns(true);

            var elevator = new ElevatorModel
            {
                CurrentFloor = 2
            };

            // Act
            _sut.GoToFloor(elevator, 2);

            // Assert
            Assert.AreEqual(2, elevator.CurrentFloor);
        }

        [Test]
        public void LockDoor_WhenNullProvided_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.LockDoor(null));
        }

        [Test]
        public void LockDoor_WhenDoorUnlocked_LocksDoor()
        {
            // Arrange
            var elevator = new ElevatorModel
            {
                IsDoorLocked = false
            };

            // Act
            _sut.LockDoor(elevator);

            // Assert
            Assert.IsTrue(elevator.IsDoorLocked);
            _elevatorActionLoggingService.Verify(x => x.LogEvent(It.IsAny<ElevatorModel>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void LockDoor_WhenDoorLocked_LocksDoor()
        {
            // Arrange
            var elevator = new ElevatorModel
            {
                IsDoorLocked = true
            };

            // Act
            _sut.LockDoor(elevator);

            // Assert
            Assert.IsTrue(elevator.IsDoorLocked);
            _elevatorActionLoggingService.Verify(x => x.LogEvent(It.IsAny<ElevatorModel>(), It.IsAny<string>()), Times.Once);
         }

        [Test]
        public void UnlockDoorAsync_WhenNullProvided_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.UnlockDoor(null));
        }

        [Test]
        public void UnlockDoor_WhenDoorLocked_UnlocksDoor()
        {
            // Arrange
            var elevator = new ElevatorModel
                {
                    IsDoorLocked = true
                };

            // Act
            _sut.UnlockDoor(elevator);

            // Assert
            Assert.IsFalse(elevator.IsDoorLocked);
            _elevatorActionLoggingService.Verify(x => x.LogEvent(It.IsAny<ElevatorModel>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void UnlockDoor_WhenDoorUnlocked_UnlocksDoor()
        {
            // Arrange
            var elevator = new ElevatorModel
                {
                    IsDoorLocked = false
                };

            // Act
            _sut.UnlockDoor(elevator);

            // Assert
            Assert.IsFalse(elevator.IsDoorLocked);
            _elevatorActionLoggingService.Verify(x => x.LogEvent(It.IsAny<ElevatorModel>(), It.IsAny<string>()), Times.Once);
        }
    }
}