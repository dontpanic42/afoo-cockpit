using System.ComponentModel;
using Newtonsoft.Json;

namespace AFooCockpit.App.Core.Settings
{
    /// <summary>
    /// Class to manage application settings
    /// </summary>
    internal class Settings
    {
        /// <summary>
        /// This class will be added to all config files - this can be used
        /// in the future to see if a config file is compatible with the current
        /// app version and to migrate the config file if needed
        /// </summary>
        public class SettingsMetadata : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler? PropertyChanged;

            private string? _appName;
            private string? _appVersion;

            public string? AppName { get => _appName; 
                set
                {
                    _appName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AppName)));
                } 
            }

            public string? AppVersion
            {
                get => _appVersion;
                set
                {
                    _appVersion = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AppVersion)));
                }
            }

            public SettingsMetadata()
            {
            }
        }

        /// <summary>
        /// Wrapper for list type settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class SettingsList<T> : INotifyPropertyChanged
        {
            private BindingList<T>? _bindingList;
            public event PropertyChangedEventHandler? PropertyChanged;

            public required BindingList<T> BindingList
            {
                get => _bindingList!; 
                set
                {
                    if(_bindingList != value && _bindingList != null)
                    {
                        _bindingList.ListChanged -= _bindingList_ListChanged;
                    }

                    _bindingList = value;
                    _bindingList.ListChanged += _bindingList_ListChanged;
                }
            }

            private void _bindingList_ListChanged(object? sender, ListChangedEventArgs e)
            {
                PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(nameof(BindingList)));
            }

        }

        public class SettingsRoot
        {
            private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            public class HasUnsavedChangesEventArgs : EventArgs
            {
                public required bool HasUnsavedChanges;
            }

            public delegate void OnHasUnsavedChangesHandler(SettingsRoot sender, HasUnsavedChangesEventArgs args);

            public event OnHasUnsavedChangesHandler? OnHasUnsavedChanges;

            private string SettingsPath = "";
            private string SettingsFile = "";

            private Dictionary<string, INotifyPropertyChanged> _settings = new Dictionary<string, INotifyPropertyChanged>();

            private bool _hasUnsavedChanges = true;
            public bool HasUnsavedChanges
            {
                get => _hasUnsavedChanges;
                private set
                {
                    if (value != _hasUnsavedChanges)
                    {
                        _hasUnsavedChanges = value;
                        OnHasUnsavedChanges?.Invoke(this, new HasUnsavedChangesEventArgs { HasUnsavedChanges = value });
                    }
                }
            }

            public SettingsRoot(string settingsPath, string settingsFile)
            {
                SettingsPath = settingsPath;
                SettingsFile = settingsFile;
                Load();
            }

            /// <summary>
            /// Gets a setting from the settings object. If the setting doesn't exist (yet), the default
            /// value will be added to the settings object and returned.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="key"></param>
            /// <param name="defaultSettings"></param>
            /// <returns></returns>
            public T GetOrDefault<T>(string key, T defaultSettings) where T : INotifyPropertyChanged, new()
            {
                if (_settings.ContainsKey(key))
                {
                    T value = (T) _settings[key];
                    return value;
                }
                else
                {
                    Add(key, defaultSettings);
                    return defaultSettings;
                }
            }

            /// <summary>
            /// Handler for list-type settings
            /// </summary>
            /// <typeparam name="T">Type extensing BidningList</typeparam>
            /// <typeparam name="V">The list type</typeparam>
            /// <param name="key"></param>
            /// <param name="defaultSettings"></param>
            /// <returns></returns>
            /// <exception cref="NotSupportedException"></exception>
            public BindingList<T> GetOrDefault<T>(string key, BindingList<T> defaultSettings)
            {
                if (!_settings.ContainsKey(key))
                {
                    Add(key, new SettingsList<T> { BindingList = defaultSettings });
                    return defaultSettings;
                }
                else
                {
                    var value =  _settings[key];
                    // Check if the requested setting is of type SettingsList
                    if (typeof(SettingsList<T>).IsAssignableFrom(value.GetType()))
                    {
                        return ((SettingsList<T>)value).BindingList;
                    }

                    throw new NotSupportedException($"Cannot return list setting - requested settings {key} is not assignable from {value.GetType().Name}");
                }
            }

            /// <summary>
            /// Adds a new object to the settings. This method should not be used by classes using settings,
            /// Instead they should use GetOrDefault() to ensure settings are always present
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            private void Add(string key, INotifyPropertyChanged value)
            {
                value.PropertyChanged += Value_PropertyChanged;
                _settings.Add(key, value);
                HasUnsavedChanges = true;
            }

            public void Remove(string key)
            {
                if (_settings.ContainsKey(key))
                {
                    ((INotifyPropertyChanged)_settings[key]).PropertyChanged -= Value_PropertyChanged;
                    _settings.Remove(key);
                    HasUnsavedChanges = true;
                }
            }

            /// <summary>
            /// Event handler from the child objects - when a settings child object changes, we
            /// have unsaved changes...
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void Value_PropertyChanged(object? sender, PropertyChangedEventArgs e)
            {
                HasUnsavedChanges = true;
            }

            /// <summary>
            /// Forces unsaved changes to be shown.
            /// Can be used for changes that are not tracked automatically
            /// </summary>
            public void Taint()
            {
                HasUnsavedChanges = true;
            }

            /// <summary>
            /// Returns the configuration storage directory. Creates the directory if it doesn't exist
            /// </summary>
            /// <returns></returns>
            private DirectoryInfo GetSettingsDirectory()
            {
                var baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var appFolder = Path.Join(baseFolder, SettingsPath);
                return Directory.CreateDirectory(appFolder);
            }

            /// <summary>
            /// Save the current config
            /// </summary>
            public void Save()
            {
                var dir = GetSettingsDirectory();
                var fileName = Path.Combine(dir.FullName, SettingsFile);

                logger.Info($"Saving config to {fileName}");

                var jsonOptions = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.Indented,
                    // This is important - otherwise it will append to lists, instead of restoring
                    // lists that are saved, see https://github.com/JamesNK/Newtonsoft.Json/issues/2788
                    ObjectCreationHandling = ObjectCreationHandling.Replace,
                };

                var jsonString = JsonConvert.SerializeObject(_settings, jsonOptions);

                using (StreamWriter outputFile = new StreamWriter(fileName))
                {
                    outputFile.Write(jsonString);
                }

                logger.Info($"Wrote config file {fileName}");
                HasUnsavedChanges = false;
            }

            /// <summary>
            /// If this is the first app run, we need to create a settings file structure
            /// </summary>
            private void CreateDefaultSettingsStructure()
            {
                logger.Debug($"No configuration found, creating new configuration file (first app start)");
                var metadata = new SettingsMetadata
                {
                    AppName = GetAppName(),
                    AppVersion = GetAppVersionString()
                };

                Add("metadata", metadata);
                Save();
            }

            /// <summary>
            /// Load the latest config
            /// </summary>
            private void Load()
            {
                var dir = GetSettingsDirectory();
                var fileName = Path.Combine(dir.FullName, SettingsFile);

                if (Path.Exists(fileName))
                {
                    try
                    {
                        string jsonString = File.ReadAllText(fileName);

                        var jsonOptions = new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto,
                            Formatting = Formatting.Indented
                        };

                        var newSettings = JsonConvert.DeserializeObject<Dictionary<string, INotifyPropertyChanged>>(jsonString, jsonOptions);
                        if (newSettings != null)
                        {
                            // Remove event handlers from old settings obejct. This should
                            // never be needed, but just to be clean/on the safe side
                            foreach (var kv in _settings)
                            {
                                kv.Value.PropertyChanged -= Value_PropertyChanged;
                            }

                            _settings.Clear();

                            // Add new event handlers
                            foreach (var kv in newSettings)
                            {
                                Add(kv.Key, kv.Value);
                            }

                            HasUnsavedChanges = false;
                        }
                        else
                        {
                            throw new Exception("Unable to load config file - resulted in null obj");
                        }

                    }
                    catch (Exception loadException)
                    {
                        // Old config will be saved as ".backup" for debug purposes
                        var newFilename = $"{fileName}.backup";
                        // Move the corrupt settings to the backup location
                        File.Move(fileName, newFilename);
                        // Give feedback to the user
                        var result = MessageBox.Show(
                            $"There was an error loading the applicaton configuration.\r\n{loadException.Message}\r\n\r\nConfiguration will be reset. You can find the old configuration as {newFilename}", 
                            "Settings Error", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);
                        // Create default settings
                        CreateDefaultSettingsStructure();
                    }
                }
                else
                {
                    CreateDefaultSettingsStructure();
                }
            }
        }

#if DEBUG
        /// <summary>
        /// Settings storage path for dev builds
        /// We want to have different settings paths for release and dev builds, that way we can test
        /// with one while still actively use the other one.
        /// </summary>
        private static readonly string APP_SETTINGS_PATH = Path.Join(GetAppName(), "dev");
#else
        /// <summary>
        /// Settings storage path for release builds
        /// We want to have different settings paths for release and dev builds, that way we can test
        /// with one while still actively use the other one.
        /// </summary>
        private static readonly string APP_SETTINGS_PATH = Path.Join(GetAppName(), "release");
#endif
        /// <summary>
        /// Default settings file name
        /// </summary>
        private static readonly string APP_SETTINGS_FILE = "settings.json";

        /// <summary>
        /// Internal base for App
        /// </summary>
        private static SettingsRoot? _appSettings;

        /// <summary>
        /// Returns the name of the application
        /// </summary>
        /// <returns></returns>
        public static string GetAppName()
        {
            var name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
         
            if (name == null)
            {
                return "Unknown App";
            }

            return name!;
        }

        /// <summary>
        /// Returns the version of the app as a string
        /// </summary>
        /// <returns></returns>
        public static string GetAppVersionString()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (version == null)
            {
                return "0.0.0";
            }

            return version.ToString();
        }
        
        /// <summary>
        /// Returns the current settings object
        /// </summary>
        public static SettingsRoot App
        {
            get
            {
                if(_appSettings == null)
                {
                    _appSettings = new SettingsRoot(APP_SETTINGS_PATH, APP_SETTINGS_FILE);
                }

                return _appSettings;
            }
        }
    }
}
