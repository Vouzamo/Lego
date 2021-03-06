﻿using System.Threading.Tasks;

namespace Lego.Core.Models.Devices.Hubs
{
    public class TechnicSmartHub : Hub
    {
        public TechnicSmartHub(IConnection connection) : base(connection)
        {

        }

        public async Task<T> PortA<T>() where T : IDevice
        {
            return await EstablishDeviceConnectionByPort<T>(0b00);
        }

        public async Task<T> PortB<T>() where T : IDevice
        {
            return await EstablishDeviceConnectionByPort<T>(0b01);
        }

        public async Task<T> PortC<T>() where T : IDevice
        {
            return await EstablishDeviceConnectionByPort<T>(0b10);
        }

        public async Task<T> PortD<T>() where T : IDevice
        {
            return await EstablishDeviceConnectionByPort<T>(0b11);
        }
    }
}
