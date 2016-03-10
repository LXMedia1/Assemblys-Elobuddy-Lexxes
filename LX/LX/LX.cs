using System;
using System.Runtime.InteropServices;
using EloBuddy.SDK.Events;
using System.Reflection;
namespace LX
{
	class LX
	{
		static void Main(string[] args)
		{
			Loading.OnLoadingComplete += OnLoadingComplete;
		}

		private static void OnLoadingComplete(EventArgs args)
		{
			LX_Evade.SpellDedector.Initiate();
			LX_Activator.Activator.Initiate();
			//LX_Evade.Evade.Initiate();
		}
	}
}
