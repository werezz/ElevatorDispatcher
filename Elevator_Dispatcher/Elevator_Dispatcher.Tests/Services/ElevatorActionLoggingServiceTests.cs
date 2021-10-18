using Elevator_Dispatcher.Models;
using Elevator_Dispatcher.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elevator_Dispatcher.Tests
{
    class ElevatorActionLoggingServiceTests
    {
        private ElevatorActionLoggingService _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new ElevatorActionLoggingService();
        }

        [Test]
        public void LogAction_WhenNullProvided_ThrowsException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => _sut.LogEvent(null, string.Empty));
        }

        [Test]
        public void LogAction_WhenArgumentsProvided_LogsNewAction()
        {
            // Arrange
            var elevator = new ElevatorModel();
            var subject = "Test purposes subject";

            // Act
            _sut.LogEvent(elevator, subject);

            // Assert
            Assert.AreEqual("Test purposes subject", elevator.Actions.First().Subject);
            Assert.AreNotEqual(0L, elevator.Actions.First().TimeStamp);
        }

        [Test]
        public void GetElevatorActionLog_WhenNullProvided_ThrowsException()
        {           
            // Assert
            Assert.Throws<ArgumentNullException>(() => _sut.GetElevatorActionLog(null));
        }

        [Test]
        public void GetElevatorActionLog_WhenElevatorProvided_ReturnsEventLog()
        {
            // Arrange
            var elevator = new ElevatorModel()
            {
                Actions = new List<ElevatorAction>
                {
                    new ElevatorAction
                    {
                        Subject = "Test",
                        TimeStamp = 1,
                    }
                }
            };

            // Act
            var result = _sut.GetElevatorActionLog(elevator);

            // Assert
            Assert.IsTrue(result.Contains("1") && result.Contains("Test"));
        }

        [Test]
        public void GetActionTimeStampLog_WhenElevatorProvided_ReturnsEventLog()
        {
            // Arrange
            var elevator = new ElevatorModel()
            {
                Actions = new List<ElevatorAction>
                {
                    new ElevatorAction
                    {
                        Subject = "Test",
                        TimeStamp = 1,
                    }
                }
            };

            // Act
            var result = _sut.GetActionTimeStampLog(elevator,elevator.Actions[0].TimeStamp);

            // Assert
            Assert.IsTrue(result.Contains("1") && result.Contains("Test"));
        }

    }
}
