using Elevator_Dispatcher.Services;
using NUnit.Framework;

namespace Elevator_Dispatcher.Tests
{
    class ElevatorRoutingValidationServiceTests
    {
        private ElevatorRoutingValidationService _sut;

        [SetUp]
        public void Initialize()
        {
            _sut = new ElevatorRoutingValidationService();
        }

        [Test]
        public void IsFloorNumberCorrect_WhenCorrect_ReturnsTrue()
        {
            // Act
            var result = _sut.IsFloorNumberCorrect(1);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsFloorNumberCorrect_WhenIncorrect_ReturnsFalse()
        {
            // Act
            var result = _sut.IsFloorNumberCorrect(100000000);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsRouteCorrect_WhenCorrect_ReturnsTrue()
        {
            // Act
            var result = _sut.IsRouteCorrect(1, 2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsRouteCorrect_WhenIncorrectStart_ReturnsFalse()
        {
            // Act
            var result = _sut.IsRouteCorrect(100000000, 10);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void IsRouteCorrect_WhenIncorrectEnd_ReturnsFalse()
        {
            // Act
            var result = _sut.IsRouteCorrect(1, 100000000);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
