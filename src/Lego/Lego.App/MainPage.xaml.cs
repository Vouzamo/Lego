using Lego.Core;
using Lego.Core.Extensions;
using Lego.Core.Models.Devices.General;
using Lego.Core.Models.Devices.Hubs;
using Lego.Core.Models.Devices.Parts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            using (var connectionManager = new BluetoothLEConnectionManager())
            {
                var connectionA = await connectionManager.FindConnectionById("BluetoothLE#BluetoothLEb8:31:b5:93:3c:8c-90:84:2b:4d:d2:62");
                var connectionB = await connectionManager.FindConnectionById("BluetoothLE#BluetoothLEb8:31:b5:93:3c:8c-90:84:2b:4e:1b:dd");

                var hubA = new TechnicSmartHub(connectionA);
                var hubB = new TechnicSmartHub(connectionB);

                // wait until connected
                await hubA.Connect();
                await hubB.Connect();

                // wait until all 3 motors are connected to Hub A
                var leftTrack = await hubA.PortA<TechnicMotorXL>();
                var rightTrack = await hubA.PortB<TechnicMotorXL>();
                var turntable = await hubA.PortD<TechnicMotorL>();

                // wait until all 4 motors are connected to Hub B
                var primaryBoom = await hubB.PortA<TechnicMotorXL>();
                var secondaryBoom = await hubB.PortB<TechnicMotorL>();
                var tertiaryBoom = await hubB.PortC<TechnicMotorL>();
                var bucket = await hubB.PortD<TechnicMotorL>();

                primaryBoom.SetSpeedForDuration(100, 50, RotateDirection.Clockwise, 2000);
                bucket.SetSpeedForDuration(100, 100, RotateDirection.Clockwise, 2000);
                await Task.Delay(2000);

                await primaryBoom.AutoCalibrate(50);
                await secondaryBoom.AutoCalibrate(50);
                await tertiaryBoom.AutoCalibrate(40);
                await bucket.AutoCalibrate(30);

                // Routine to toggle between the calibrated min and max absolute position.
                do
                {
                    primaryBoom.GotoAbsolutePositionMin(100, 100);
                    secondaryBoom.GotoAbsolutePositionMin(100, 100);
                    tertiaryBoom.GotoAbsolutePositionMin(100, 100);
                    bucket.GotoAbsolutePositionMin(100, 100);
                    await Task.Delay(15000);
                    primaryBoom.GotoAbsolutePositionMax(100, 100);
                    secondaryBoom.GotoAbsolutePositionMax(100, 100);
                    tertiaryBoom.GotoAbsolutePositionMax(100, 100);
                    bucket.GotoAbsolutePositionMax(100, 100);
                    await Task.Delay(15000);
                }
                while (true);
            }
        }
    }
}
