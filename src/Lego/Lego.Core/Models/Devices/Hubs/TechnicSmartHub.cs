using System.Threading.Tasks;

namespace Lego.Core.Models.Devices.Hubs
{
    public class TechnicSmartHub : Hub
    {
        public TechnicSmartHub(IConnection connection, string deviceId) : base(connection, deviceId)
        {

        }

        public async Task<T> PortA<T>() where T : IODevice
        {
            return await EstablishDeviceConnectionByPort<T>(0b00);
        }

        public async Task<T> PortB<T>() where T : IODevice
        {
            return await EstablishDeviceConnectionByPort<T>(0b01);
        }

        public async Task<T> PortC<T>() where T : IODevice
        {
            return await EstablishDeviceConnectionByPort<T>(0b10);
        }

        public async Task<T> PortD<T>() where T : IODevice
        {
            return await EstablishDeviceConnectionByPort<T>(0b11);
        }
    }
}
