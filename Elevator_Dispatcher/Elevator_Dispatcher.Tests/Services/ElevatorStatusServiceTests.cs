using Elevator_Dispatcher.Models;
using Elevator_Dispatcher.Services;
using NUnit.Framework;
using System;

namespace Elevator_Dispatcher.Tests
{
    class ElevatorStatusServiceTests
    {
        private ElevatorStatusService _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new ElevatorStatusService();
        }

        [Test]
        public void GetStatus_WhenNullProvided_ThrowsException()
        {       
            // Assert
            Assert.Throws<ArgumentNullException>(() => _sut.GetStatus(null));
        }

        [Test]
        public void GetStatus_WhenElevatorProvided_ShouldReturnStatus()
        {
            // Arrange
            var elevator = new ElevatorModel();

            // Act
            var result = _sut.GetStatus(elevator);

            // Assert
            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }
    }
}
