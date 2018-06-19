using MeowDSIO;
using MeowDSIO.DataFiles;
using MeowDSIO.DataTypes.PARAM;
using MeowDSIO.DataTypes.PARAMDEF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace MeowsBetterParamEditor
{
    public class MainDataContext : INotifyPropertyChanged
    {
        private UserConfig _config = new UserConfig();
        public UserConfig Config
        {
            get => _config;
            set
            {
                _config = value;
                NotifyPropertyChanged(nameof(Config));
            }
        }

        public void LoadConfig()
        {
            if (File.Exists(UserConfigPath))
            {
                string cfgJson = File.ReadAllText(UserConfigPath);
                Config = Newtonsoft.Json.JsonConvert.DeserializeObject<UserConfig>(cfgJson);
            }
            else
            {
                Config = new UserConfig();
                SaveConfig();
            }
        }

        public void SaveConfig()
        {
            if (File.Exists(UserConfigPath))
            {
                File.Delete(UserConfigPath);
            }
            string cfgJson = Newtonsoft.Json.JsonConvert.SerializeObject(Config, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(UserConfigPath, cfgJson);
        }



        //private MsbSectorFormat _primary_tab = MsbSectorFormat.NONE;

        //public MsbSectorFormat PRIMARY_TAB
        //{
        //    get => _primary_tab;
        //    set
        //    {
        //        if (_primary_tab != value)
        //        {
        //            _primary_tab = value;
        //            NotifyPropertyChanged(nameof(PRIMARY_TAB));
        //            NotifyPropertyChanged(nameof(SECONDARY_TAB_LIST));
        //            NotifyPropertyChanged(nameof(SECONDARY_TAB));
        //        }
        //    }
        //}

        //private static readonly ObservableCollection<SecondaryTabType> TAB_LIST_MODEL_PARAM_ST = new ObservableCollection<SecondaryTabType>()
        //{
        //    SecondaryTabType.MODEL_PARAM_ST,
        //};

        //private static readonly ObservableCollection<SecondaryTabType> TAB_LIST_EVENT_PARAM_ST = new ObservableCollection<SecondaryTabType>()
        //{
        //    SecondaryTabType.EVENT_PARAM_ST_Lights,
        //    SecondaryTabType.EVENT_PARAM_ST_Sounds,
        //    SecondaryTabType.EVENT_PARAM_ST_SFX,
        //    SecondaryTabType.EVENT_PARAM_ST_WindSFX,
        //    SecondaryTabType.EVENT_PARAM_ST_Treasures,
        //    SecondaryTabType.EVENT_PARAM_ST_Generators,
        //    SecondaryTabType.EVENT_PARAM_ST_BloodMsg,
        //    SecondaryTabType.EVENT_PARAM_ST_ObjActs,
        //    SecondaryTabType.EVENT_PARAM_ST_SpawnPoints,
        //    SecondaryTabType.EVENT_PARAM_ST_MapOffset,
        //    SecondaryTabType.EVENT_PARAM_ST_Navimesh,
        //    SecondaryTabType.EVENT_PARAM_ST_Environment,
        //    SecondaryTabType.EVENT_PARAM_ST_BlackEyeOrbInvasions,
        //};
        //private static readonly ObservableCollection<SecondaryTabType> TAB_LIST_POINT_PARAM_ST = new ObservableCollection<SecondaryTabType>()
        //{
        //    SecondaryTabType.POINT_PARAM_ST_Points,
        //    SecondaryTabType.POINT_PARAM_ST_Spheres,
        //    SecondaryTabType.POINT_PARAM_ST_Cylinders,
        //    SecondaryTabType.POINT_PARAM_ST_Boxes,
        //};
        //private static readonly ObservableCollection<SecondaryTabType> TAB_LIST_PARTS_PARAM_ST = new ObservableCollection<SecondaryTabType>()
        //{
        //    SecondaryTabType.PARTS_PARAM_ST_MapPieces,
        //    SecondaryTabType.PARTS_PARAM_ST_Objects,
        //    SecondaryTabType.PARTS_PARAM_ST_NPCs,
        //    SecondaryTabType.PARTS_PARAM_ST_Players,
        //    SecondaryTabType.PARTS_PARAM_ST_Collisions,
        //    SecondaryTabType.PARTS_PARAM_ST_Navimeshes,
        //    SecondaryTabType.PARTS_PARAM_ST_UnusedObjects,
        //    SecondaryTabType.PARTS_PARAM_ST_UnusedNPCs,
        //    SecondaryTabType.PARTS_PARAM_ST_UnusedCollisions,
        //};
        //private static readonly ObservableCollection<SecondaryTabType> TAB_LIST_NONE = new ObservableCollection<SecondaryTabType>() { };

        //private ObservableCollection<SecondaryTabType> _secondary_tab_list = new ObservableCollection<SecondaryTabType>();
        //public ObservableCollection<SecondaryTabType> SECONDARY_TAB_LIST
        //{
        //    get => _secondary_tab_list;
        //    set
        //    {
        //        switch (PRIMARY_TAB)
        //        {
        //            case MsbSectorFormat.MODEL_PARAM_ST:
        //                _secondary_tab_list = TAB_LIST_MODEL_PARAM_ST;
        //                break;
        //            case MsbSectorFormat.EVENT_PARAM_ST:
        //                _secondary_tab_list = TAB_LIST_EVENT_PARAM_ST;
        //                break;
        //            case MsbSectorFormat.POINT_PARAM_ST:
        //                _secondary_tab_list = TAB_LIST_POINT_PARAM_ST;
        //                break;
        //            case MsbSectorFormat.PARTS_PARAM_ST:
        //                _secondary_tab_list = TAB_LIST_PARTS_PARAM_ST;
        //                break;
        //            default:
        //                _secondary_tab_list = TAB_LIST_NONE;
        //                break;
        //        }
        //    }
        //}

        //private SecondaryTabType _secondary_tab = SecondaryTabType.MODEL_PARAM_ST;
        //public SecondaryTabType SECONDARY_TAB
        //{
        //    get => _secondary_tab;
        //    set
        //    {
        //        if (_secondary_tab != value)
        //        {
        //            _secondary_tab = value;
        //            NotifyPropertyChanged(nameof(SECONDARY_TAB));
        //        }
        //    }
        //}

        private IEnumerable _current_list = null;
        public IEnumerable CURRENT_LIST
        {
            get => _current_list;
            set
            {
                if (_current_list != value)
                {
                    _current_list = value;
                    NotifyPropertyChanged(nameof(CURRENT_LIST));
                }
            }
        }

        public string UserConfigPath => IOHelper.Frankenpath(Environment.CurrentDirectory, CONFIG_FILE);

        public const string CONFIG_FILE = "MapStudioVessel_UserConfig.json";

        private ObservableCollection<MSBRef> _msbs = new ObservableCollection<MSBRef>();

        public ObservableCollection<MSBRef> MSBs
        {
            get => _msbs;
            set
            {
                _msbs = value;
                NotifyPropertyChanged(nameof(MSBs));
            }
        }

        public async Task LoadParamsInOtherThread(Action<bool> setIsLoading)
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    setIsLoading?.Invoke(true);
                });

                LoadAllPARAMs();

                //Application.Current.Dispatcher.Invoke(() =>
                //{
                //    Mouse.OverrideCursor = null;
                //    setIsLoading?.Invoke(false);
                //});
            });
        }

        private void LoadAllPARAMs()
        {
            if (Config?.InterrootPath == null)
                return;

            var UPCOMING_MSBs = new ObservableCollection<MSBRef>();

            //MSBTODO: Load the fucking MSBs :trashcat:

            foreach (var msbFile in Directory.GetFiles(Config.MSBFolder, "*.msb", SearchOption.TopDirectoryOnly))
            {
                var newMsb = DataFile.LoadFromFile<MSB>(msbFile);
                UPCOMING_MSBs.Add(new MSBRef(new FileInfo(msbFile).Name, newMsb));
            }


            Application.Current.Dispatcher.Invoke(() =>
            {
                MSBs = UPCOMING_MSBs;
            });
        }

        public async Task SaveInOtherThread(Action<bool> setIsLoading)
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    setIsLoading?.Invoke(true);
                });

                var backupsCreated = new List<string>();
                SaveAllPARAMs(backupsCreated);

                if (backupsCreated.Count > 0)
                {
                    var sb = new StringBuilder();

                    sb.AppendLine("The following MSB file backup(s) did not exist and had to be created before saving:");

                    foreach (var b in backupsCreated)
                    {
                        sb.AppendLine($"\t'{b.Replace(Config.InterrootPath, ".")}'");
                    }

                    sb.AppendLine();

                    sb.AppendLine("Note: previously-created backups are NEVER overridden by this application. " +
                        "Subsequent file save operations will not display a messagebox if a backup of every file already exists.");

                    MessageBox.Show(sb.ToString(), "Backups Created", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                SaveConfig();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    setIsLoading?.Invoke(false);
                });
            });
        }

        public async Task RestoreBackupsInOtherThread(Action<bool> setIsLoading)
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    setIsLoading?.Invoke(true);
                });

                foreach (var msb in MSBs)
                {
                    if (msb.Value.RestoreBackup() != true)
                    {
                        throw new Exception("Backups didn't restore wtf - Tell Meowmaritus OMEGALUL");
                    }

                    DataFile.Reload(msb.Value);
                }

                LoadAllPARAMs();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    setIsLoading?.Invoke(false);
                });
            });
        }

        private void SaveAllPARAMs(List<string> backupsCreated)
        {
            foreach (var msb in MSBs)
            {
                if(msb.Value.CreateBackup(overwriteExisting: false) == true)
                {
                    backupsCreated.Add(msb.Value.FileBackupPath);
                }

                try
                {
                    DataFile.Resave(msb.Value);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Error while saving MSB \"{msb.FancyDisplayName}\": \n\n{e.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
