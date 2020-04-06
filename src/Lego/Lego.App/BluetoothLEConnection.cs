using Lego.Core;
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
        protected List<BluetoothLEDevice> Devices { get; set; } = new List<BluetoothLEDevice>();

        public event EventHandler<IMessage> OnMessageReceived;

        public BluetoothLEConnection()
        {

        }

        public async Task<Hub> EstablishHubConnectionById(string deviceId)
        {
            bool keepLooking = true;

            var hub = new Hub(this, deviceId);

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
                        var services = await device.GetGattServicesAsync();
                        var service = services.Services.FirstOrDefault(s => s.Uuid.Equals(new Guid("00001623-1212-efde-1623-785feabcd123")));

                        var characteristics = await service.GetCharacteristicsAsync();
                        var characteristic = characteristics.Characteristics.FirstOrDefault(c => c.Uuid == new Guid("00001624-1212-efde-1623-785feabcd123"));

                        characteristic.ValueChanged += OnValueChanged;

                        var result = await characteristic.WriteClientCharacteristicConfigurationDescriptorWithResultAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);

                        Devices.Add(device);
                        keepLooking = false;
                    }
                }
            };

            watcher.Start();

            while(keepLooking)
            {
                await Task.Delay(500);
            }

            watcher.Stop();

            return hub;
        }

        private void OnValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            var buffer = args.CharacteristicValue;

            CryptographicBuffer.CopyToByteArray(buffer, out byte[] bytes);

            var message = new Message(bytes);

            var deviceId = sender.Service.DeviceId;

            var device = Devices.FirstOrDefault(d => d.DeviceId.Equals(deviceId));

            OnMessageReceived.Invoke(this, message);
        }

        public async void SendMessage(string deviceId, IMessage message)
        {
            var device = Devices.FirstOrDefault(d => d.DeviceId.Equals(deviceId));

            var services = await device.GetGattServicesAsync();
            var service = services.Services.FirstOrDefault(s => s.Uuid.Equals(new Guid("00001623-1212-efde-1623-785feabcd123")));

            var characteristics = await service.GetCharacteristicsAsync();
            var characteristic = characteristics.Characteristics.FirstOrDefault(c => c.Uuid == new Guid("00001624-1212-efde-1623-785feabcd123"));

            var buffer = CryptographicBuffer.CreateFromByteArray(message.Bytes.ToArray());

            var result = await characteristic.WriteValueWithResultAsync(buffer);
        }
    }
}
