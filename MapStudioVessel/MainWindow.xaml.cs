using MahApps.Metro.Controls;
using MeowDSIO;
using MeowDSIO.DataFiles;
using MeowDSIO.DataTypes.MSB;
using MeowDSIO.DataTypes.MSB.EVENT_PARAM_ST;
using MeowDSIO.DataTypes.MSB.MODEL_PARAM_ST;
using MeowDSIO.DataTypes.MSB.PARTS_PARAM_ST;
using MeowDSIO.DataTypes.MSB.POINT_PARAM_ST;
using MeowDSIO.DataTypes.PARAM;
using MeowDSIO.DataTypes.PARAMDEF;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MeowsBetterParamEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private FindWindow FIND = null;

        public MainWindow()
        {
            InitializeComponent();

            //DEBUG
            //PARAMDATA.DEBUG_RestoreBackupsLoadResave();

#if REMASTER
            Title += " - !DARK SOULS REMASTERED VERSION!";
#endif
        }

        private Dictionary<IEnumerable, double> TAB_SCROLLS_H = new Dictionary<IEnumerable, double>();
        private Dictionary<IEnumerable, double> TAB_SCROLLS_V = new Dictionary<IEnumerable, double>();

        private List<string> DeSOnlyFields = new List<string>() { "SfxUnkIndex", "MsgParam", "NpcSubUnk0", "PlayerSubUnk" };
        private List<string> DS1OnlyFields = new List<string>() { "InChest", "StartDisabled", "SeekGuidanceOnly", "PartName", "ThinkParamID", "VagrantID1", "VagrantID2", "VagrantID3", "DisableStart", "DisableBonfireID", "PlayRegionID", "LockCamID1", "LockCamID2" };
        
        private void SetLoadingMode(bool isLoading)
        {
            MainGrid.Opacity = isLoading ? 0.25 : 1;
            MainGrid.IsEnabled = !isLoading;

            LoadingTextBox.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;

            Mouse.OverrideCursor = isLoading ? Cursors.Wait : null;
        }

        private void SetSavingMode(bool isLoading)
        {
            MainGrid.Opacity = isLoading ? 0.25 : 1;
            MainGrid.IsEnabled = !isLoading;

            SavingTextBox.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;

            Mouse.OverrideCursor = isLoading ? Cursors.Wait : null;
        }
        
        private async Task BrowseForInterrootDialog(Action<bool> setIsLoading)
        {
            var browseDialog = new OpenFileDialog()
            {
                AddExtension = false,
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                FileName = "DarkSoulsRemastered.exe",
                Filter = "Executable files (*.exe)|*.exe",
                ShowReadOnly = false,
                Title = "Choose your DarkSoulsRemastered.exe file...",
                ValidateNames = true
            };

            if ((browseDialog.ShowDialog() ?? false) == true)
            {
                var interrootDir = new FileInfo(browseDialog.FileName).DirectoryName;
                if (CheckInterrootDirValid(interrootDir))
                {
                    PARAMDATA.Config.InterrootPath = interrootDir;
                    PARAMDATA.SaveConfig();
                    await PARAMDATA.LoadParamsInOtherThread(setIsLoading);
                }
                else
                {
                    var sb = new StringBuilder();

                    sb.AppendLine(@"Directory of EXE chosen did not include the following directories/files which are required:");
                    sb.AppendLine(@" - '.\map\MapStudio\'");

                    //if (CheckIfUdsfmIsProbablyNotInstalled(interrootDir))
                    //{
                    //    sb.AppendLine();
                    //    sb.AppendLine();
                    //    sb.AppendLine("This installation does not appear to be unpacked with " +
                    //        "UnpackDarkSoulsForModding because it meets one or more of the " +
                    //        "criteria below (please note that it is only a suggestion and not " +
                    //        "required for this tool to function; only the above criteria is " +
                    //        "required to be met in order to use this tool).");

                    //    sb.AppendLine(@" - '.\unpackDS-backup' does not exist.");
                    //    sb.AppendLine(@" - '.\dvdbnd0.bdt' exists.");
                    //    sb.AppendLine(@" - '.\dvdbnd1.bdt' exists.");
                    //    sb.AppendLine(@" - '.\dvdbnd2.bdt' exists.");
                    //    sb.AppendLine(@" - '.\dvdbnd3.bdt' exists.");
                    //    sb.AppendLine(@" - '.\dvdbnd0.bhd5' exists.");
                    //    sb.AppendLine(@" - '.\dvdbnd1.bhd5' exists.");
                    //    sb.AppendLine(@" - '.\dvdbnd2.bhd5' exists.");
                    //    sb.AppendLine(@" - '.\dvdbnd3.bhd5' exists.");
                    //}

                    MessageBox.Show(
                        sb.ToString(), 
                        "Invalid Directory", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error);

                }
            }
        }

        //private bool CheckIfUdsfmIsProbablyNotInstalled(string dir)
        //{
        //    return !Directory.Exists(IOHelper.Frankenpath(dir, @"unpackDS-backup")) ||
        //        File.Exists(IOHelper.Frankenpath(dir, @"dvdbnd0.bdt")) ||
        //        File.Exists(IOHelper.Frankenpath(dir, @"dvdbnd1.bdt")) ||
        //        File.Exists(IOHelper.Frankenpath(dir, @"dvdbnd2.bdt")) ||
        //        File.Exists(IOHelper.Frankenpath(dir, @"dvdbnd3.bdt")) ||
        //        File.Exists(IOHelper.Frankenpath(dir, @"dvdbnd0.bhd5")) ||
        //        File.Exists(IOHelper.Frankenpath(dir, @"dvdbnd1.bhd5")) ||
        //        File.Exists(IOHelper.Frankenpath(dir, @"dvdbnd2.bhd5")) ||
        //        File.Exists(IOHelper.Frankenpath(dir, @"dvdbnd3.bhd5"));
        //}

        private bool CheckInterrootDirValid(string dir)
        {
            return
                Directory.Exists(IOHelper.Frankenpath(dir, @"map\MapStudio"));
        }

        public bool CheckMapDirValid(string dir)
        {
            return
                System.IO.Directory.Exists(IOHelper.Frankenpath(dir, @"MapStudio"));
        }

        //private bool CheckMapDirValid(string dir)
        //{
        //    return
        //        Directory.Exists(IOHelper.Frankenpath(dir, @"MapStudio"));
        //}
        //private void RANDOM_DEBUG_TESTING()
        //{
        //    //var uniqueInternalDataTypes = new List<string>();
        //    //foreach (var p in PARAMDATA.ParamDefs)
        //    //{
        //    //    string testDir = IOHelper.Frankenpath(Environment.CurrentDirectory, "VERBOSE_DUMP");

        //    //    if (!Directory.Exists(testDir))
        //    //        Directory.CreateDirectory(testDir);

        //    //    string testFileName = IOHelper.Frankenpath(testDir, p.Key + ".txt");

        //    //    var sb = new StringBuilder();

        //    //    foreach (var e in p.Value.Entries)
        //    //    {
        //    //        sb.AppendLine($"{e.Name}:");
        //    //        sb.AppendLine($"\tID: {e.ID}");
        //    //        sb.AppendLine($"\tInternal Value Type: {e.InternalValueType}");
        //    //        sb.AppendLine($"\tDefault: {e.DefaultValue}");
        //    //        sb.AppendLine($"\t[PARAM MAN] Display Name: {e.DisplayName}:");
        //    //        sb.AppendLine($"\t[PARAM MAN] Description: {e.Description}");
        //    //        sb.AppendLine($"\t[PARAM MAN] Min: {e.Min}");
        //    //        sb.AppendLine($"\t[PARAM MAN] Max: {e.Max}");
        //    //        sb.AppendLine($"\t[PARAM MAN] Incrementation: {e.Increment}");
        //    //        sb.AppendLine($"\t[PARAM MAN] GUI Value Format: \"{e.GuiValueStringFormat}\"");
        //    //        sb.AppendLine($"\t[PARAM MAN] GUI Value Type: {e.GuiValueType}");
        //    //        sb.AppendLine($"\t[PARAM MAN] GUI Value Mode: {e.GuiValueDisplayMode}");
        //    //        sb.AppendLine($"\t[PARAM MAN] GUI Value Size: {e.GuiValueByteCount}");
        //    //        sb.AppendLine();
        //    //    }

        //    //    File.WriteAllText(testFileName, sb.ToString());
        //    //}

        //    var sb = new StringBuilder();

        //    foreach (var p in PARAMDATA.Params)
        //    {
        //        int defSize = p.Value.AppliedPARAMDEF.CalculateEntrySize();

        //        if (p.Value._debug_calculatedEntrySize != defSize)
        //        {
        //            sb.AppendLine($"Entry size check fail - {p.BNDName} - {p.Value.Name} - {p.Value._debug_calculatedEntrySize} (Def: {defSize})");

        //            //foreach (var e in p.Value.AppliedPARAMDEF.Entries)
        //            //{

        //            //    sb.AppendLine($"\tDef Entry size check fail ({e.Name}) - BitCount = {e.ValueBitCount}, DispVarBytes*8 = {(e.GuiValueByteCount * 8)}");
        //            //}
        //        }
        //    }

        //    Console.WriteLine(sb.ToString());

        //    Console.WriteLine();
        //}

        public MSB SelectedMsb
        {
            get
            {
                if (MainTabs.SelectedValue != null)
                    return (MainTabs.SelectedItem as MSBRef).Value;
                else
                    return null;
            }
        }

        private void UltraSuperMegaDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void UltraSuperMegaDataGrid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void MenuSelectDarkSoulsDirectory_Click(object sender, RoutedEventArgs e)
        {
            var setup = new SelectGameWindow()
            {
                Owner = this
            };
            if (setup.ShowDialog() ?? false)
            {
                await PARAMDATA.LoadParamsInOtherThread(SetLoadingMode);
                PARAMDATA.SaveConfig();
                AdjustVisibilitySettings();
            }
            
        }

        private void AdjustVisibilitySettings()
        {
            if (PARAMDATA.Config.Game == "DeSPS3")
            {
                TabEventsObjActs.Visibility = Visibility.Collapsed;
                TabEventsSpawnPoints.Visibility = Visibility.Collapsed;
                TabEventsEnvironmentEvents.Visibility = Visibility.Collapsed;
                TabEventsMapOffsets.Visibility = Visibility.Collapsed;
                TabEventsNavimeshes.Visibility = Visibility.Collapsed;
                TabEventsEnvironmentEvents.Visibility = Visibility.Collapsed;
                TabEventsBlackEyeOrbInvasions.Visibility = Visibility.Collapsed;
            }
            else
            {
                TabEventsObjActs.Visibility = Visibility.Visible;
                TabEventsSpawnPoints.Visibility = Visibility.Visible;
                TabEventsEnvironmentEvents.Visibility = Visibility.Visible;
                TabEventsMapOffsets.Visibility = Visibility.Visible;
                TabEventsNavimeshes.Visibility = Visibility.Visible;
                TabEventsEnvironmentEvents.Visibility = Visibility.Visible;
                TabEventsBlackEyeOrbInvasions.Visibility = Visibility.Visible;
            }
        }

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PARAMDATA.LoadConfig();

            if (!string.IsNullOrWhiteSpace(PARAMDATA.Config?.MapFolder) && CheckMapDirValid(PARAMDATA.Config?.MapFolder))
            {
                await PARAMDATA.LoadParamsInOtherThread(SetLoadingMode);
            }
            else
            {
                //if (MessageBox.Show(
                //    //"note: a dark souls installation unpacked by the mod " +
                //    //"'unpackdarksoulsformodding' by hotpocketremix is >>>required<<<." +
                //    //"\n" +
                //    @"please navigate to your 'darksouls.exe' or 'darksoulsremastered.exe' file." +
                //    "once the inital setup is performed, the path will be saved." +
                //    "\nyou may press cancel to continue without selecting the path but the gui will " +
                //    "be blank until you go to 'file -> select dark souls directory...'",
                //    "initial setup", MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
                //{
                //    await BrowseForInterrootDialog(SetLoadingMode);
                //}
                var setup = new SelectGameWindow()
                {
                    Owner = this
                };
                if (setup.ShowDialog() ?? false)
                {
                    await PARAMDATA.LoadParamsInOtherThread(SetLoadingMode);
                    PARAMDATA.SaveConfig();
                }
            }

            MainTabs.Items.Refresh();
            AdjustVisibilitySettings();

            //TOP_SECRET_DEBUG_REPORT_FUNCTION();
        }

        private void TOP_SECRET_DEBUG_REPORT_FUNCTION()
        {
            var MAIN_DICT = new Dictionary<string, Dictionary<string, List<object>>>();

            foreach (var msb in PARAMDATA.MSBs)
            {
                var thisMsbDict = msb.Value.DebugGenerateUnknownFieldReport(null, includeBases: true, includeSubtypes: true);

                foreach (var outerKvp in thisMsbDict)
                {
                    if (!MAIN_DICT.ContainsKey(outerKvp.Key))
                        MAIN_DICT.Add(outerKvp.Key, new Dictionary<string, List<object>>());

                    foreach (var innerKvp in outerKvp.Value)
                    {
                        if (!MAIN_DICT[outerKvp.Key].ContainsKey(innerKvp.Key))
                            MAIN_DICT[outerKvp.Key].Add(innerKvp.Key, new List<object>());

                        foreach (var innerKvpListItem in innerKvp.Value)
                        {
                            if (!MAIN_DICT[outerKvp.Key][innerKvp.Key].Contains(innerKvpListItem))
                                MAIN_DICT[outerKvp.Key][innerKvp.Key].Add(innerKvpListItem);
                        }

                        
                    }
                }
            }

            StringBuilder sb = new StringBuilder();

            foreach (var outerKvp in MAIN_DICT)
            {
                sb.AppendLine(outerKvp.Key);
                foreach (var innerKvp in outerKvp.Value)
                {
                    sb.AppendLine($"    {innerKvp.Key}");

                    foreach (var v in innerKvp.Value)
                    {
                        sb.AppendLine($"            {v.ToString()}");
                    }
                    sb.AppendLine("    ");
                }
                sb.AppendLine();
                sb.AppendLine();
            }

            Clipboard.SetText(sb.ToString());
            MessageBox.Show("TOP SECRET DEBUG REPORT: COPIED TO CLIPBOARD");
        }

        private void CmdSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private bool CheckSave()
        {
            return true;
            //foreach (var msb in PARAMDATA.MSBs)
            //{
            //    var list = msb.Value.Regions.GlobalList;

            //    for (int i = 0; i < list.Count; i++)
            //    foreach (var r in msb.Value.Regions.GlobalList)
            //    {

            //    }
            //}
        }

        private async void CmdSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (CheckSave())
            {
                await PARAMDATA.SaveInOtherThread(SetSavingMode);
            }
        }

        private void MainTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainTabs == null)
                return;

            UPDATE_DATA_GRID();
        }

        private void ParamEntryList_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            //SetLoadingMode(false);
        }

        private void ParamEntryList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        private void ParamEntryList_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //TODO: Use e.Cancel, asking user if they wanna save changes and all that

            //Even if the user decides not to save the params, always save the config:
            PARAMDATA.SaveConfig();
        }

        private void MainTabs_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            SetLoadingMode(false);
        }

        private async void MenuRestoreBackups_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to restore all backups and undo any custom modifications you've ever done?!", "RESET EVERYTHING?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await PARAMDATA.RestoreBackupsInOtherThread(SetLoadingMode);
            }
        }

        private void ParamManStyleDataGrid_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            var about = new About
            {
                Owner = this
            };
            about.ShowDialog();
        }

        private static T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            T child = default(T);

            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        private void StoreTabScroll()
        {
            if (PARAMDATA.CURRENT_LIST == null)
                return;

            ScrollViewer sv = GetVisualChild<ScrollViewer>(MSB_DATA_GRID);

            if (!TAB_SCROLLS_H.ContainsKey(PARAMDATA.CURRENT_LIST))
                TAB_SCROLLS_H.Add(PARAMDATA.CURRENT_LIST, sv.HorizontalOffset);
            else
                TAB_SCROLLS_H[PARAMDATA.CURRENT_LIST] = sv.HorizontalOffset;

            if (!TAB_SCROLLS_V.ContainsKey(PARAMDATA.CURRENT_LIST))
                TAB_SCROLLS_V.Add(PARAMDATA.CURRENT_LIST, sv.VerticalOffset);
            else
                TAB_SCROLLS_V[PARAMDATA.CURRENT_LIST] = sv.VerticalOffset;
        }

        private void LoadTabScroll()
        {
            if (PARAMDATA.CURRENT_LIST == null)
                return;

            ScrollViewer sv = GetVisualChild<ScrollViewer>(MSB_DATA_GRID);

            if (TAB_SCROLLS_H.ContainsKey(PARAMDATA.CURRENT_LIST))
            {
                sv.ScrollToHorizontalOffset(TAB_SCROLLS_H[PARAMDATA.CURRENT_LIST]);
            }
            else
            {
                sv.ScrollToHorizontalOffset(0);
            }

            if (TAB_SCROLLS_V.ContainsKey(PARAMDATA.CURRENT_LIST))
            {
                sv.ScrollToVerticalOffset(TAB_SCROLLS_V[PARAMDATA.CURRENT_LIST]);
            }
            else
            {
                sv.ScrollToVerticalOffset(0);
            }
        }

        public void UPDATE_DATA_GRID()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                StoreTabScroll();

                //DisableHexColumns();

                if (SelectedMsb == null)
                {
                    PARAMDATA.CURRENT_LIST = null;
                    return;
                }

                if (TabsPrimary.SelectedItem == TabModels)
                {
                    PARAMDATA.CURRENT_LIST = SelectedMsb.Models;

                    if (TabModels_Tabs.SelectedItem == TabModelsCharacters)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Models.Characters;
                    else if (TabModels_Tabs.SelectedItem == TabModelsCollisions)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Models.Collisions;
                    else if (TabModels_Tabs.SelectedItem == TabModelsMapPieces)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Models.MapPieces;
                    else if (TabModels_Tabs.SelectedItem == TabModelsNavimeshes)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Models.Navimeshes;
                    else if (TabModels_Tabs.SelectedItem == TabModelsObjects)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Models.Objects;
                    else if (TabModels_Tabs.SelectedItem == TabModelsPlayers)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Models.Players;

                }
                else if (TabsPrimary.SelectedItem == TabEvents)
                {
                    if (TabEvents_Tabs.SelectedItem == TabEventsBlackEyeOrbInvasions)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Events.NpcWorldInvitations;
                    else if (TabEvents_Tabs.SelectedItem == TabEventsBloodMessages)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Events.BloodMessages;
                    else if (TabEvents_Tabs.SelectedItem == TabEventsEnvironmentEvents)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Events.EnvLightMapSpot;
                    else if (TabEvents_Tabs.SelectedItem == TabEventsGenerators)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Events.Generators;
                    else if (TabEvents_Tabs.SelectedItem == TabEventsLights)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Events.Lights;
                    else if (TabEvents_Tabs.SelectedItem == TabEventsMapOffsets)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Events.MapOffsets;
                    else if (TabEvents_Tabs.SelectedItem == TabEventsNavimeshes)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Events.Navimeshes;
                    else if (TabEvents_Tabs.SelectedItem == TabEventsObjActs)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Events.ObjActs;
                    else if (TabEvents_Tabs.SelectedItem == TabEventsSFX)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Events.SFXs;
                    else if (TabEvents_Tabs.SelectedItem == TabEventsSounds)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Events.Sounds;
                    else if (TabEvents_Tabs.SelectedItem == TabEventsSpawnPoints)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Events.SpawnPoints;
                    else if (TabEvents_Tabs.SelectedItem == TabEventsTreasures)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Events.Treasures;
                    else if (TabEvents_Tabs.SelectedItem == TabEventsWindSFX)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Events.WindSFXs;
                }
                else if (TabsPrimary.SelectedItem == TabRegions)
                {
                    if (TabRegions_Tabs.SelectedItem == TabRegionsPoints)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Regions.Points;
                    else if (TabRegions_Tabs.SelectedItem == TabRegionsSpheres)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Regions.Spheres;
                    else if (TabRegions_Tabs.SelectedItem == TabRegionsCylinders)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Regions.Cylinders;
                    else if (TabRegions_Tabs.SelectedItem == TabRegionsBoxes)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Regions.Boxes;
                }
                else if (TabsPrimary.SelectedItem == TabParts)
                {
                    //EnableHexColumns_DrawGroup();
                    //EnableHexColumns_DispGroup();

                    if (TabParts_Tabs.SelectedItem == TabPartsCollisions)
                    {
                        //EnableHexColumns_NaviMeshGroup();
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Parts.Hits;
                    }
                    else if (TabParts_Tabs.SelectedItem == TabPartsMapPieces)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Parts.MapPieces;
                    else if (TabParts_Tabs.SelectedItem == TabPartsNavimeshes)
                    {
                        //EnableHexColumns_NaviMeshGroup();
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Parts.Navimeshes;
                    }
                    else if (TabParts_Tabs.SelectedItem == TabPartsNPCs)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Parts.NPCs;
                    else if (TabParts_Tabs.SelectedItem == TabPartsObjects)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Parts.Objects;
                    else if (TabParts_Tabs.SelectedItem == TabPartsPlayers)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Parts.Players;
                    else if (TabParts_Tabs.SelectedItem == TabPartsUnusedCollisions)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Parts.ConnectHits;
                    else if (TabParts_Tabs.SelectedItem == TabPartsUnusedNPCs)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Parts.DummyNPCs;
                    else if (TabParts_Tabs.SelectedItem == TabPartsUnusedObjects)
                        PARAMDATA.CURRENT_LIST = SelectedMsb.Parts.DummyObjects;
                }
            }));


        }

        private void TabsPrimary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UPDATE_DATA_GRID();
        }

        private void TabModels_Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UPDATE_DATA_GRID();
        }

        private void TabEvents_Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UPDATE_DATA_GRID();
        }

        private void TabRegions_Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UPDATE_DATA_GRID();
        }

        private void TabParts_Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UPDATE_DATA_GRID();
        }

        private void MSB_DATA_GRID_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            if (SelectedMsb == null)
                return;

            if (TabsPrimary.SelectedItem == TabEvents)
            {
                int nextEventIndex = SelectedMsb?.Events.GetNextIndex() ?? 0;

                if (TabEvents_Tabs.SelectedItem == TabEventsBlackEyeOrbInvasions)
                {
                    e.NewItem = new MsbEventNpcWorldInvitation()
                    {
                        Name = $"BlackEyeOrbInvasion_{nextEventIndex}",
                        Index = SelectedMsb.Events.GetNextIndex(EventParamSubtype.BlackEyeOrbInvasions)
                    };
                }
                else if (TabEvents_Tabs.SelectedItem == TabEventsBloodMessages)
                {
                    e.NewItem = new MsbEventBloodMsg()
                    {
                        Name = $"BloodMessage_{nextEventIndex}",
                        Index = SelectedMsb.Events.GetNextIndex(EventParamSubtype.BloodMsg)
                    };
                }
                else if (TabEvents_Tabs.SelectedItem == TabEventsEnvironmentEvents)
                {
                    e.NewItem = new MsbEventEnvironment()
                    {
                        Name = $"EnvironmentEvent_{nextEventIndex}",
                        Index = SelectedMsb.Events.GetNextIndex(EventParamSubtype.Environment)
                    };
                }
                else if (TabEvents_Tabs.SelectedItem == TabEventsGenerators)
                {
                    e.NewItem = new MsbEventGenerator()
                    {
                        Name = $"Generator_{nextEventIndex}",
                        Index = SelectedMsb.Events.GetNextIndex(EventParamSubtype.Generators)
                    };
                }
                else if (TabEvents_Tabs.SelectedItem == TabEventsLights)
                {
                    e.NewItem = new MsbEventLight()
                    {
                        Name = $"Light_{nextEventIndex}",
                        Index = SelectedMsb.Events.GetNextIndex(EventParamSubtype.Lights)
                    };
                }
                else if (TabEvents_Tabs.SelectedItem == TabEventsMapOffsets)
                {
                    e.NewItem = new MsbEventMapOffset()
                    {
                        Name = $"MapOffset_{nextEventIndex}",
                        Index = SelectedMsb.Events.GetNextIndex(EventParamSubtype.MapOffset)
                    };
                }
                else if (TabEvents_Tabs.SelectedItem == TabEventsNavimeshes)
                {
                    e.NewItem = new MsbEventNavimesh()
                    {
                        Name = $"Navimesh_{nextEventIndex}",
                        Index = SelectedMsb.Events.GetNextIndex(EventParamSubtype.Navimesh)
                    };
                }
                else if (TabEvents_Tabs.SelectedItem == TabEventsObjActs)
                {
                    e.NewItem = new MsbEventObjAct()
                    {
                        Name = $"ObjAct_{nextEventIndex}",
                        Index = SelectedMsb.Events.GetNextIndex(EventParamSubtype.ObjActs)
                    };
                }
                else if (TabEvents_Tabs.SelectedItem == TabEventsSFX)
                {
                    e.NewItem = new MsbEventSFX()
                    {
                        Name = $"SFX_{nextEventIndex}",
                        Index = SelectedMsb.Events.GetNextIndex(EventParamSubtype.SFX)
                    };
                }
                else if (TabEvents_Tabs.SelectedItem == TabEventsSounds)
                {
                    e.NewItem = new MsbEventSound()
                    {
                        Name = $"Sound_{nextEventIndex}",
                        Index = SelectedMsb.Events.GetNextIndex(EventParamSubtype.Sounds)
                    };
                }
                else if (TabEvents_Tabs.SelectedItem == TabEventsSpawnPoints)
                {
                    e.NewItem = new MsbEventSpawnPoint()
                    {
                        Name = $"SpawnPoint_{nextEventIndex}",
                        Index = SelectedMsb.Events.GetNextIndex(EventParamSubtype.SpawnPoints)
                    };
                }
                else if (TabEvents_Tabs.SelectedItem == TabEventsTreasures)
                {
                    e.NewItem = new MsbEventTreasure()
                    {
                        Name = $"Treasure_{nextEventIndex}",
                        Index = SelectedMsb.Events.GetNextIndex(EventParamSubtype.Treasures)
                    };
                }
                else if (TabEvents_Tabs.SelectedItem == TabEventsWindSFX)
                {
                    e.NewItem = new MsbEventWindSFX()
                    {
                        Name = $"WindSFX_{nextEventIndex}",
                        Index = SelectedMsb.Events.GetNextIndex(EventParamSubtype.WindSFX)
                    };
                }

                (e.NewItem as MsbEventBase).EventIndex = nextEventIndex;
            }
            else if (TabsPrimary.SelectedItem == TabModels)
            {
                if (TabModels_Tabs.SelectedItem == TabModelsMapPieces)
                {
                    e.NewItem = new MsbModelMapPiece()
                    {
                        Name = "m0000B0",
                        PlaceholderModel = $@"N:\FRPG\data\Model\map\{PARAMDATA.MSBs.Where(x => x.Value == SelectedMsb).First().FancyDisplayName}\sib\m0000B0.sib",
                        InstanceCount = 1,
                        Index = SelectedMsb.Models.GetNextIndex(MeowDSIO.DataTypes.MSB.ModelParamSubtype.MapPiece)
                    };
                }
                else if (TabModels_Tabs.SelectedItem == TabModelsObjects)
                {
                    e.NewItem = new MsbModelObject()
                    {
                        Name = "o0500",
                        PlaceholderModel = @"N:\FRPG\data\Model\obj\o0500\sib\o0500.sib",
                        InstanceCount = 1,
                        Index = SelectedMsb.Models.GetNextIndex(MeowDSIO.DataTypes.MSB.ModelParamSubtype.Object)
                    };
                }
                else if (TabModels_Tabs.SelectedItem == TabModelsCharacters)
                {
                    e.NewItem = new MsbModelCharacter()
                    {
                        Name = "c4100",
                        PlaceholderModel = @"N:\FRPG\data\Model\chr\c4100\sib\c4100.sib",
                        InstanceCount = 1,
                        Index = SelectedMsb.Models.GetNextIndex(MeowDSIO.DataTypes.MSB.ModelParamSubtype.Character)
                    };
                }
                else if (TabModels_Tabs.SelectedItem == TabModelsPlayers)
                {
                    e.NewItem = new MsbModelPlayer()
                    {
                        Name = "c0000",
                        PlaceholderModel = @"N:\FRPG\data\Model\chr\c0000\sib\c0000.SIB",
                        InstanceCount = 1,
                        Index = SelectedMsb.Models.GetNextIndex(MeowDSIO.DataTypes.MSB.ModelParamSubtype.Player)
                    };
                }
                else if (TabModels_Tabs.SelectedItem == TabModelsCollisions)
                {
                    e.NewItem = new MsbModelCollision()
                    {
                        Name = "h0000B0",
                        PlaceholderModel = $@"N:\FRPG\data\Model\map\{PARAMDATA.MSBs.Where(x => x.Value == SelectedMsb).First().FancyDisplayName}\hkxwin\h0000B0.hkxwin",
                        InstanceCount = 1,
                        Index = SelectedMsb.Models.GetNextIndex(MeowDSIO.DataTypes.MSB.ModelParamSubtype.Collision)
                    };
                }
                else if (TabModels_Tabs.SelectedItem == TabModelsNavimeshes)
                {
                    e.NewItem = new MsbModelNavimesh()
                    {
                        Name = "n0000B0",
                        PlaceholderModel = $@"N:\FRPG\data\Model\map\{PARAMDATA.MSBs.Where(x => x.Value == SelectedMsb).First().FancyDisplayName}\navimesh\n0000B0.sib",
                        InstanceCount = 1,
                        Index = SelectedMsb.Models.GetNextIndex(MeowDSIO.DataTypes.MSB.ModelParamSubtype.Navimesh)
                    };
                }
            }
            else if (TabsPrimary.SelectedItem == TabRegions)
            {
                var newRegionIndex = SelectedMsb.Regions.GetNextIndex();

                if (TabRegions_Tabs.SelectedItem == TabRegionsPoints)
                    e.NewItem = new MsbRegionPoint(SelectedMsb.Regions) { Name = $"Point_{newRegionIndex}" };
                else if (TabRegions_Tabs.SelectedItem == TabRegionsSpheres)
                    e.NewItem = new MsbRegionSphere(SelectedMsb.Regions) { Name = $"Sphere_{newRegionIndex}" };
                else if (TabRegions_Tabs.SelectedItem == TabRegionsCylinders)
                    e.NewItem = new MsbRegionCylinder(SelectedMsb.Regions) { Name = $"Cylinder_{newRegionIndex}" };
                else if (TabRegions_Tabs.SelectedItem == TabRegionsBoxes)
                    e.NewItem = new MsbRegionBox(SelectedMsb.Regions) { Name = $"Box_{newRegionIndex}" };
            }
            else if (TabsPrimary.SelectedItem == TabParts)
            {
                var nextPartsIndex = 0;

                if (TabParts_Tabs.SelectedItem == TabPartsCollisions)
                {
                    nextPartsIndex = SelectedMsb.Parts.GetNextIndex(PartsParamSubtype.Hits);
                    e.NewItem = new MsbPartsHit() { Name = $"Collision_{nextPartsIndex}", Index = nextPartsIndex };
                }
                else if (TabParts_Tabs.SelectedItem == TabPartsMapPieces)
                {
                    nextPartsIndex = SelectedMsb.Parts.GetNextIndex(PartsParamSubtype.MapPieces);
                    e.NewItem = new MsbPartsMapPiece() { Name = $"MapPiece_{nextPartsIndex}", Index = nextPartsIndex };
                }
                else if (TabParts_Tabs.SelectedItem == TabPartsNavimeshes)
                {
                    nextPartsIndex = SelectedMsb.Parts.GetNextIndex(PartsParamSubtype.Navimeshes);
                    e.NewItem = new MsbPartsNavimesh() { Name = $"Navimesh_{nextPartsIndex}", Index = nextPartsIndex };
                }
                else if (TabParts_Tabs.SelectedItem == TabPartsNPCs)
                {
                    nextPartsIndex = SelectedMsb.Parts.GetNextIndex(PartsParamSubtype.NPCs);
                    e.NewItem = new MsbPartsNPC() { Name = $"NPC_{nextPartsIndex}", Index = nextPartsIndex };
                }
                else if (TabParts_Tabs.SelectedItem == TabPartsObjects)
                {
                    nextPartsIndex = SelectedMsb.Parts.GetNextIndex(PartsParamSubtype.Objects);
                    e.NewItem = new MsbPartsObject() { Name = $"Object_{nextPartsIndex}", Index = nextPartsIndex };
                }
                else if (TabParts_Tabs.SelectedItem == TabPartsPlayers)
                {
                    nextPartsIndex = SelectedMsb.Parts.GetNextIndex(PartsParamSubtype.Players);
                    e.NewItem = new MsbPartsPlayer() { Name = $"Player_{nextPartsIndex}", Index = nextPartsIndex };
                }
                else if (TabParts_Tabs.SelectedItem == TabPartsUnusedCollisions)
                {
                    nextPartsIndex = SelectedMsb.Parts.GetNextIndex(PartsParamSubtype.ConnectHits);
                    e.NewItem = new MsbPartsConnectHit() { Name = $"UnusedCollision_{nextPartsIndex}", Index = nextPartsIndex };
                }
                else if (TabParts_Tabs.SelectedItem == TabPartsUnusedNPCs)
                {
                    nextPartsIndex = SelectedMsb.Parts.GetNextIndex(PartsParamSubtype.DummyNPCs);
                    e.NewItem = new MsbPartsNPCDummy() { Name = $"UnusedNPC_{nextPartsIndex}", Index = nextPartsIndex };
                }
                else if (TabParts_Tabs.SelectedItem == TabPartsUnusedObjects)
                {
                    nextPartsIndex = SelectedMsb.Parts.GetNextIndex(PartsParamSubtype.DummyObjects);
                    e.NewItem = new MsbPartsObjectDummy() { Name = $"UnusedObject_{nextPartsIndex}", Index = nextPartsIndex };
                }
            }




        }

        private void CmdFind_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (FIND == null || !FIND.IsVisible);
        }

        private void CmdFind_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (FIND == null || !FIND.IsVisible)
            {
                FIND = new FindWindow
                {
                    Owner = this,
                    MsbDataContext = PARAMDATA
                };

                if (MainTabs.SelectedValue != null)
                    FIND.CurrentMsb = (MainTabs.SelectedItem as MSBRef);
                else
                    FIND.CurrentMsb = null;

                FIND.SearchResultsDataGrid.SelectionChanged += SearchResultsDataGrid_SelectionChanged;

                FIND.Show();
            }
        }

        private void GoToMsbSearchResult(MsbSearchResult r)
        {
            Dispatcher.Invoke(() =>
            {
                //try
                //{
                MainTabs.SelectedItem = PARAMDATA.MSBs.Where(x => x.FancyDisplayName.ToUpper() == r.MsbName.ToUpper()).First();

                void SwitchTabs(TabControl tc, string tabName)
                {
                    bool foundTab = false;
                    foreach (var tab in tc.Items)
                    {
                        if (((TabItem)tab).Header.ToString().ToUpper() == tabName.ToUpper())
                        {
                            tc.SelectedItem = tab;
                            foundTab = true;
                            break;
                        }
                    }
                    if (!foundTab)
                    {
                        throw new Exception($"Unable to find tab. Info:\n{r.ToString()}");
                    }
                }

                if (r.PrimaryTab == "MODELS")
                {
                    TabsPrimary.SelectedItem = TabModels;
                    SwitchTabs(TabModels_Tabs, r.SecondaryTab);
                }
                else if (r.PrimaryTab == "EVENTS")
                {
                    TabsPrimary.SelectedItem = TabEvents;
                    SwitchTabs(TabEvents_Tabs, r.SecondaryTab);
                }
                else if (r.PrimaryTab == "REGIONS")
                {
                    TabsPrimary.SelectedItem = TabRegions;
                    SwitchTabs(TabRegions_Tabs, r.SecondaryTab);
                }
                else if (r.PrimaryTab == "PARTS")
                {
                    TabsPrimary.SelectedItem = TabParts;
                    SwitchTabs(TabParts_Tabs, r.SecondaryTab);
                }

                int columnIndex = -1;
                bool foundColumn = false;
                for (int i = 0; i < MSB_DATA_GRID.Columns.Count; i++)
                {
                    if (MSB_DATA_GRID.Columns[i].Header.ToString().ToUpper() == r.PropertyName.ToUpper())
                    {
                        columnIndex = i;
                        foundColumn = true;
                        break;
                    }
                }

                if (!foundColumn)
                {
                    throw new Exception($"Unable to find column. Info:\n{r.ToString()}");
                }

                var selectCell = new DataGridCellInfo(r.ActualRow, MSB_DATA_GRID.Columns[columnIndex]);

                MSB_DATA_GRID.SelectedCells.Clear();
                MSB_DATA_GRID.SelectedCells.Add(selectCell);
                MSB_DATA_GRID.ScrollIntoView(r.ActualRow, MSB_DATA_GRID.Columns[columnIndex]);
                //}
                //catch
                //{
                //    System.Threading.Thread.Sleep(250);
                //    GoToMsbSearchResult(r);
                //}
                

            });

        }

        private void SearchResultsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                var r = (MsbSearchResult)item;


                GoToMsbSearchResult(r);

                break;
            }
        }

        private void MSB_DATA_GRID_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (MSB_DATA_GRID.SelectedItem == null && MSB_DATA_GRID.SelectedItems.Count == 0)
                {
                    return;
                }

                var isMulti = (MSB_DATA_GRID.SelectedItems.Count > 1);

                var userChoice = MessageBox.Show($"Are you sure you want to delete " +
                    $"the {(isMulti ? MSB_DATA_GRID.SelectedItems.Count.ToString() + " " : "")}" +
                    $"selected row{(isMulti ? "s" : "")}?\nThis cannot be undone.\n" +
                    $"Any references to the selected row{(isMulti ? "s" : "")} " +
                    $"will be removed automatically.", 
                    $"Delete Row{(isMulti ? "s" : "")}?", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (userChoice != MessageBoxResult.Yes)
                {
                    e.Handled = true;
                    return;
                }

                void removeItem(object item)
                {
                    if (TabsPrimary.SelectedItem == TabRegions)
                    {
                        if (TabRegions_Tabs.SelectedItem == TabRegionsPoints)
                            SelectedMsb.Regions.Points.Remove(item as MsbRegionPoint);
                        else if (TabRegions_Tabs.SelectedItem == TabRegionsSpheres)
                            SelectedMsb.Regions.Spheres.Remove(item as MsbRegionSphere);
                        else if (TabRegions_Tabs.SelectedItem == TabRegionsCylinders)
                            SelectedMsb.Regions.Cylinders.Remove(item as MsbRegionCylinder);
                        else if (TabRegions_Tabs.SelectedItem == TabRegionsBoxes)
                            SelectedMsb.Regions.Boxes.Remove(item as MsbRegionBox);
                    }
                    else if (TabsPrimary.SelectedItem == TabModels)
                    {
                        var modelToRemove = item as MsbModelBase;

                        foreach (var thing in SelectedMsb.Parts)
                        {
                            if (thing.ModelName == modelToRemove.Name)
                            {
                                thing.ModelName = "";
                            }
                        }

                        if (TabModels_Tabs.SelectedItem == TabModelsCharacters)
                            SelectedMsb.Models.Characters.Remove(item as MsbModelCharacter);
                        else if (TabModels_Tabs.SelectedItem == TabModelsCollisions)
                            SelectedMsb.Models.Collisions.Remove(item as MsbModelCollision);
                        else if (TabModels_Tabs.SelectedItem == TabModelsMapPieces)
                            SelectedMsb.Models.MapPieces.Remove(item as MsbModelMapPiece);
                        else if (TabModels_Tabs.SelectedItem == TabModelsNavimeshes)
                            SelectedMsb.Models.Navimeshes.Remove(item as MsbModelNavimesh);
                        else if (TabModels_Tabs.SelectedItem == TabModelsObjects)
                            SelectedMsb.Models.Objects.Remove(item as MsbModelObject);
                        else if (TabModels_Tabs.SelectedItem == TabModelsPlayers)
                            SelectedMsb.Models.Players.Remove(item as MsbModelPlayer);
                    }
                }

                void overrideDelete(bool observableCollection)
                {
                    e.Handled = true;

                    if (isMulti)
                    {
                        if (observableCollection)
                        {
                            while (MSB_DATA_GRID.SelectedItems.Count > 0)
                            {
                                removeItem(MSB_DATA_GRID.SelectedItems[0]);
                            }
                        }
                        else
                        {
                            foreach (var thingToDelete in MSB_DATA_GRID.SelectedItems)
                            {
                                removeItem(thingToDelete);
                            }
                        }
                        
                    }
                    else
                    {
                        removeItem(MSB_DATA_GRID.SelectedItem);
                    }

                    MSB_DATA_GRID.Items.Refresh();
                }

                if (TabsPrimary.SelectedItem == TabRegions)
                {
                    overrideDelete(observableCollection: true);
                }
                else if (TabsPrimary.SelectedItem == TabModels)
                {
                    overrideDelete(observableCollection: false);
                }

            }

        }

        private void MSB_DATA_GRID_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            LoadTabScroll();
        }

        //private void EnableHexColumns_DrawGroup()
        //{
        //    HardcodedColumn_LoadIndex(HardcodedColumn_DrawGroup);
        //    HardcodedColumn_DrawGroup.Visibility = Visibility.Visible;
        //}

        //private void EnableHexColumns_DispGroup()
        //{
        //    HardcodedColumn_LoadIndex(HardcodedColumn_DispGroup);
        //    HardcodedColumn_DispGroup.Visibility = Visibility.Visible;
        //}

        //private void EnableHexColumns_NaviMeshGroup()
        //{
        //    HardcodedColumn_LoadIndex(HardcodedColumn_NavimeshGroup);
        //    HardcodedColumn_NavimeshGroup.Visibility = Visibility.Visible;
        //}

        //private void DisableHexColumns()
        //{
        //    HardcodedColumn_DrawGroup.Visibility = Visibility.Collapsed;
        //    HardcodedColumn_DispGroup.Visibility = Visibility.Collapsed;
        //    HardcodedColumn_NavimeshGroup.Visibility = Visibility.Collapsed;
        //}


        private Dictionary<DataGridColumn, DataGridColumn> DisplayIndexMap = new Dictionary<DataGridColumn, DataGridColumn>();
        private void HardcodedColumn_LoadIndex(DataGridColumn c)
        {
            if (DisplayIndexMap.ContainsKey(c))
            {
                if (DisplayIndexMap[c].DisplayIndex >= 0)
                  c.DisplayIndex = DisplayIndexMap[c].DisplayIndex;
            }
        }
        private void HardCodedColumn_SaveIndex(DataGridColumn c, DataGridColumn mapTo)
        {
            if (!DisplayIndexMap.ContainsKey(c))
                DisplayIndexMap.Add(c, mapTo);
            else
                DisplayIndexMap[c] = mapTo;
        }
        private void MSB_DATA_GRID_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
               e.Column = null;
            }
            else
            {
                //if (e.PropertyName == "DrawGroup")
                //{
                //    HardCodedColumn_SaveIndex(HardcodedColumn_DrawGroup, e.Column);
                //    e.Column.Visibility = Visibility.Collapsed;
                //}
                //else if (e.PropertyName == "DispGroup")
                //{
                //    HardCodedColumn_SaveIndex(HardcodedColumn_DispGroup, e.Column);
                //    e.Column.Visibility = Visibility.Collapsed;
                //}
                //else if (e.PropertyName == "NavimeshGroup")
                //{
                //    HardCodedColumn_SaveIndex(HardcodedColumn_NavimeshGroup, e.Column);
                //    e.Column.Visibility = Visibility.Collapsed;
                //}
            }

            if (PARAMDATA.Config.Game == "DeSPS3")
            {
                if (e.PropertyName.Contains("RefTexID") || DeSOnlyFields.Contains(e.PropertyName))
                {
                    e.Column.Visibility = Visibility.Visible;
                }
                else if (DS1OnlyFields.Contains(e.PropertyName))
                {
                    e.Column.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                if (e.PropertyName.Contains("RefTexID") || DeSOnlyFields.Contains(e.PropertyName))
                {
                    e.Column.Visibility = Visibility.Collapsed;
                }
                else if (DS1OnlyFields.Contains(e.PropertyName))
                {
                    e.Column.Visibility = Visibility.Visible;
                }
            }

        }
    }
}
