﻿/*
 * by Malcolm McCaffery http://chentiangemalc.wordpress.com
 * Twitter @chentiangemalc
 * free to use as you wish...just if you make it better, send me the fixes! :)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SetProxy
{
	class Program
	{
		/// <summary>
		/// Demonstrates use of ProxyRoutines.SetProxy
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			bool EnableProxy = false;
			string ProxyAddress = "";
			string AutoConfigURL = "";
			string Exceptions = "";
			string ConnectionName = "";
			bool ByPassLocal = false;
			bool AutoDetect = false;

			if (args.Length == 0)
			{
				PrintHelp();
				return;
			}
			foreach (string s in args)
			{
				string[] optionValue = s.ToLower().Split(':');
				string option = optionValue[0];
				string value = optionValue[1];

				switch (option) {
				case "/proxy":
					switch (value)
					{
						case "enable":
							EnableProxy = true;
							break;
						case "disable":
							EnableProxy = false;
							break;
						default:
							Console.WriteLine(string.Format("Unrecognized value {0}", value));
							Environment.Exit(1);
					}
					break;
				case "/address":
					ProxyAddress = value;
					break;
				case "/pac":
					AutoConfigURL = value;
					break;
				case "/autodetect":
					switch (value)
					{
						case "enable":
							EnableProxy = true;
							AutoDetect = true;
							break;
						case "disable":
							AutoDetect = false;
							break;
						default:
							Console.WriteLine(string.Format("Unrecognized value {0}", value));
							Environment.Exit(1);
					}
					break;
				case "/exceptions":
					Exceptions = value;
					break;
				case "/connection":
					ConnectionName = value;
					break;
				case "/bypasslocal":
					ByPassLocal = true;
				default:
					Console.WriteLine(string.Format("Unrecognized parameter '{0}' - value ignored", s));
					Environment.Exit(1);
				}
			}

			// insert bypass for local into exceptions...
			if (ByPassLocal)
			{
				if (string.IsNullOrEmpty(Exceptions))
				{
					Exceptions = "<local>";
				}
				else
				{
					Exceptions = string.Format("{0};<local>", Exceptions);
				}
			}

			try
			{
				if (ProxyRoutines.SetProxy(EnableProxy, AutoDetect, ProxyAddress, Exceptions, AutoConfigURL, ConnectionName))
				{
					Console.WriteLine("Configured Proxy OK!");
				}
				else
				{
					Console.WriteLine("Failed to configure proxy!");
					Environment.Exit(1);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("Failed to set proxy. Error: {0}", ex.Message));
			}
		}

		static void PrintHelp()
		{
			Console.WriteLine("SetProxy by Malcolm McCaffery");
			Console.WriteLine("http://chentiangemalc.wordpress.com");
			Console.WriteLine();
			Console.WriteLine("Options:");
			Console.WriteLine();
			Console.WriteLine("/proxy:[enable|disable] - Enable/Disable Proxy");
			Console.WriteLine("/address:proxy:port - Specify Proxy Address");
			Console.WriteLine("/pac:url - Specify PAC File URL");
			Console.WriteLine("/autodetect:[enable|disable] - enable/disable auto detect");
			Console.WriteLine("/exceptions:[exception list]");
			Console.WriteLine("/connection:[connection name] - specify a dial up connection name to set proxy for");
			Console.WriteLine("/bypasslocal - Bypass proxy for local connections.");
			Console.WriteLine();
		}
	}
}
