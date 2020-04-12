using Lego.Core;
using Lego.Core.Models.Messaging;
using Lego.Core.Models.Messaging.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Lego.Core.Extensions;

namespace Lego.Tests
{
    [TestClass]
    public class ByteTests
    {
        [TestMethod]
        public void AngularVelocity()
        {
            byte speed1 = 1;
            byte speed2 = 50;
            byte speed3 = 100;
            byte speed4 = byte.MinValue;
            byte speed5 = byte.MaxValue;

            var velocity1A = speed1.AsAngularVelocity(RotateDirection.Clockwise);
            var velocity1B = speed1.AsAngularVelocity(RotateDirection.CounterClockwise);
            var velocity2A = speed2.AsAngularVelocity(RotateDirection.Clockwise);
            var velocity2B = speed2.AsAngularVelocity(RotateDirection.CounterClockwise);
            var velocity3A = speed3.AsAngularVelocity(RotateDirection.Clockwise);
            var velocity3B = speed3.AsAngularVelocity(RotateDirection.CounterClockwise);
            var velocity4A = speed4.AsAngularVelocity(RotateDirection.Clockwise);
            var velocity4B = speed4.AsAngularVelocity(RotateDirection.CounterClockwise);
            var velocity5A = speed5.AsAngularVelocity(RotateDirection.Clockwise);
            var velocity5B = speed5.AsAngularVelocity(RotateDirection.CounterClockwise);

            Assert.AreEqual<byte>(1, velocity1A);
            Assert.AreEqual<byte>(0b11111111, velocity1B);
            Assert.AreEqual<byte>(50, velocity2A);
            Assert.AreEqual<byte>(0b11001110, velocity2B);
            Assert.AreEqual<byte>(100, velocity3A);
            Assert.AreEqual<byte>(0b10011100, velocity3B);
            Assert.AreEqual<byte>(0, velocity4A);
            Assert.AreEqual<byte>(0, velocity4B);
            Assert.AreEqual<byte>(100, velocity5A);
            Assert.AreEqual<byte>(0b10011100, velocity5B);
        }
    }

    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        public void MessageLengths()
        {
            var message1 = new Message(new byte[] { 0b01010101, 0b11111111 });
            var message2 = new Message(new byte[] { 0b01111111, 0b11111111 });
            var message3 = new Message(new byte[] { 0b10000000, 0b00000001 });
            var message4 = new Message(new byte[] { 0b10000001, 0b00000001 });

            Assert.AreEqual(85, message1.MessageLength);
            Assert.AreEqual(127, message2.MessageLength);
            Assert.AreEqual(128, message3.MessageLength);
            Assert.AreEqual(129, message4.MessageLength);
        }

        [TestMethod]
        public void MessageTypes()
        {
            var message1 = new Message(new byte[] { 0b00000011, 0b00000000, 0b00000100 });

            Assert.AreEqual(MessageType.Hub__Attached_IO, message1.MessageType);
        }

        [TestMethod]
        public void MessageBody()
        {
            var message1 = new Message(new byte[] { 0b00000100, 0b00000000, 0b00000100, 0b00000001, 0b00000000 });

            var body = message1.Body;

            Assert.AreEqual(1, body.Count());
            Assert.AreEqual(0b00000001, message1.Body.Single());
        }
    }
}
