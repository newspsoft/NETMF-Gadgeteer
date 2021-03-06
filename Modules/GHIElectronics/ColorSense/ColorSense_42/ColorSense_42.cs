﻿using System;
using Microsoft.SPOT;

using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using GTI = Gadgeteer.Interfaces;

namespace Gadgeteer.Modules.GHIElectronics
{
    // -- CHANGE FOR MICRO FRAMEWORK 4.2 --
    // If you want to use Serial, SPI, or DaisyLink (which includes GTI.SoftwareI2C), you must do a few more steps
    // since these have been moved to separate assemblies for NETMF 4.2 (to reduce the minimum memory footprint of Gadgeteer)
    // 1) add a reference to the assembly (named Gadgeteer.[interfacename])
    // 2) in GadgeteerHardware.xml, uncomment the lines under <Assemblies> so that end user apps using this module also add a reference.

    /// <summary>
    /// A ColorSense module for Microsoft .NET Gadgeteer
    /// </summary>
    [Obsolete]
    public class ColorSense : GTM.Module
    {
        private static GTI.DigitalOutput LEDControl;
        private static GTI.SoftwareI2C softwareI2C;
        private static byte[] readRegisterData = new byte[2];
        private const byte colorAddress = 0x39;

        // Note: A constructor summary is auto-generated by the doc builder.
        /// <summary></summary>
        /// <param name="socketNumber">The socket that this module is plugged in to.</param>
        public ColorSense(int socketNumber)
        {
            Socket socket = Socket.GetSocket(socketNumber, true, this, null);

            socket.EnsureTypeIsSupported(new char[] {'X', 'Y'}, this);

            LEDControl = new GTI.DigitalOutput(socket, Socket.Pin.Three, false, this);

            softwareI2C = new GTI.SoftwareI2C(socket, Socket.Pin.Five, Socket.Pin.Four, this);

           // Send COMMAND to access control register for chip power-up
           // Send to power-up chip
            softwareI2C.Write(colorAddress, new byte[] { 0x80, 0x03 });
        }

        /// <summary>
        /// Toggles the on-board LEDs to the passed in state.
        /// </summary>
        /// <param name="LEDState">State to set the LEDs to.</param>
        public void ToggleOnboardLED(bool LEDState)
        {
            LEDControl.Write(LEDState);
        }

        /// <summary>
        /// Reads the current color from the sensor and returns the results
        /// </summary>
        /// <returns>Returns an instance of the ColorChannels structure, holding the current measurement of color values.</returns>
        public ColorChannels ReadColorChannels()
        {
            ColorChannels returnData;

            byte[] TransmitBuffer = new byte[1];

            TransmitBuffer[0] = 0x90; // Send COMMAND to access Green Color Channel register for chip
            returnData.Green = readWord(0x90, TransmitBuffer)[0];
            TransmitBuffer[0] = 0x91;
            returnData.Green |= (uint)readWord(0x91, TransmitBuffer)[0] << 8;
            //returnData.Green = 256 * (uint)readWord(TransmitBuffer)[0] + returnData.Green;

            TransmitBuffer[0] = 0x92; // Send COMMAND to access Red Color Channel register for chip
            returnData.Red = readWord(0x92, TransmitBuffer)[0];
            TransmitBuffer[0] = 0x93;
            returnData.Red |= (uint)readWord(0x93, TransmitBuffer)[0] << 8;

            TransmitBuffer[0] = 0x94; // Send COMMAND to access Blue Color Channel register for chip
            returnData.Blue = readWord(0x94, TransmitBuffer)[0];
            TransmitBuffer[0] = 0x95;
            returnData.Blue |= (uint)readWord(0x95, TransmitBuffer)[0] << 8;

            TransmitBuffer[0] = 0x96; // Send COMMAND to access Clear Channel register for chip
            returnData.Clear = readWord(0x96, TransmitBuffer)[0];
            TransmitBuffer[0] = 0x97;
            returnData.Clear |= (uint)readWord(0x97, TransmitBuffer)[0] << 8;

            return returnData;
        }

        private byte[] readWord(byte address, byte[] CommandBytes)
        {
            softwareI2C.WriteRead(colorAddress, CommandBytes, readRegisterData);
            return readRegisterData;
        }

        /// <summary>
        /// Structure to hold color data
        /// </summary>
        public struct ColorChannels
        {
            /// <summary>
            /// Intensity of green-filtered channel
            /// </summary>
            public uint Green;

            /// <summary>
            /// Intensity of red-filtered channel
            /// </summary>
            public uint Red;

            /// <summary>
            /// Intensity of blue-filtered channel
            /// </summary>
            public uint Blue;

            /// <summary>
            /// Intensity of non-filtered channel
            /// </summary>
            public uint Clear;
        }
    }
}
