﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AssemblySoft.WonkaBuild {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class BuildResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal BuildResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("AssemblySoft.WonkaBuild.BuildResources", typeof(BuildResources).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot find the tasks definition directory!.
        /// </summary>
        public static string Error_CannotfindTaskDefinitions {
            get {
                return ResourceManager.GetString("Error_CannotfindTaskDefinitions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to find task history to load!.
        /// </summary>
        public static string Error_UnableToFindTaskHistoryToLoad {
            get {
                return ResourceManager.GetString("Error_UnableToFindTaskHistoryToLoad", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to find any tasks to load!.
        /// </summary>
        public static string Error_UnableTofindTasksToLoad {
            get {
                return ResourceManager.GetString("Error_UnableTofindTasksToLoad", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Completed loading history..
        /// </summary>
        public static string Status_CompletedLoadingHistory {
            get {
                return ResourceManager.GetString("Status_CompletedLoadingHistory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Completed loading tasks..
        /// </summary>
        public static string Status_CompletedLoadingTasks {
            get {
                return ResourceManager.GetString("Status_CompletedLoadingTasks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Loading history....
        /// </summary>
        public static string Status_LoadingHistory {
            get {
                return ResourceManager.GetString("Status_LoadingHistory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Loading tasks....
        /// </summary>
        public static string Status_LoadingTasks {
            get {
                return ResourceManager.GetString("Status_LoadingTasks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Starting build....
        /// </summary>
        public static string Status_StartingBuild {
            get {
                return ResourceManager.GetString("Status_StartingBuild", resourceCulture);
            }
        }
    }
}
