﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DependecyServices.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(BatteryImplementation))]
namespace DependecyServices.Droid
{
    public class BatteryImplementation : IBattery
    {
        public BatteryImplementation()
        {

        }

        public int RemainingChargePercent
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using(var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            var level = battery.GetIntExtra(BatteryManager.ExtraLevel, -1);
                            var scale = battery.GetIntExtra(BatteryManager.ExtraScale, -1);
                            return (int)Math.Floor(level * 100D / scale);
                        }
                    }
                }

                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }

        public DependecyServices.BatteryStatus Status
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using (var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            int status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
                            var isCharging = status == (int)BatteryStatus.Charging || status == (int)BatteryStatus.Full;
                            var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);
                            var usbCharge = chargePlug == (int)BatteryPlugged.Usb;
                            var acCharge = chargePlug == (int)BatteryPlugged.Ac;
                            bool wirelessCharge = false;
                            wirelessCharge = chargePlug == (int)BatteryPlugged.Wireless;
                            isCharging = (usbCharge || acCharge || wirelessCharge);
                            if (isCharging)
                                return DependecyServices.BatteryStatus.Charging;
                            switch (status)
                            {
                                case (int)BatteryStatus.Charging:
                                    return DependecyServices.BatteryStatus.Charging;
                                case (int)BatteryStatus.Discharging:
                                    return DependecyServices.BatteryStatus.Discharging;
                                case (int)BatteryStatus.Full:
                                    return DependecyServices.BatteryStatus.Full;
                                case (int)BatteryStatus.NotCharging:
                                    return DependecyServices.BatteryStatus.NotCharging;
                                default:
                                    return DependecyServices.BatteryStatus.Unknown;


                            }
                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you hace android.permission.BATTERY_STATS");
                    throw;
                }
            }
        }

        public PowerSource PowerSource
        {
            get
            {
                try
                {
                    using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
                    {
                        using(var battery = Application.Context.RegisterReceiver(null, filter))
                        {
                            int status = battery.GetIntExtra(BatteryManager.ExtraStatus, -1);
                            var isCharging = status == (int)BatteryStatus.Charging || status == (int)BatteryStatus.Full;
                            var chargePlug = battery.GetIntExtra(BatteryManager.ExtraPlugged, -1);
                            var usbCharge = chargePlug == (int)BatteryPlugged.Usb;
                            var aCharge = chargePlug == (int)BatteryPlugged.Ac;

                            bool wirelessCharge = false;
                            wirelessCharge = chargePlug == (int)BatteryPlugged.Wireless;
                            isCharging = (usbCharge || aCharge || wirelessCharge);
                            if (!isCharging)
                                return DependecyServices.PowerSource.Battery;
                            else if (usbCharge)
                                return DependecyServices.PowerSource.Usb;
                            else if (aCharge)
                                return DependecyServices.PowerSource.Ac;
                            else if (wirelessCharge)
                                return DependecyServices.PowerSource.Wireless;
                            else
                                return DependecyServices.PowerSource.Other;

                        }
                    }
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
                    throw;
                }

            }
        }
    }
}