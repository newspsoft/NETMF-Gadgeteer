﻿using GTI = Gadgeteer.Interfaces;
using GTM = Gadgeteer.Modules;

namespace Gadgeteer.Modules.GHIElectronics
{
	/// <summary>
	/// A Relay X1 module for Microsoft .NET Gadgeteer
	/// </summary>
	public class RelayX1 : GTM.Module
	{
		private GTI.DigitalOutput enable;
		private bool state;

		/// <summary>Constructs a new RelayX1 instance.</summary>
		/// <param name="socketNumber">The socket that this module is plugged in to.</param>
		public RelayX1(int socketNumber)
		{
			Socket socket = Socket.GetSocket(socketNumber, true, this, null);

			socket.EnsureTypeIsSupported(new char[] { 'X', 'Y' }, this);

			this.enable = new GTI.DigitalOutput(socket, Socket.Pin.Five, false, this);

		}

		/// <summary>
		/// Gets or sets whether the relay is on or off.
		/// </summary>
		public bool Enabled
		{
			get
			{
				return this.state;
			}
			set
			{
				this.enable.Write(value);
				this.state = value;
			}
		}

		/// <summary>
		/// Turns the relay on.
		/// </summary>
		public void TurnOn()
		{
			this.Enabled = true;
		}

		/// <summary>
		/// Turns the relay off.
		/// </summary>
		public void TurnOff()
		{
			this.Enabled = false;
		}
	}
}
