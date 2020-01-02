/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 * 
 * Copyright (c) 2011 - 2020 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2020 EasyCoding Team.
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Security.Principal;

namespace srcrepair.core
{
    /// <summary>
    /// Class for working with processes.
    /// </summary>
    public static class ProcessManager
    {
        /// <summary>
        /// Terminates process with specified name and return it's PID.
        /// If multiple processes with the same image name detected,
        /// will return PID of the last terminated process.
        /// If no processes matches criteria, will return 0.
        /// </summary>
        /// <param name="ProcessName">Process name.</param>
        /// <returns>PID of terminated process or 0.</returns>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public static int ProcessTerminate(string ProcessName)
        {
            // Setting default PID to 0...
            int ProcID = 0;

            // Filtering processes by name and creating array...
            Process[] LocalByName = Process.GetProcessesByName(ProcessName);

            // Using loop to terminate all processes from this array...
            foreach (Process ResName in LocalByName)
            {
                // Saving PID...
                ProcID = ResName.Id;
                
                // Terminating process...
                ResName.Kill();
            }

            // Returning PID of terminated process or default value...
            return ProcID;
        }

        /// <summary>
        /// Checks if process with specified name is running.
        /// </summary>
        /// <param name="ProcessName">Process name.</param>
        /// <returns>Returns True if process exists.</returns>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public static bool IsProcessRunning(string ProcessName)
        {
            Process[] LocalByName = Process.GetProcessesByName(ProcessName);
            return LocalByName.Length > 0;
        }

        /// <summary>
        /// Runs an external executable with UAC-elevated access rights
        /// (run as admininstrator).
        /// </summary>
        /// <param name="FileName">Full path to executable.</param>
        /// <returns>Returns PID of newly created process.</returns>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public static int StartWithUAC(string FileName)
        {
            // Setting advanced properties...
            ProcessStartInfo ST = new ProcessStartInfo()
            {
                FileName = FileName,
                Verb = "runas",
                WindowStyle = ProcessWindowStyle.Normal,
                UseShellExecute = true
            };

            // Starting process...
            Process NewProcess = Process.Start(ST);

            // Returning PID of created process...
            return NewProcess.Id;
        }

        /// <summary>
        /// Checks if current user has local adminstrators access rights
        /// (permissions).
        /// </summary>
        /// <returns>Returns True if current user has admin rights.</returns>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public static bool IsCurrentUserAdmin()
        {
            bool Result;
            try
            {
                // Checking access rights of currently logged-in user...
                WindowsPrincipal UP = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                // Checking if current user is in Administrators group...
                Result = UP.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                // Exception detected. User has no admin rights.
                Result = false;
            }
            return Result;
        }

        /// <summary>
        /// Runs external executable in hidden state with optional command-line
        /// options and waits for its completion.
        /// </summary>
        /// <param name="SAppName">Full path to external executable.</param>
        /// <param name="SParameters">Command-line options.</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public static void StartProcessAndWait(string SAppName, string SParameters)
        {
            // Setting advanced properties...
            ProcessStartInfo ST = new ProcessStartInfo()
            {
                FileName = SAppName,
                Arguments = SParameters,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            // Starting process...
            Process NewProcess = Process.Start(ST);

            // Waiting for completion...
            NewProcess.WaitForExit();
        }

        /// <summary>
        /// Runs external executable in hidden state with optional command-line
        /// options, waits for its completion and returns its stdout output.
        /// </summary>
        /// <param name="SAppName">Full path to external executable.</param>
        /// <param name="SParameters">Command-line options.</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public static string StartProcessWithStdOut(string SAppName, string SParameters)
        {
            // Creating local variable for saving result...
            string Result;

            // Setting advanced properties...
            ProcessStartInfo ST = new ProcessStartInfo()
            {
                FileName = SAppName,
                Arguments = SParameters,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            // Starting process...
            Process NewProcess = Process.Start(ST);

            // Reading stdout output of running process...
            Result = NewProcess.StandardOutput.ReadToEnd();

            // Waiting for completion...
            NewProcess.WaitForExit();

            // Returning result...
            return Result;
        }

        /// <summary>
        /// Opens specified URL in default Web browser.
        /// </summary>
        /// <param name="URI">Full URL.</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public static void OpenWebPage(string URI)
        {
            Process.Start(URI);
        }

        /// <summary>
        /// Adds quotes to path.
        /// </summary>
        /// <param name="Source">Source string with path.</param>
        /// <returns>Quoted string with path.</returns>
        public static string AddQuotesToPath(string Source)
        {
            return String.Format(Properties.Resources.AppOpenHandlerEscapeTemplate, Source);
        }

        /// <summary>
        /// Opens specified text file in a default (or overrided in application's
        /// settings (only on Windows platform)) text editor.
        /// </summary>
        /// <param name="FileName">Full path to text file.</param>
        /// <param name="EditorBin">External text editor (Windows only).</param>
        /// <param name="OS">Operating system type.</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public static void OpenTextEditor(string FileName, string EditorBin, CurrentPlatform.OSType OS)
        {
            switch (OS)
            {
                case CurrentPlatform.OSType.Windows:
                    Process.Start(EditorBin, AddQuotesToPath(FileName));
                    break;
                case CurrentPlatform.OSType.Linux:
                    Process.Start(Properties.Resources.AppOpenHandlerLin, AddQuotesToPath(FileName));
                    break;
                case CurrentPlatform.OSType.MacOSX:
                    Process.Start(Properties.Resources.AppOpenHandlerMac, String.Format("{0} \"{1}\"", "-t", FileName));
                    break;
            }
        }

        /// <summary>
        /// Shows specified file in a default file manager.
        /// </summary>
        /// <param name="FileName">Full path to file.</param>
        /// <param name="OS">Operating system type.</param>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        public static void OpenExplorer(string FileName, CurrentPlatform.OSType OS)
        {
            switch (OS)
            {
                case CurrentPlatform.OSType.Windows:
                    Process.Start(Properties.Resources.ShBinWin, String.Format("{0} \"{1}\"", Properties.Resources.ShParamWin, FileName));
                    break;
                case CurrentPlatform.OSType.Linux:
                    Process.Start(Properties.Resources.AppOpenHandlerLin, String.Format("\"{0}\"", Path.GetDirectoryName(FileName)));
                    break;
                case CurrentPlatform.OSType.MacOSX:
                    Process.Start(Properties.Resources.AppOpenHandlerMac, String.Format("\"{0}\"", Path.GetDirectoryName(FileName)));
                    break;
            }
        }
    }
}
