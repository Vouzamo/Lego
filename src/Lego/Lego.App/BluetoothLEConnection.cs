using Lego.Core;
using Lego.Core.Models.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Security.Cryptography;

namespace Lego.App
{
    public class BluetoothLEConnection : IConnection
    {
        protected Hub ConnectedHub { get; set; }
        protected BluetoothLEDevice Device { get; set; }

        public BluetoothLEConnection(BluetoothLEDevice device)
        {
            Device = device;
        }

        public async Task Connect(Hub hub)
        {
            var service = await GetService();

            if (service.Session.CanMaintainConnection)
            {
                service.Session.MaintainConnection = true;
            }

            ConnectedHub = hub;

            var characteristic = await GetCharacteristic(service);

            characteristic.ValueChanged += Characteristic_ValueChanged;
            
            var result = await characteristic.WriteClientCharacteristicConfigurationDescriptorWithResultAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
        }

        private void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            var buffer = args.CharacteristicValue;

            CryptographicBuffer.CopyToByteArray(buffer, out byte[] bytes);

            var message = new Message(bytes);

            ConnectedHub.ReceiveMessage(message);
        }

        public async Task<GattDeviceService> GetService()
        {
            var services = await Device.GetGattServicesAsync();
            var service = services.Services.Single(s => s.Uuid.Equals(new Guid("00001623-1212-efde-1623-785feabcd123")));

            return service;
        }

        public async Task<GattCharacteristic> GetCharacteristic(GattDeviceService service)
        {
            var characteristics = await service.GetCharacteristicsAsync();
            var characteristic = characteristics.Characteristics.Single(c => c.Uuid == new Guid("00001624-1212-efde-1623-785feabcd123"));

            return characteristic;
        }

        public async void SendMessage(IMessage message)
        {
            var buffer = CryptographicBuffer.CreateFromByteArray(message.Bytes.ToArray());

            var service = await GetService();
            var characteristic = await GetCharacteristic(service);
            
            var result = await characteristic.WriteValueWithResultAsync(buffer);
        }
    }

    public class BluetoothLEConnectionManager : IConnectionManager
    {
        public BluetoothLEConnectionManager()
        {

        }

        public async Task<T> EstablishHubConnectionById<T>(string deviceId) where T : Hub, new()
        {
            var hub = new T();

            var watcher = new BluetoothLEAdvertisementWatcher
            {
                ScanningMode = BluetoothLEScanningMode.Active
            };

            watcher.Received += async (w, btAdv) => {
                var device = await BluetoothLEDevice.FromBluetoothAddressAsync(btAdv.BluetoothAddress);
                
                if(device != null)
                {
                    Debug.WriteLine($"BLEWATCHER Found: {device.DeviceId}");

                    if (device.DeviceId == deviceId)
                    {
                        var connection = new BluetoothLEConnection(device);

                        hub.Connect(connection);
                    }
                }
            };

            watcher.Start();

            while(!hub.IsConnected)
            {
                await Task.Delay(500);
            }

            watcher.Stop();

            return hub;
        }
    }
}
