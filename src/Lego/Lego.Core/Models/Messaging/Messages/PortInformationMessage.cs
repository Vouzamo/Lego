using System;
using System.Collections.Generic;
using System.Linq;

namespace Lego.Core.Models.Messaging.Messages
{
    public class PortInformationMessage : Message
    {
        public byte Port => Body.ElementAt(0);
        public PortInformationType InformationType => (PortInformationType)Body.ElementAt(1);

        #region ModeInformation
        public PortCapabilities Capabilities => (PortCapabilities)Body.ElementAt(2);
        public byte ModeCount => Body.ElementAt(3);
        public ModeCombinations InputModes => (ModeCombinations)BitConverter.ToUInt16(Body.ToArray(), 4);
        public ModeCombinations OutputModes => (ModeCombinations)BitConverter.ToUInt16(Body.ToArray(), 6);
        #endregion

        #region CombinationInfo
        public IEnumerable<ModeCombinations> ModeCombinations => ParseModeCombinations();
        #endregion

        public PortInformationMessage(byte[] bytes) : base(bytes)
        {
            
        }

        public IEnumerable<ModeCombinations> ParseModeCombinations()
        {
            var modeCombinations = new List<ModeCombinations>();

            if(InformationType != PortInformationType.Possible_Mode_Combinations)
            {
                return Enumerable.Empty<ModeCombinations>();
            }

            foreach(var modeCombination in Body.Skip(2))
            {
                if(modeCombination == 0)
                {
                    break;
                }

                modeCombinations.Add((ModeCombinations)modeCombination);
            }

            return modeCombinations;
        }
    }
}
