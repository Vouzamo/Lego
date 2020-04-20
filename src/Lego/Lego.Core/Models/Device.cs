using Lego.Core.Extensions;
using Lego.Core.Models.Messaging;
using Lego.Core.Models.Messaging.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lego.Core
{
    public class PortModeInformation
    {
        public string Name { get; set; }
        public byte[] ValueFormat { get; set; }

        public bool IsReady => !string.IsNullOrEmpty(Name) && ValueFormat != default;
    }

    public class Device : IDevice
    {
        public bool IsReady { get; protected set; } = false;

        public Hub Hub { get; }
        public byte Port { get; }
        public PortCapabilities Capabilities { get; protected set; }

        public ModeCombinations AvailableInputModes { get; protected set; }
        public ModeCombinations AvailableOutputModes { get; protected set; }
        public IEnumerable<ModeCombinations> ModeCombinations { get; protected set; }

        public Dictionary<byte, PortModeInformation> ModeInformation { get; protected set; } = new Dictionary<byte, PortModeInformation>();

        public bool IsLocked { get; protected set; } = false;
        public Dictionary<byte, PortInputFormatSingleMessage> InputModes { get; protected set; } = new Dictionary<byte, PortInputFormatSingleMessage>();

        public ModeCombinations InputMode { get; protected set; }

        public Device(Hub hub, byte port)
        {
            Hub = hub;
            Port = port;

            SendMessage(new PortInformationRequestMessage(Port, PortInformationType.Mode_Information));
        }

        public void SendMessage(IMessage message)
        {
            Hub.SendMessage(message);
        }

        public virtual void ReceiveMessage(IMessage message)
        {
            switch(message)
            {
                case PortValueSingleMessage portValueMessage:
                    HandleValue(portValueMessage.Body.ToArray());
                    break;
                case PortValueCombinedMessage portValueCombinedMessage:
                    HandleValue(portValueCombinedMessage.Body.ToArray());
                    break;
                case PortInformationMessage portInformationMessage:
                    switch(portInformationMessage.InformationType)
                    {
                        case PortInformationType.Mode_Information:
                            Capabilities = portInformationMessage.Capabilities;

                            AvailableInputModes = portInformationMessage.InputModes;

                            foreach(var mode in AvailableInputModes.ToModes())
                            {
                                ModeInformation.Add(mode, new PortModeInformation());

                                SendMessage(new PortModeInformationRequestMessage(Port, mode, PortModeInformationType.Name));
                                SendMessage(new PortModeInformationRequestMessage(Port, mode, PortModeInformationType.ValueFormat));
                            }

                            AvailableOutputModes = portInformationMessage.OutputModes;

                            if(Capabilities.HasFlag(PortCapabilities.Combinable))
                            {
                                SendMessage(new PortInformationRequestMessage(Port, PortInformationType.Possible_Mode_Combinations));
                            }

                            break;
                        case PortInformationType.Possible_Mode_Combinations:
                            ModeCombinations = portInformationMessage.ModeCombinations;

                            IsReady = true;
                            break;
                    }
                    break;
                case PortModeInformationMessage portModeInformationMessage:
                    switch(portModeInformationMessage.InformationType)
                    {
                        case PortModeInformationType.Name:
                            ModeInformation[portModeInformationMessage.Mode].Name = portModeInformationMessage.Name;
                            break;
                        case PortModeInformationType.ValueFormat:
                            ModeInformation[portModeInformationMessage.Mode].ValueFormat = portModeInformationMessage.ValueFormat;
                            break;
                    }
                    break;
                case PortInputFormatSingleMessage portInputFormatSingleMessage:
                    InputModes[portInputFormatSingleMessage.Mode] = portInputFormatSingleMessage;

                    if (!IsLocked)
                    {
                        // Single mode can assume just one mode is enabled
                        InputMode = (ModeCombinations)(0b_0000_0000_0000_0001 << portInputFormatSingleMessage.Mode);
                    }
                    break;
                case PortInputFormatCombinedMessage portInputFormatCombinedMessage:
                    if (IsLocked)
                    {
                        // Assume everything worked and unlock the mode(s)
                        SendMessage(new PortInputFormatSetupCombinedMessage(Port, PortInputFormatSetupSubCommands.Unlock_And_Start_With_Multi_Update_Disabled));

                        // set input mode here?
                        IsLocked = false;
                    }
                    break;

            }
        }

        public void SetSingleInputMode(byte mode, uint delta = 1, bool notify = true)
        {
            if (Capabilities.HasFlag(PortCapabilities.Input))
            {
                SendMessage(new PortInputFormatSetupSingleMessage(Port, mode, delta, notify));
            }
        }

        public async Task<bool> SetCombinedInputMode(byte modeCombinationIndex, uint delta = 1, bool notify = true)
        {
            if (Capabilities.HasFlag(PortCapabilities.Input) && Capabilities.HasFlag(PortCapabilities.Combinable))
            {
                var modeCombination = ModeCombinations.ElementAtOrDefault(modeCombinationIndex);

                if (modeCombination != default)
                {
                    // combined input
                    SendMessage(new PortInputFormatSetupCombinedMessage(Port, PortInputFormatSetupSubCommands.Lock_LPF2_Device_For_Setup));

                    IsLocked = true;

                    await Task.Delay(1000);

                    InputModes.Clear();

                    var modeAndDatasetCombinations = new List<byte>();

                    foreach (var mode in modeCombination.ToModes())
                    {
                        SendMessage(new PortInputFormatSetupSingleMessage(Port, mode, delta, notify));

                        for(int i = 1; i <= ModeInformation[mode].ValueFormat[0]; i++)
                        {
                            var modeAndDatasetInfo = mode << 4;

                            modeAndDatasetInfo |= (i - 1);

                            modeAndDatasetCombinations.Add((byte)modeAndDatasetInfo);
                        }

                        do
                        {
                            await Task.Delay(500);
                        }
                        while (!InputModes.ContainsKey(mode));
                    }

                    SendMessage(new PortInputFormatSetupCombinedSetModeDatasetMessage(Port, modeCombinationIndex, modeAndDatasetCombinations));

                    do
                    {
                        await Task.Delay(500);
                    }
                    while (IsLocked);

                    InputMode = modeCombination;

                    return true;
                }
            }

            return false;
        }

        public virtual void HandleValue(byte[] bytes)
        {

        }
    }
}
