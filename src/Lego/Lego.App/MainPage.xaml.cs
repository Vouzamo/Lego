using Lego.Core;
using Lego.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Lego.App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();

            DoStuff();
        }

        private async void DoStuff()
        {
            var connection = new BluetoothLEConnection();

            var hubA = await connection.EstablishHubConnectionById("BluetoothLE#BluetoothLEb8:31:b5:93:3c:8c-90:84:2b:4d:d2:62");
            //var hubB = await connection.EstablishHubConnectionById("BluetoothLE#BluetoothLEb8:31:b5:93:3c:8c-90:84:2b:4e:1b:dd");

            var leftTrack = await hubA.EstablishDeviceConnectionByPort<TechnicMotor>(0);
            var rightTrack = await hubA.EstablishDeviceConnectionByPort<TechnicMotor>(1);
            var turntable = await hubA.EstablishDeviceConnectionByPort<TechnicMotor>(3);

            turntable.SetSpeed(100);

            await Task.Delay(5000);

            leftTrack.SetSpeed(100);
            rightTrack.SetSpeed(100);

            await Task.Delay(5000);

            //var led = await hub.EstablishDeviceConnectionByInterface<LED, ILightEmittingDiode>();
            //led.SetColor(0x05);
        }
    }
}
