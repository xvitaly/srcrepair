﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace srcrepair.core {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class DebugStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DebugStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("srcrepair.core.DebugStrings", typeof(DebugStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value &quot;{0}&quot; of the &quot;{1}&quot; field is out of range..
        /// </summary>
        internal static string AppDbgCoreFieldOutOfRange {
            get {
                return ResourceManager.GetString("AppDbgCoreFieldOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Working with the registry is not supported on this platform..
        /// </summary>
        internal static string AppDbgCoreRegNotSupported {
            get {
                return ResourceManager.GetString("AppDbgCoreRegNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The service binary does not exist..
        /// </summary>
        internal static string AppDbgCoreServiceBinaryNotFound {
            get {
                return ResourceManager.GetString("AppDbgCoreServiceBinaryNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Working with the service is not supported on this platform..
        /// </summary>
        internal static string AppDbgCoreServiceNotSupported {
            get {
                return ResourceManager.GetString("AppDbgCoreServiceNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The requested value &quot;{0}&quot; for the &quot;{1}&quot; setter is out of range..
        /// </summary>
        internal static string AppDbgCoreSetterOutOfRange {
            get {
                return ResourceManager.GetString("AppDbgCoreSetterOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Directory {0} not found. Cannot add it to the Zip archive!.
        /// </summary>
        internal static string AppDbgCoreZipAddDirNotFound {
            get {
                return ResourceManager.GetString("AppDbgCoreZipAddDirNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Minor exception while while building the CleanupTargets list object..
        /// </summary>
        internal static string AppDbgExCoreClnManConstructor {
            get {
                return ResourceManager.GetString("AppDbgExCoreClnManConstructor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Minor exception while while building the Config list object..
        /// </summary>
        internal static string AppDbgExCoreConfManConstructor {
            get {
                return ResourceManager.GetString("AppDbgExCoreConfManConstructor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Minor exception while building the games list object..
        /// </summary>
        internal static string AppDbgExCoreGameManConstructor {
            get {
                return ResourceManager.GetString("AppDbgExCoreGameManConstructor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The helper application executable file does not exist..
        /// </summary>
        internal static string AppDbgExCoreHelperNxExists {
            get {
                return ResourceManager.GetString("AppDbgExCoreHelperNxExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Minor exception while building the HUD list object..
        /// </summary>
        internal static string AppDbgExCoreHudManConstructor {
            get {
                return ResourceManager.GetString("AppDbgExCoreHudManConstructor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Minor exception while building the list of available plugins object..
        /// </summary>
        internal static string AppDbgExCorePluginManConstructor {
            get {
                return ResourceManager.GetString("AppDbgExCorePluginManConstructor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The plugin executable file does not exist..
        /// </summary>
        internal static string AppDbgExCorePluginNotFound {
            get {
                return ResourceManager.GetString("AppDbgExCorePluginNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Minor exception while fetching the Steam mount points..
        /// </summary>
        internal static string AppDbgExCoreStmManMountPointsFetchError {
            get {
                return ResourceManager.GetString("AppDbgExCoreStmManMountPointsFetchError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No InstallPath value detected. Unable to get installation path from the registry..
        /// </summary>
        internal static string AppDbgExCoreStmManNoInstallPathDetected {
            get {
                return ResourceManager.GetString("AppDbgExCoreStmManNoInstallPathDetected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No Language value detected. Unable to get language settings from the registry..
        /// </summary>
        internal static string AppDbgExCoreStmManNoLangNameDetected {
            get {
                return ResourceManager.GetString("AppDbgExCoreStmManNoLangNameDetected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The SteamID list is empty. Cannot choose one of them..
        /// </summary>
        internal static string AppDbgExCoreStmManSidListEmpty {
            get {
                return ResourceManager.GetString("AppDbgExCoreStmManSidListEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to open the registry key..
        /// </summary>
        internal static string AppDbgExCoreType1VideoRegOpenError {
            get {
                return ResourceManager.GetString("AppDbgExCoreType1VideoRegOpenError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown version of the Source Engine has been detected..
        /// </summary>
        internal static string AppDbgExCoreUnknownEngineVersion {
            get {
                return ResourceManager.GetString("AppDbgExCoreUnknownEngineVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to fetch the value of the {0} video setting..
        /// </summary>
        internal static string AppDbgExCoreVideoLoadCvar {
            get {
                return ResourceManager.GetString("AppDbgExCoreVideoLoadCvar", resourceCulture);
            }
        }
    }
}
