﻿using System;
using SQLite;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
//using Xamarin.Forms;
using RadiusNetworks.IBeaconAndroid;
using System.Linq;
using Android.Graphics;
using Android.Support.V4.App;
using Xamarin.Forms;
using Android.Media;

//using Android.Support.V4.App;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;

namespace CoffeeBreak.Droid
{
//	[Activity (Label = "CoffeeBreak.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
//	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
//	{
//		protected override void OnCreate (Bundle bundle)
//		{
//			base.OnCreate (bundle);
//
//			global::Xamarin.Forms.Forms.Init (this, bundle);
//
//			LoadApplication (new App ());
//		}
//	}


	[Activity(Label = "Find The Monkey", MainLauncher = true, LaunchMode = LaunchMode.SingleTask)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, IBeaconConsumer
	{
		private User username;

		private const string UUID = "ACFD065E-C3C0-11E3-9BBE-1A514932AC01";
		private const string monkeyId = "Monkey";

		IBeaconManager _iBeaconManager;
		MonitorNotifier _monitorNotifier;
		RangeNotifier _rangeNotifier;
		RadiusNetworks.IBeaconAndroid.Region _monitoringRegion;
		RadiusNetworks.IBeaconAndroid.Region _rangingRegion;

		int _previousProximity;

		public MainActivity()
		{
			_iBeaconManager = IBeaconManager.GetInstanceForApplication(this);

			_monitorNotifier = new MonitorNotifier();
			_rangeNotifier = new RangeNotifier();

			_monitoringRegion = new RadiusNetworks.IBeaconAndroid.Region(monkeyId, UUID, null, null);
			_rangingRegion = new RadiusNetworks.IBeaconAndroid.Region(monkeyId, UUID, null, null);
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

			LoadApplication (new App ());

		#region Gestione notifiche
//			username = Utility.GetUser (DependencyService.Get<ISQLite> ().GetConnection ());
//			string uname = username != null ? username.username : "";
//			notificationManager = GetSystemService( Context.NotificationService) as NotificationManager;
//			// Istanzio il notification builder
//			NotificationCompat.Builder builder = new NotificationCompat.Builder (this)
//				.SetContentTitle ("Sample Notification")
//				.SetContentText ("Ciao " + uname + "\n\nBuon caffe'!")
//				.SetSmallIcon (Resource.Drawable.icon)
//				.SetVibrate(new long[]{500,700, 900})
//				.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
//				.SetAutoCancel(true);
//
//
//			// Costruisco la notifica
//			notification = builder.Build();
		#endregion




			if (Utility.GetUser(DependencyService.Get<ISQLite> ().GetConnection ()) != null) {
			
				MyPage.SetText("Sto cercando...",Xamarin.Forms.Color.White);

				_iBeaconManager.Bind(this);
	
				_monitorNotifier.EnterRegionComplete += EnteredRegion;
				_monitorNotifier.ExitRegionComplete += ExitedRegion;
	
				_rangeNotifier.DidRangeBeaconsInRegionComplete += this.RangingBeaconsInRegion;
			}


		}

		protected override void OnResume()
		{
			base.OnResume();
		}

		protected override void OnPause()
		{
			base.OnPause();
		}

		void EnteredRegion(object sender, MonitorEventArgs e)
		{

	    }

		void ExitedRegion(object sender, MonitorEventArgs e)
		{
			RunOnUiThread(() => Toast.MakeText(this, "No beacons visible", ToastLength.Short).Show());
		}


		// bool isFirstTime = true;
		DateTime startTime = DateTime.Now;
		void RangingBeaconsInRegion(object sender, RangeEventArgs e)
		{
			if (e.Beacons.Count > 0)
			{
				var beacon = e.Beacons.FirstOrDefault();
				var message = string.Empty;
				username = Utility.GetUser (DependencyService.Get<ISQLite> ().GetConnection ());


				switch((ProximityType)beacon.Proximity)
				{
				case ProximityType.Immediate:

					UpdateDisplay ("Ciao " + username.username + ", ho trovato un beacon! \n E' MOLTO vicino\n\nRssi: " + (ProximityType)beacon.Rssi, Xamarin.Forms.Color.Green);
                        //bool send = DependencyService.Get<ISendMail> ().Send();

                        //---------------
                        var uri = Android.Net.Uri.Parse ("http://asknet.ddns.net/CoffeeBreakService.asmx/UpdateMessage?u=Pino");
                        var intent = new Intent (Intent.ActionView, uri);
                        StartActivity (intent);
                        // --------------


                        //					if(isFirstTime || (DateTime.Now - startTime).TotalSeconds > 10) 
                        //					{
                        //						isFirstTime = false;
                        //						ShowNotification();
                        //						startTime = DateTime.Now;
                        //					break;
                        //					}
                        break;
				case ProximityType.Near:
					UpdateDisplay("Ho trovato un beacon! \n E' vicino\n\nRssi: " + (ProximityType)beacon.Rssi, Xamarin.Forms.Color.Yellow);
					break;
				case ProximityType.Far:
					UpdateDisplay("Ho trovato un beacon! \n E' lontano\n\nRssi: " + (ProximityType)beacon.Rssi, Xamarin.Forms.Color.Blue);
					break;
				case ProximityType.Unknown:
					UpdateDisplay("Non sono sicuro che ci sia un Beacon nelle vicinanze.\n\nRssi: " + (ProximityType)beacon.Rssi, Xamarin.Forms.Color.Red);
					break;
				}

				_previousProximity = beacon.Proximity;
			}
		}

		#region IBeaconConsumer impl
		public void OnIBeaconServiceConnect()
		{
			_iBeaconManager.SetMonitorNotifier(_monitorNotifier);
			_iBeaconManager.SetRangeNotifier(_rangeNotifier);

			_iBeaconManager.StartMonitoringBeaconsInRegion(_monitoringRegion);
			_iBeaconManager.StartRangingBeaconsInRegion(_rangingRegion);
		}
		#endregion

		private void UpdateDisplay(string message, Xamarin.Forms.Color color)
		{
			RunOnUiThread(() =>
				{
					MyPage.SetText(message, color);
					//_text.Text = message;
					//_view.SetBackgroundColor(color);
				});
		}



        #region ============   Gestione notifiche  ===============
        /*
        public void ShowNotification()
		{
			username = Utility.GetUser (DependencyService.Get<ISQLite> ().GetConnection ());
			string uname = username != null ? username.username : "";
			notificationManager = GetSystemService( Context.NotificationService) as NotificationManager;
			// Istanzio il notification builder
			NotificationCompat.Builder builder = new NotificationCompat.Builder (this)
				.SetContentTitle ("Coffee Break!")
				.SetContentText ("Ciao " + uname + "\n\nBuon caffe'!")
				.SetSmallIcon (Resource.Drawable.icon)
				.SetVibrate(new long[]{500,700, 900})
				.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
				.SetPriority((int)NotificationPriority.High)
				.SetAutoCancel(true);

			// Costruisco la notifica
			notification = builder.Build();

			notificationManager.Notify (notificationId, notification);
		}
        */
        #endregion



        protected override void OnDestroy()
		{
			base.OnDestroy();
//
//			_monitorNotifier.EnterRegionComplete -= EnteredRegion;
//			_monitorNotifier.ExitRegionComplete -= ExitedRegion;
//
//			_rangeNotifier.DidRangeBeaconsInRegionComplete -= RangingBeaconsInRegion;
//
//			_iBeaconManager.StopMonitoringBeaconsInRegion(_monitoringRegion);
//			_iBeaconManager.StopRangingBeaconsInRegion(_rangingRegion);
//			_iBeaconManager.UnBind(this);
		}
	}
}

