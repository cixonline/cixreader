﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CIXClientTest {
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
    internal class Resource1 {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource1() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CIXClientTest.Resource1", typeof(Resource1).Assembly);
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
        ///   Looks up a localized string similar to &lt;MessageResultSet2 xmlns=&quot;http://cixonline.com&quot; xmlns:i=&quot;http://www.w3.org/2001/XMLSchema-instance&quot;&gt;
        ///    &lt;Count&gt;329&lt;/Count&gt;
        ///    &lt;Messages&gt;
        ///        &lt;Message2&gt;
        ///            &lt;Author&gt;spalmer&lt;/Author&gt;
        ///            &lt;Body&gt;Hmm... switch to View-&gt;Sort By Date. Do they show up there? Have you deleted the topic or database since installing the latest build?
        ///
        ///-Steve
        ///steve@icuk.net
        ///&lt;/Body&gt;
        ///            &lt;DateTime&gt;22/04/2015 10:04:10&lt;/DateTime&gt;
        ///            &lt;Flag&gt;U&lt;/Flag&gt;
        ///            &lt;Forum&gt;cix.beta&lt;/Forum&gt;
        ///      [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string UserSyncData {
            get {
                return ResourceManager.GetString("UserSyncData", resourceCulture);
            }
        }
    }
}
