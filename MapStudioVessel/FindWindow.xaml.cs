using MahApps.Metro.Controls;
using MeowDSIO.DataTypes.MSB.EVENT_PARAM_ST;
using MeowDSIO.DataTypes.MSB.MODEL_PARAM_ST;
using MeowDSIO.DataTypes.MSB.PARTS_PARAM_ST;
using MeowDSIO.DataTypes.MSB.POINT_PARAM_ST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MeowsBetterParamEditor
{
    /// <summary>
    /// Interaction logic for FindWindow.xaml
    /// </summary>
    public partial class FindWindow : MetroWindow
    {
        public MainDataContext MsbDataContext { get; set; } = null;
        public MSBRef CurrentMsb { get; set; } = null;

        public FindWindow()
        {
            InitializeComponent();
        }

        private void RadioButtonAllMsbs_Click(object sender, RoutedEventArgs e)
        {
            //RadioButtonAllMsbs.IsChecked = !(RadioButtonAllMsbs.IsChecked ?? false);
            RadioButtonCurrentMsb.IsChecked = !(RadioButtonAllMsbs.IsChecked ?? false);
        }

        private void RadioButtonCurrentMsb_Click(object sender, RoutedEventArgs e)
        {
            //RadioButtonCurrentMsb.IsChecked = !(RadioButtonCurrentMsb.IsChecked ?? false);
            RadioButtonAllMsbs.IsChecked = !(RadioButtonCurrentMsb.IsChecked ?? false);
        }

        private Dictionary<string, string> GetProperties<T>(T thing)
        {
            var result = new Dictionary<string, string>();
            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var p in properties)
            {
                result.Add(p.Name, p.GetValue(thing).ToString());
            }
            return result;
        }

        private bool CheckMatch(string val, string search, bool matchCase, bool matchEntireString)
        {
            var valStr = matchCase ? val.Trim() : val.Trim().ToLower();
            var searchStr = matchCase ? search.Trim() : search.Trim().ToLower();
            if (matchEntireString)
            {
                return val == search;
            }
            else
            {
                return val.Contains(search);
            }
        }


        private IEnumerable<MsbSearchResult> SearchMsb(MSBRef msb, string search, bool matchCase, bool matchEntireString)
        {
            var resultsList = new List<MsbSearchResult>();
            (string MainTab, string SubTab, string Row, string Property) Current = ("", "", "", "");

            //void DEBUG_WriteCurrent()
            //{
            //    Console.Write(Current.MainTab);
            //    Console.Write("    ");
            //    Console.Write(Current.SubTab);
            //    Console.Write("    ");
            //    Console.Write(Current.Row);
            //    Console.Write("    ");
            //    Console.Write(Current.Property);
            //    Console.Write("    ");
            //    Console.WriteLine();
            //}

            void CheckAddResult(string fieldValue, object rowObj)
            {
                //DEBUG_WriteCurrent();
                //Console.WriteLine($"Checking {fieldValue}");
                //Console.WriteLine();


                if (CheckMatch(fieldValue, search, matchCase, matchEntireString))
                {
                    resultsList.Add(new MsbSearchResult()
                    {
                        MsbName = msb.FancyDisplayName,
                        PrimaryTab = Current.MainTab,
                        SecondaryTab = Current.SubTab,
                        Row = Current.Row,
                        PropertyName = Current.Property,
                        PropertyValue = fieldValue,
                        ActualRow = rowObj
                    });
                }
            }

            void CheckList<T>(string mainTab, string subTab, IEnumerable<T> list, Func<T, string> getRowName)
            {
                Current.MainTab = mainTab.ToUpper();
                Current.SubTab = subTab;

                //DEBUG_WriteCurrent();
                //Console.WriteLine("Checking List...");

                foreach (var item in list)
                {
                    Current.Row = getRowName(item);

                    //DEBUG_WriteCurrent();
                    //Console.WriteLine("Checking Row...");

                    var properties = GetProperties<T>(item);
                    foreach (var p in properties)
                    {
                        Current.Property = p.Key;
                        CheckAddResult(p.Value, item);
                    }
                }
            }

            CheckList<MsbModelMapPiece>("Models", "Map Pieces", msb.Value.Models.MapPieces, x => $"\"{x.Name}\"");
            CheckList<MsbModelObject>("Models", "Objects", msb.Value.Models.Objects, x => $"\"{x.Name}\"");
            CheckList<MsbModelCharacter>("Models", "Characters", msb.Value.Models.Characters, x => $"\"{x.Name}\"");
            CheckList<MsbModelPlayer>("Models", "Players", msb.Value.Models.Players, x => $"\"{x.Name}\"");
            CheckList<MsbModelCollision>("Models", "Collisions", msb.Value.Models.Collisions, x => $"\"{x.Name}\"");
            CheckList<MsbModelNavimesh>("Models", "Navimeshes", msb.Value.Models.Navimeshes, x => $"\"{x.Name}\"");

            CheckList<MsbEventLight>("Events", "Lights", msb.Value.Events.Lights, x => $"\"{x.Name}\" (Event Idx: {x.EventIndex})");
            CheckList<MsbEventSound>("Events", "Sounds", msb.Value.Events.Sounds, x => $"\"{x.Name}\" (Event Idx: {x.EventIndex})");
            CheckList<MsbEventSFX>("Events", "SFX", msb.Value.Events.SFXs, x => $"\"{x.Name}\" (Event Idx: {x.EventIndex})");
            CheckList<MsbEventWindSFX>("Events", "Wind SFX", msb.Value.Events.WindSFXs, x => $"\"{x.Name}\" (Event Idx: {x.EventIndex})");
            CheckList<MsbEventTreasure>("Events", "Treasures", msb.Value.Events.Treasures, x => $"\"{x.Name}\" (Event Idx: {x.EventIndex})");
            CheckList<MsbEventGenerator>("Events", "Generators", msb.Value.Events.Generators, x => $"\"{x.Name}\" (Event Idx: {x.EventIndex})");
            CheckList<MsbEventBloodMsg>("Events", "Blood Messages", msb.Value.Events.BloodMessages, x => $"\"{x.Name}\" (Event Idx: {x.EventIndex})");
            CheckList<MsbEventObjAct>("Events", "ObjActs", msb.Value.Events.ObjActs, x => $"\"{x.Name}\" (Event Idx: {x.EventIndex})");
            CheckList<MsbEventSpawnPoint>("Events", "Spawn Points", msb.Value.Events.SpawnPoints, x => $"\"{x.Name}\" (Event Idx: {x.EventIndex})");
            CheckList<MsbEventMapOffset>("Events", "Map Offsets", msb.Value.Events.MapOffsets, x => $"\"{x.Name}\" (Event Idx: {x.EventIndex})");
            CheckList<MsbEventNavimesh>("Events", "Navimeshes", msb.Value.Events.Navimeshes, x => $"\"{x.Name}\" (Event Idx: {x.EventIndex})");
            CheckList<MsbEventEnvironment>("Events", "Environment Events", msb.Value.Events.EnvironmentEvents, x => $"\"{x.Name}\" (Event Idx: {x.EventIndex})");
            CheckList<MsbEventBlackEyeOrbInvasion>("Events", "Black Eye Orb Invasions", msb.Value.Events.BlackEyeOrbInvasion, x => $"\"{x.Name}\" (Event Idx: {x.EventIndex})");

            CheckList<MsbRegionPoint>("Regions", "Points", msb.Value.Regions.Points, x => $"\"{x.Name}\" (Idx: {x.Index})");
            CheckList<MsbRegionSphere>("Regions", "Spheres", msb.Value.Regions.Spheres, x => $"\"{x.Name}\" (Idx: {x.Index})");
            CheckList<MsbRegionCylinder>("Regions", "Cylinders", msb.Value.Regions.Cylinders, x => $"\"{x.Name}\" (Idx: {x.Index})");
            CheckList<MsbRegionBox>("Regions", "Boxes", msb.Value.Regions.Boxes, x => $"\"{x.Name}\" (Idx: {x.Index})");

            CheckList<MsbPartsMapPiece>("Parts", "Map Pieces", msb.Value.Parts.MapPieces, x => $"\"{x.Name}\"");
            CheckList<MsbPartsObject>("Parts", "Objects", msb.Value.Parts.Objects, x => $"\"{x.Name}\"");
            CheckList<MsbPartsNPC>("Parts", "NPCs", msb.Value.Parts.NPCs, x => $"\"{x.Name}\"");
            CheckList<MsbPartsPlayer>("Parts", "Players", msb.Value.Parts.Players, x => $"\"{x.Name}\"");
            CheckList<MsbPartsCollision>("Parts", "Collisions", msb.Value.Parts.Collisions, x => $"\"{x.Name}\"");
            CheckList<MsbPartsNavimesh>("Parts", "Navimeshes", msb.Value.Parts.Navimeshes, x => $"\"{x.Name}\"");
            CheckList<MsbPartsObjectUnused>("Parts", "Unused Objects", msb.Value.Parts.UnusedObjects, x => $"\"{x.Name}\"");
            CheckList<MsbPartsNPCUnused>("Parts", "Unused NPCs", msb.Value.Parts.UnusedNPCs, x => $"\"{x.Name}\"");
            CheckList<MsbPartsCollisionUnused>("Parts", "Unused Collisions", msb.Value.Parts.UnusedCollisions, x => $"\"{x.Name}\"");

            return resultsList;
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                ButtonSearch.IsEnabled = false;
            }), DispatcherPriority.ContextIdle);

            Dispatcher.BeginInvoke(new Action(() =>
            {

                IEnumerable<MSBRef> msbsToSearch = new List<MSBRef>();
                if (RadioButtonAllMsbs.IsChecked == true)
                {
                    msbsToSearch = MsbDataContext.MSBs;
                }
                else if (RadioButtonCurrentMsb.IsChecked == true)
                {
                    msbsToSearch = new List<MSBRef>() { CurrentMsb };
                }

                IEnumerable<MsbSearchResult> searchResults = new List<MsbSearchResult>();

                foreach (var msb in msbsToSearch)
                {
                    searchResults = searchResults.Concat(
                        SearchMsb(
                            msb: msb, 
                            search: TextBoxSearch.Text, 
                            matchCase: CheckBoxMatchCase.IsChecked ?? false, 
                            matchEntireString: CheckBoxMatchEntireString.IsChecked ?? false));
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    SearchResultsDataGrid.ItemsSource = searchResults;
                    LabelResultsHeader.Content = $"Results ({searchResults.Count()}):";
                    ButtonSearch.IsEnabled = true;
                }), DispatcherPriority.ContextIdle);


            }), DispatcherPriority.ContextIdle);

            
        }
    }
}
