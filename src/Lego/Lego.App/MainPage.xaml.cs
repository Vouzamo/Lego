using Lego.Core;
using Lego.Core.Extensions;
using Lego.Core.Models.Devices.Hubs;
using Lego.Core.Models.Devices.Parts;
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
            var connectionManager = new BluetoothLEConnectionManager();

            // wait until both hubs are connected
            var hubA = await connectionManager.EstablishHubConnectionById<TechnicSmartHub>("BluetoothLE#BluetoothLEb8:31:b5:93:3c:8c-90:84:2b:4d:d2:62");
            var hubB = await connectionManager.EstablishHubConnectionById<TechnicSmartHub>("BluetoothLE#BluetoothLEb8:31:b5:93:3c:8c-90:84:2b:4e:1b:dd");

            // wait until all 3 motors are connected to Hub A
            var leftTrack = await hubA.PortA<TechnicMotorXL>();
            var rightTrack = await hubA.PortB<TechnicMotorXL>();
            var turntable = await hubA.PortD<TechnicMotorL>();

            // wait until all 4 motors are connected to Hub B
            var primaryBoom = await hubB.PortA<TechnicMotorXL>();
            var secondaryBoom = await hubB.PortB<TechnicMotorL>();
            var tertiaryBoom = await hubB.PortC<TechnicMotorL>();
            var bucket = await hubB.PortD<TechnicMotorL>();

            // Naive commands that could be improved later on with calibration routines specific to the model configuration (e.g. motors connected to linear actuators could be calibrated to store absolute range of moment instead of relying on internal clutches etc).

            // move forwards for 5 seconds
            leftTrack.SetSpeedForDuration(50, 100, RotateDirection.CounterClockwise, 5000);
            rightTrack.SetSpeedForDuration(50, 100, RotateDirection.Clockwise, 5000);
            await Task.Delay(5000);

            // rotate boom for 3 seconds
            turntable.SetSpeedForDuration(100, 100, RotateDirection.Clockwise, 3000);
            await Task.Delay(3000);

            // reposition boom
            primaryBoom.SetSpeedForDuration(25, 100, RotateDirection.CounterClockwise, 1000);
            secondaryBoom.SetSpeedForDuration(75, 100, RotateDirection.Clockwise, 3000);
            tertiaryBoom.SetSpeedForDuration(100, 100, RotateDirection.Clockwise, 2000);

            await Task.Delay(3000);

            // lift bucket
            bucket.SetSpeedForDuration(50, 100, RotateDirection.Clockwise, 2000);
        }
    }
}
