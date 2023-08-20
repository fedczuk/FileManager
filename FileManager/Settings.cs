using Microsoft.Win32;
using System.Security.Permissions;
using System;

[assembly: RegistryPermissionAttribute(SecurityAction.RequestMinimum,
        ViewAndModify = "HKEY_CURRENT_USER")]  

namespace FileManager.Properties {
      
    
    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    internal sealed partial class Settings {
        
        public Settings() {
            // // To add event handlers for saving and changing settings, uncomment the lines below:
            //
            this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }
        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            switch (e.SettingName)
            {
                case "RunWithSystem":
                    
                    RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                    try
                    {
                        if ((bool)e.NewValue)
                            rk.SetValue("FM", System.Windows.Application.ResourceAssembly.Location);
                        else
                            rk.DeleteValue("FM");
                    }
                    catch (Exception) { }
                    break;
            }
        }
        
        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            // Add code to handle the SettingsSaving event here.
        }
    }
}
