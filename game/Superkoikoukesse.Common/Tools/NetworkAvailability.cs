// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using System.Net;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.SystemConfiguration;
using MonoTouch.CoreFoundation;

namespace Superkoikoukesse.Common.Utils
{
	/// <summary>
	/// Network status enumeration
	/// </summary>
	public enum NetworkStatus
	{
		NotReachable,
		ReachableViaCarrierDataNetwork,
		ReachableViaWiFiNetwork
	}

	/// <summary>
	/// Reachability.
	/// </summary>
	/// <see>https://github.com/xamarin/monotouch-samples/blob/master/ReachabilitySample/reachability.cs</see>
	public static class NetworkAvailability
	{
	
		public static bool IsReachableWithoutRequiringConnection (NetworkReachabilityFlags flags)
		{
			// Is it reachable with the current network configuration?
			bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;
		
			// Do we need a connection to reach it?
			bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0;
		
			// Since the network stack will automatically try to get the WAN up,
			// probe that
			if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
				noConnectionRequired = true;
		
			return isReachable && noConnectionRequired;
		}

		/// <summary>
		/// Is the host reachable with the current network configuration
		/// </summary>
		/// <returns><c>true</c> if is host reachable the specified host; otherwise, <c>false</c>.</returns>
		/// <param name="host">Host.</param>
		public static bool IsHostReachable (string host)
		{
			if (host == null || host.Length == 0)
				return false;
		
			using (var r = new NetworkReachability (host)) {
				NetworkReachabilityFlags flags;
			
				if (r.TryGetFlags (out flags)) {
					return IsReachableWithoutRequiringConnection (flags);
				}
			}
			return false;
		}
		// 
		// Raised every time there is an interesting reachable event, 
		// we do not even pass the info as to what changed, and 
		// we lump all three status we probe into one
		//
		public static event EventHandler ReachabilityChanged;

		static void OnChange (NetworkReachabilityFlags flags)
		{
			var h = ReachabilityChanged;
			if (h != null)
				h (null, EventArgs.Empty);
		}
		//
		// Returns true if it is possible to reach the AdHoc WiFi network
		// and optionally provides extra network reachability flags as the
		// out parameter
		//
		static NetworkReachability adHocWiFiNetworkReachability;

		public static bool IsAdHocWiFiNetworkAvailable (out NetworkReachabilityFlags flags)
		{
			if (adHocWiFiNetworkReachability == null) {
				adHocWiFiNetworkReachability = new NetworkReachability (new IPAddress (new byte [] {
					169,
					254,
					0,
					0
				}));
				adHocWiFiNetworkReachability.SetCallback (OnChange);
				adHocWiFiNetworkReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
			}
		
			if (!adHocWiFiNetworkReachability.TryGetFlags (out flags))
				return false;
		
			return IsReachableWithoutRequiringConnection (flags);
		}

		static NetworkReachability defaultRouteReachability;

		static bool IsNetworkAvailable (out NetworkReachabilityFlags flags)
		{
			if (defaultRouteReachability == null) {
				defaultRouteReachability = new NetworkReachability (new IPAddress (0));
				defaultRouteReachability.SetCallback (OnChange);
				defaultRouteReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
			}
			if (!defaultRouteReachability.TryGetFlags (out flags))
				return false;
			return IsReachableWithoutRequiringConnection (flags);
		}

		static NetworkReachability remoteHostReachability;

		public static NetworkStatus RemoteHostStatus (string HostName)
		{
			NetworkReachabilityFlags flags;
			bool reachable;
		
			if (remoteHostReachability == null) {
				remoteHostReachability = new NetworkReachability (HostName);
			
				// Need to probe before we queue, or we wont get any meaningful values
				// this only happens when you create NetworkReachability from a hostname
				reachable = remoteHostReachability.TryGetFlags (out flags);
			
				remoteHostReachability.SetCallback (OnChange);
				remoteHostReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
			} else
				reachable = remoteHostReachability.TryGetFlags (out flags);			
		
			if (!reachable)
				return NetworkStatus.NotReachable;
		
			if (!IsReachableWithoutRequiringConnection (flags))
				return NetworkStatus.NotReachable;
		
			if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
				return NetworkStatus.ReachableViaCarrierDataNetwork;
		
			return NetworkStatus.ReachableViaWiFiNetwork;
		}

		public static NetworkStatus InternetConnectionStatus ()
		{
			NetworkReachabilityFlags flags;
			bool defaultNetworkAvailable = IsNetworkAvailable (out flags);
			if (defaultNetworkAvailable) {
				if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
					return NetworkStatus.NotReachable;
			} else if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
				return NetworkStatus.ReachableViaCarrierDataNetwork;
			else if (flags == 0)
				return NetworkStatus.NotReachable;
			return NetworkStatus.ReachableViaWiFiNetwork;
		}

		public static NetworkStatus LocalWifiConnectionStatus ()
		{
			NetworkReachabilityFlags flags;
			if (IsAdHocWiFiNetworkAvailable (out flags)) {
				if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
					return NetworkStatus.ReachableViaWiFiNetwork;
			}
			return NetworkStatus.NotReachable;
		}
	}
}
