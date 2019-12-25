/*

Developed By: HazyTube
Name: Touch The Rear
Released on: GitHub and LSPDFR

*/

using System;
using System.Reflection;
using System.Windows.Forms;
using Rage;
using LSPD_First_Response.Mod.API;

[assembly: Rage.Attributes.Plugin("TouchTheRear", Description = "Touch the back of a car on a traffic stop", Author = "HazyTube")]
namespace TouchTheRear
{
    public class Main : Plugin
    {
        public override void Initialize()
        {
            Globals.Application.PluginName = "TouchTheRear";

            Functions.OnOnDutyStateChanged += DutyStateChange;
            AppDomain.CurrentDomain.AssemblyResolve += LSPDFRResolveEventHandler;

            Logger.Log(
                $"{Globals.Application.PluginName} {Assembly.GetExecutingAssembly().GetName().Version} has been  initialized.");

            //This sets the currentversion
            Globals.Application.CurrentVersion = $"{Assembly.GetExecutingAssembly().GetName().Version}";

            //This sets the config path to /plugins/lspdfr for the ini file
            Globals.Application.ConfigPath = "Plugins/LSPDFR/";

            Globals.Application.ConfigFileName = "TouchTheRear.ini";

            Globals.Status.Firsteventdistancetoobject = true;
            Globals.Status.Secondeventdistancetoobject = true;
        }

        public void DutyStateChange(bool OnDuty)
        {
            if (OnDuty)
            {
                Game.LogTrivial(
                    $"--------------------------------------{Globals.Application.PluginName} startup log--------------------------------------");

                int versionStatus = Updater.CheckUpdate();

                if (versionStatus == -1)
                {
                    Notifier.StartUpNotificationOutdated();
                    Logger.Log(
                        $"Plugin is out of date. (Current Version: {Globals.Application.CurrentVersion}) - (Latest Version: {Globals.Application.LatestVersion})");
                }
                else if (versionStatus == -2)
                {
                    Logger.Log("There was an issue checking plugin versions, the plugin may be out of date!");
                }
                else if (versionStatus == 1)
                {
                    Logger.Log(
                        "Current version of plugin is higher than the version reported on the official GitHub, this could be an error that you may want to report!");
                    Notifier.StartUpNotification();
                }
                else
                {
                    Notifier.StartUpNotification();
                    Logger.Log($"Plugin version v{Globals.Application.CurrentVersion} loaded succesfully");
                }
                
                //Loads the config file (.ini file)
                Settings.LoadSettings();

                GameFiber.StartNew(delegate
                {
                    while (true)
                    {
                        GameFiber.Yield();

                        if (Game.LocalPlayer.Character.Exists() && Game.LocalPlayer.Character.IsAlive &&
                            Functions.IsPlayerPerformingPullover() && Game.LocalPlayer.Character.IsOnFoot)
                        {
                            LHandle currentPullover = Functions.GetCurrentPullover();
                            Vehicle PulledOverVehicle = Functions.GetPulloverSuspect(currentPullover).CurrentVehicle;
                            
                            if (Game.LocalPlayer.Character.DistanceTo(PulledOverVehicle.RearPosition) < 2 && Globals.Status.Firsteventdistancetoobject && PulledOverVehicle.Exists())
                            {
                                Globals.Status.Firsteventdistancetoobject = false;

                                if (Globals.Keybindings.TouchRearModifier == Keys.None)
                                {
                                    Notifier.DisplayNotification("Traffic Stop",
                                        $"Press ~g~{Globals.Keybindings.TouchRearKey}~s~ to touch the rear of the vehicle");
                                }
                                else
                                {
                                    Notifier.DisplayNotification("Traffic Stop",
                                        $"Press ~g~{Globals.Keybindings.TouchRearKey}~s~ + ~g~{Globals.Keybindings.TouchRearModifier}~s~ to touch the rear of the vehicle");
                                }
                            }

                            if (Game.LocalPlayer.Character.DistanceTo(PulledOverVehicle.RearPosition) > 2)
                            {
                                Globals.Status.Firsteventdistancetoobject = true;
                            }

                            if (Globals.IsKeyDown(Globals.Keybindings.TouchRearModifier,
                                    Globals.Keybindings.TouchRearKey) &&
                                Game.LocalPlayer.Character.DistanceTo(PulledOverVehicle.Position) < 1 &&
                                PulledOverVehicle.Exists())
                            {
                                Game.LocalPlayer.Character.Face(PulledOverVehicle);
                                
                                Game.LocalPlayer.Character.Tasks.PlayAnimation(new AnimationDictionary("anim@mp_point"),
                                    "1st_person_outro_low", 3f, AnimationFlags.None).WaitForCompletion(20000);
                            }
                        }
                    }
                });
            }
        }

        public override void Finally()
        {
            Logger.Log($"Touch The Rear {Globals.Application.CurrentVersion} has been unloaded");
        }

        public static Assembly LSPDFRResolveEventHandler(object sender, ResolveEventArgs args)
        {
            foreach (Assembly allUserPlugin in Functions.GetAllUserPlugins())
            {
                if (args.Name.ToLower().Contains(allUserPlugin.GetName().Name.ToLower()))
                    return allUserPlugin;
            }
            return (Assembly) null;
        }
    }
}
