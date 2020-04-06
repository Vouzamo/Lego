using Lego.Core;
using Lego.Core.Models.Messaging;
using Lego.Core.Models.Messaging.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Lego.Tests
{
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
