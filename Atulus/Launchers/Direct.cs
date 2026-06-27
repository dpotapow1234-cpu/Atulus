using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atulus.Launchers
{
    public static class Direct
    {
        public static string GetSteamPath()
        {
            // 1. Check current user registry (Most reliable for active Steam installations)
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam"))
            {
                if (key?.GetValue("SteamPath") is string path)
                {
                    return path.Replace('/', '\\'); // Normalize directory slashes
                }
            }

            // 2. Fallback to Local Machine 64-bit/32-bit registry
            string[] machineKeys = {
            @"SOFTWARE\Wow6432Node\Valve\Steam", // 64-bit Windows
            @"SOFTWARE\Valve\Steam"              // 32-bit Windows
            };

            foreach (var subKey in machineKeys)
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(subKey))
                {
                    if (key?.GetValue("InstallPath") is string path)
                    {
                        return path;
                    }
                }
            }

            return null; // Steam not found
        }

        public static string GetEpicGamesPath()
        {
            // Try to open the 64-bit and 32-bit uninstallation registry keys
            string registryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

            using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey key = baseKey.OpenSubKey(registryPath))
                {
                    if (key != null)
                    {
                        foreach (string subkeyName in key.GetSubKeyNames())
                        {
                            using (RegistryKey subkey = key.OpenSubKey(subkeyName))
                            {
                                // Check if the application name matches Epic Games Launcher
                                string displayName = subkey?.GetValue("DisplayName") as string;
                                if ("Epic Games Launcher".Equals(displayName, StringComparison.OrdinalIgnoreCase))
                                {
                                    string installLocation = subkey.GetValue("InstallLocation") as string;
                                    if (!string.IsNullOrEmpty(installLocation))
                                    {
                                        // Path normally points to the Epic Games folder, so we target the executable
                                        return System.IO.Path.Combine(installLocation, "Launcher", "Portal", "Binaries", "Win64", "EpicGamesLauncher.exe");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
