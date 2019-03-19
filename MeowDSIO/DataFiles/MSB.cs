using MeowDSIO.DataTypes.MSB;
using MeowDSIO.DataTypes.MSB.EVENT_PARAM_ST;
using MeowDSIO.DataTypes.MSB.MODEL_PARAM_ST;
using MeowDSIO.DataTypes.MSB.PARTS_PARAM_ST;
using MeowDSIO.DataTypes.MSB.POINT_PARAM_ST;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataFiles
{
    public enum MsbSectorFormat
    {
        NONE,
        MODEL_PARAM_ST,
        EVENT_PARAM_ST,
        POINT_PARAM_ST,
        PARTS_PARAM_ST,
    }

    public class MSB : DataFile
    {
        public Dictionary<string, Dictionary<string, List<object>>> DebugGenerateUnknownFieldReport(
            string mapNameToIncludeInFieldNameNullForNoMapName = null, 
            bool includeBases = true, 
            bool includeSubtypes = true)
        {
            //<StructName, <FieldName, ValueList>>
            var MAIN_DICT = new Dictionary<string, Dictionary<string, List<object>>>();

            void RegisterDebugReportData(string structName, Dictionary<string, object> basetypeReport, Dictionary<string, object> subtypeReport)
            {
                if (!MAIN_DICT.ContainsKey(structName))
                {
                    MAIN_DICT.Add(structName, new Dictionary<string, List<object>>());
                }

                if (includeBases)
                {
                    foreach (var kvp in basetypeReport)
                    {
                        if (!MAIN_DICT[structName].ContainsKey(kvp.Key))
                            MAIN_DICT[structName].Add(kvp.Key, new List<object>());

                        if (!MAIN_DICT[structName][kvp.Key].Contains(kvp.Value))
                        {
                            if (mapNameToIncludeInFieldNameNullForNoMapName != null)
                                MAIN_DICT[structName][kvp.Key].Add($"[{mapNameToIncludeInFieldNameNullForNoMapName}] {(kvp.Value.ToString())}");
                            else
                                MAIN_DICT[structName][kvp.Key].Add(kvp.Value);
                        }
                    }
                }

                if (includeSubtypes)
                {
                    foreach (var kvp in subtypeReport)
                    {
                        if (!MAIN_DICT[structName].ContainsKey(kvp.Key))
                            MAIN_DICT[structName].Add(kvp.Key, new List<object>());

                        if (!MAIN_DICT[structName][kvp.Key].Contains(kvp.Value))
                        {
                            if (mapNameToIncludeInFieldNameNullForNoMapName != null)
                                MAIN_DICT[structName][kvp.Key].Add($"[{mapNameToIncludeInFieldNameNullForNoMapName}] {(kvp.Value.ToString())}");
                            else
                                MAIN_DICT[structName][kvp.Key].Add(kvp.Value);
                        }
                    }
                }
            }

            foreach (var thing in Events.GlobalList)
            {
                var baseReport = new Dictionary<string, object>();
                var subtypeReport = new Dictionary<string, object>();
                thing.DebugPushUnknownFieldReport(out string basetypeName, out string subtypeName, baseReport, subtypeReport);
                RegisterDebugReportData($"{basetypeName}::{subtypeName}", baseReport, subtypeReport);
            }

            foreach (var thing in Parts.GlobalList)
            {
                var baseReport = new Dictionary<string, object>();
                var subtypeReport = new Dictionary<string, object>();
                thing.DebugPushUnknownFieldReport(out string basetypeName, out string subtypeName, baseReport, subtypeReport);
                RegisterDebugReportData($"{basetypeName}::{subtypeName}", baseReport, subtypeReport);
            }

            foreach (var thing in Regions.GlobalList)
            {
                var baseReport = new Dictionary<string, object>();
                var subtypeReport = new Dictionary<string, object>();
                thing.DebugPushUnknownFieldReport(out string basetypeName, out string subtypeName, baseReport, subtypeReport);
                RegisterDebugReportData($"{basetypeName}::{subtypeName}", baseReport, subtypeReport);
            }

            return MAIN_DICT;
        }


        public int Unknown1;

        public MsbModelList Models = new MsbModelList();
        public MsbEventList Events = new MsbEventList();
        public MsbRegionList Regions = new MsbRegionList();
        public MsbPartsList Parts = new MsbPartsList();


        /*
int Unknown1;

int MODEL_PARAM_NameOffset;
int MODEL_PARAM_Count;
int[] MODEL_PARAM_Pointers[MODEL_PARAM_Count];
int startOfNextSectionOffset;
int end = 0;

int EVENT_PARAM_NameOffset;
int EVENT_PARAM_Count;
int[] EVENT_PARAM_Pointers[EVENT_PARAM_Count];

int POINT_PARAM_NameOffset;
int POINT_PARAM_Count;
int[] POINT_PARAM_Pointers[POINT_PARAM_Count];

int PARTS_PARAM_NameOffset;
int PARTS_PARAM_Count;
int[] PARTS_PARAM_Pointers[PARTS_PARAM_Count];

            each struct thing:

            int nameOffset;

            int otherStringOffsetEtc;

            <data>

            string name (nameOffset points here)

            string otherStringEtc (otherStringOffsetEtc points here etc)


         */

        private void UnregisterRegionUpdates()
        {
            if (Regions != null)
            {
                Regions.Points.CollectionChanged -= Points_CollectionChanged;
                Regions.Boxes.CollectionChanged -= Boxes_CollectionChanged;
                Regions.Spheres.CollectionChanged -= Spheres_CollectionChanged;
                Regions.Cylinders.CollectionChanged -= Cylinders_CollectionChanged;
            }
        }

        private void RegisterRegionUpdates()
        {
            Regions.Points.CollectionChanged += Points_CollectionChanged;
            Regions.Boxes.CollectionChanged += Boxes_CollectionChanged;
            Regions.Spheres.CollectionChanged += Spheres_CollectionChanged;
            Regions.Cylinders.CollectionChanged += Cylinders_CollectionChanged;
        }

        //private void CheckRegionContinuity<T>(PointParamSubtype t, ObservableCollection<T> list)
        //    where T : MsbRegionBase
        //{
        //    foreach (var ev in Events)
        //    {
        //        ev.Region 

        //        if (ev.RegionIndex < 0)
        //            continue;

        //        if (ev.RegionType == t)
        //        {
        //            var possibleRegionRefs = list.Where(x => x.Index == ev.RegionIndex);
        //            if (possibleRegionRefs.Any())
        //                ev.RegionIndex = list.IndexOf(possibleRegionRefs.First());
        //            else
        //                ev.RegionIndex = -1; //Region no longer exists, so point to -1;
        //        }
        //    }

        //    if (t == PointParamSubtype.Points)
        //    {
        //        foreach (var npc in Parts.NPCs)
        //        {
        //            if (npc.MovePointIndex1 >= 0)
        //            {
        //                var possibleRegionRefs = list.Where(x => x.Index == npc.MovePointIndex1);
        //                if (possibleRegionRefs.Any())
        //                    npc.MovePointIndex1 = (short)list.IndexOf(possibleRegionRefs.First());
        //                else
        //                    npc.MovePointIndex1 = -1; //Region no longer exists, so point to -1;
        //            }

        //            if (npc.MovePointIndex2 >= 0)
        //            {
        //                var possibleRegionRefs = list.Where(x => x.Index == npc.MovePointIndex2);
        //                if (possibleRegionRefs.Any())
        //                    npc.MovePointIndex2 = (short)list.IndexOf(possibleRegionRefs.First());
        //                else
        //                    npc.MovePointIndex2 = -1; //Region no longer exists, so point to -1;
        //            }

        //            if (npc.MovePointIndex3 >= 0)
        //            {
        //                var possibleRegionRefs = list.Where(x => x.Index == npc.MovePointIndex3);
        //                if (possibleRegionRefs.Any())
        //                    npc.MovePointIndex3 = (short)list.IndexOf(possibleRegionRefs.First());
        //                else
        //                    npc.MovePointIndex3 = -1; //Region no longer exists, so point to -1;
        //            }

        //            if (npc.MovePointIndex4 >= 0)
        //            {
        //                var possibleRegionRefs = list.Where(x => x.Index == npc.MovePointIndex4);
        //                if (possibleRegionRefs.Any())
        //                    npc.MovePointIndex4 = (short)list.IndexOf(possibleRegionRefs.First());
        //                else
        //                    npc.MovePointIndex4 = -1; //Region no longer exists, so point to -1;
        //            }
        //        }

        //        foreach (var npc in Parts.UnusedNPCs)
        //        {
        //            if (npc.MovePointIndex1 >= 0)
        //            {
        //                var possibleRegionRefs = list.Where(x => x.Index == npc.MovePointIndex1);
        //                if (possibleRegionRefs.Any())
        //                    npc.MovePointIndex1 = (short)list.IndexOf(possibleRegionRefs.First());
        //                else
        //                    npc.MovePointIndex1 = -1; //Region no longer exists, so point to -1;
        //            }

        //            if (npc.MovePointIndex2 >= 0)
        //            {
        //                var possibleRegionRefs = list.Where(x => x.Index == npc.MovePointIndex2);
        //                if (possibleRegionRefs.Any())
        //                    npc.MovePointIndex2 = (short)list.IndexOf(possibleRegionRefs.First());
        //                else
        //                    npc.MovePointIndex2 = -1; //Region no longer exists, so point to -1;
        //            }

        //            if (npc.MovePointIndex3 >= 0)
        //            {
        //                var possibleRegionRefs = list.Where(x => x.Index == npc.MovePointIndex3);
        //                if (possibleRegionRefs.Any())
        //                    npc.MovePointIndex3 = (short)list.IndexOf(possibleRegionRefs.First());
        //                else
        //                    npc.MovePointIndex3 = -1; //Region no longer exists, so point to -1;
        //            }

        //            if (npc.MovePointIndex4 >= 0)
        //            {
        //                var possibleRegionRefs = list.Where(x => x.Index == npc.MovePointIndex4);
        //                if (possibleRegionRefs.Any())
        //                    npc.MovePointIndex4 = (short)list.IndexOf(possibleRegionRefs.First());
        //                else
        //                    npc.MovePointIndex4 = -1; //Region no longer exists, so point to -1;
        //            }
        //        }
        //    }

            

        //    int i = 0;
        //    foreach (var r in list)
        //    {
        //        r.Index = i++;
        //    }
        //}

        private void Cylinders_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //CheckRegionContinuity(PointParamSubtype.Cylinders, Regions.Cylinders);
        }

        private void Spheres_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //CheckRegionContinuity(PointParamSubtype.Spheres, Regions.Spheres);
        }

        private void Boxes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //CheckRegionContinuity(PointParamSubtype.Boxes, Regions.Boxes);
        }

        private void Points_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //CheckRegionContinuity(PointParamSubtype.Points, Regions.Points);
        }

        protected override void Read(DSBinaryReader bin, IProgress<(int, int)> prog)
        {
            Unknown1 = bin.ReadInt32();

            UnregisterRegionUpdates();

            Models = new MsbModelList();
            Events = new MsbEventList();
            Regions = new MsbRegionList();
            Parts = new MsbPartsList();

            var OriginalIndexOrder_Model = new List<MsbModelBase>();
            var OriginalIndexOrder_Parts = new List<MsbPartsBase>();
            var OriginalIndexOrder_Regions = new List<MsbRegionBase>();
            var OriginalIndexOrder_Events = new List<MsbEventBase>();

            RegisterRegionUpdates();

            MsbSectorFormat currentSectorFormat = MsbSectorFormat.NONE;

            do
            {
                int currentSectorNameOffset = bin.ReadInt32();

                if (currentSectorNameOffset == 0)
                    break;

                bin.StepIn(currentSectorNameOffset);
                {
                    currentSectorFormat = (MsbSectorFormat)Enum.Parse(typeof(MsbSectorFormat), bin.ReadStringAscii());
                }
                bin.StepOut();

                int structCount = bin.ReadInt32() - 1;

                for (int i = 0; i < structCount; i++)
                {
                    int structOffset = bin.ReadInt32();

                    bin.StepIn(structOffset);
                    {
                        switch (currentSectorFormat)
                        {
                            case MsbSectorFormat.MODEL_PARAM_ST:
                                var modelType = ModelParamSubtype.MapPiece;
                                bin.StepIn(bin.Position + 0x04);
                                {
                                    modelType = (ModelParamSubtype)bin.ReadInt32();
                                }
                                bin.StepOut();

                                switch (modelType)
                                {
                                    case ModelParamSubtype.Character:
                                        var newMMCh = new MsbModelCharacter();
                                        newMMCh.Read(bin);
                                        OriginalIndexOrder_Model.Add(newMMCh);
                                        Models.Characters.Add(newMMCh);
                                        break;
                                    case ModelParamSubtype.Collision:
                                        var newMMCol = new MsbModelCollision();
                                        newMMCol.Read(bin);
                                        OriginalIndexOrder_Model.Add(newMMCol);
                                        Models.Collisions.Add(newMMCol);
                                        break;
                                    case ModelParamSubtype.MapPiece:
                                        var newMMMP = new MsbModelMapPiece();
                                        newMMMP.Read(bin);
                                        OriginalIndexOrder_Model.Add(newMMMP);
                                        Models.MapPieces.Add(newMMMP);
                                        break;
                                    case ModelParamSubtype.Navimesh:
                                        var newMMNVM = new MsbModelNavimesh();
                                        newMMNVM.Read(bin);
                                        OriginalIndexOrder_Model.Add(newMMNVM);
                                        Models.Navimeshes.Add(newMMNVM);
                                        break;
                                    case ModelParamSubtype.Object:
                                        var newMMO = new MsbModelObject();
                                        newMMO.Read(bin);
                                        OriginalIndexOrder_Model.Add(newMMO);
                                        Models.Objects.Add(newMMO);
                                        break;
                                    case ModelParamSubtype.Player:
                                        var newMMP = new MsbModelPlayer();
                                        newMMP.Read(bin);
                                        OriginalIndexOrder_Model.Add(newMMP);
                                        Models.Players.Add(newMMP);
                                        break;
                                }
                                break;
                            case MsbSectorFormat.EVENT_PARAM_ST:
                                var eventType = EventParamSubtype.Lights;
                                bin.StepIn(bin.Position + 0x08);
                                {
                                    eventType = (EventParamSubtype)bin.ReadInt32();
                                }
                                bin.StepOut();

                                switch (eventType)
                                {
                                    case EventParamSubtype.BlackEyeOrbInvasions:
                                        var newMEBEOI = new MsbEventNpcWorldInvitation();
                                        newMEBEOI.Read(bin);
                                        OriginalIndexOrder_Events.Add(newMEBEOI);
                                        Events.NpcWorldInvitations.Add(newMEBEOI);
                                        break;
                                    case EventParamSubtype.BloodMsg:
                                        var newMEBM = new MsbEventBloodMsg();
                                        newMEBM.Read(bin);
                                        OriginalIndexOrder_Events.Add(newMEBM);
                                        Events.BloodMessages.Add(newMEBM);
                                        break;
                                    case EventParamSubtype.Environment:
                                        var newMEE = new MsbEventEnvironment();
                                        newMEE.Read(bin);
                                        OriginalIndexOrder_Events.Add(newMEE);
                                        Events.EnvLightMapSpot.Add(newMEE);
                                        break;
                                    case EventParamSubtype.Generators:
                                        var newMEG = new MsbEventGenerator();
                                        newMEG.Read(bin);
                                        OriginalIndexOrder_Events.Add(newMEG);
                                        Events.Generators.Add(newMEG);
                                        break;
                                    case EventParamSubtype.Lights:
                                        var newMEL = new MsbEventLight();
                                        newMEL.Read(bin);
                                        OriginalIndexOrder_Events.Add(newMEL);
                                        Events.Lights.Add(newMEL);
                                        break;
                                    case EventParamSubtype.MapOffset:
                                        var newMEMO = new MsbEventMapOffset();
                                        newMEMO.Read(bin);
                                        OriginalIndexOrder_Events.Add(newMEMO);
                                        Events.MapOffsets.Add(newMEMO);
                                        break;
                                    case EventParamSubtype.Navimesh:
                                        var newMEN = new MsbEventNavimesh();
                                        newMEN.Read(bin);
                                        OriginalIndexOrder_Events.Add(newMEN);
                                        Events.Navimeshes.Add(newMEN);
                                        break;
                                    case EventParamSubtype.ObjActs:
                                        var newMEOA = new MsbEventObjAct();
                                        newMEOA.Read(bin);
                                        OriginalIndexOrder_Events.Add(newMEOA);
                                        Events.ObjActs.Add(newMEOA);
                                        break;
                                    case EventParamSubtype.SFX:
                                        var newMESFX = new MsbEventSFX();
                                        newMESFX.Read(bin);
                                        OriginalIndexOrder_Events.Add(newMESFX);
                                        Events.SFXs.Add(newMESFX);
                                        break;
                                    case EventParamSubtype.Sounds:
                                        var newMES = new MsbEventSound();
                                        newMES.Read(bin);
                                        OriginalIndexOrder_Events.Add(newMES);
                                        Events.Sounds.Add(newMES);
                                        break;
                                    case EventParamSubtype.SpawnPoints:
                                        var newMESP = new MsbEventSpawnPoint();
                                        newMESP.Read(bin);
                                        OriginalIndexOrder_Events.Add(newMESP);
                                        Events.SpawnPoints.Add(newMESP);
                                        break;
                                    case EventParamSubtype.Treasures:
                                        var newMET = new MsbEventTreasure();
                                        newMET.Read(bin);
                                        OriginalIndexOrder_Events.Add(newMET);
                                        Events.Treasures.Add(newMET);
                                        break;
                                    case EventParamSubtype.WindSFX:
                                        var newMEWS = new MsbEventWindSFX();
                                        newMEWS.Read(bin);
                                        OriginalIndexOrder_Events.Add(newMEWS);
                                        Events.WindSFXs.Add(newMEWS);
                                        break;
                                }
                                break;
                            case MsbSectorFormat.POINT_PARAM_ST:
                                var regionType = PointParamSubtype.Points;
                                bin.StepIn(bin.Position + 0x0C);
                                {
                                    regionType = (PointParamSubtype)bin.ReadInt32();
                                }
                                bin.StepOut();

                                switch (regionType)
                                {
                                    case PointParamSubtype.Points:
                                        var newMRP = new MsbRegionPoint(Regions);
                                        newMRP.Read(bin);
                                        OriginalIndexOrder_Regions.Add(newMRP);
                                        Regions.Points.Add(newMRP);
                                        break;
                                    case PointParamSubtype.Spheres:
                                        var newMRS = new MsbRegionSphere(Regions);
                                        newMRS.Read(bin);
                                        OriginalIndexOrder_Regions.Add(newMRS);
                                        Regions.Spheres.Add(newMRS);
                                        break;
                                    case PointParamSubtype.Cylinders:
                                        var newMRC = new MsbRegionCylinder(Regions);
                                        newMRC.Read(bin);
                                        OriginalIndexOrder_Regions.Add(newMRC);
                                        Regions.Cylinders.Add(newMRC);
                                        break;
                                    case PointParamSubtype.Boxes:
                                        var newMRB = new MsbRegionBox(Regions);
                                        newMRB.Read(bin);
                                        OriginalIndexOrder_Regions.Add(newMRB);
                                        Regions.Boxes.Add(newMRB);
                                        break;
                                }
                                break;

                            case MsbSectorFormat.PARTS_PARAM_ST:
                                var partsType = PartsParamSubtype.MapPieces;
                                bin.StepIn(bin.Position + 0x04);
                                {
                                    partsType = (PartsParamSubtype)bin.ReadInt32();
                                }
                                bin.StepOut();

                                switch (partsType)
                                {
                                    case PartsParamSubtype.Hits:
                                        var newMPC = new MsbPartsHit();
                                        newMPC.Read(bin);
                                        OriginalIndexOrder_Parts.Add(newMPC);
                                        Parts.Hits.Add(newMPC);
                                        break;
                                    case PartsParamSubtype.MapPieces:
                                        var newMPMP = new MsbPartsMapPiece();
                                        newMPMP.Read(bin);
                                        OriginalIndexOrder_Parts.Add(newMPMP);
                                        Parts.MapPieces.Add(newMPMP);
                                        break;
                                    case PartsParamSubtype.Navimeshes:
                                        var newMPN = new MsbPartsNavimesh();
                                        newMPN.Read(bin);
                                        OriginalIndexOrder_Parts.Add(newMPN);
                                        Parts.Navimeshes.Add(newMPN);
                                        break;
                                    case PartsParamSubtype.NPCs:
                                        var newMPNPC = new MsbPartsNPC();
                                        newMPNPC.Read(bin);
                                        OriginalIndexOrder_Parts.Add(newMPNPC);
                                        Parts.NPCs.Add(newMPNPC);
                                        break;
                                    case PartsParamSubtype.Objects:
                                        var newMPO = new MsbPartsObject();
                                        newMPO.Read(bin);
                                        OriginalIndexOrder_Parts.Add(newMPO);
                                        Parts.Objects.Add(newMPO);
                                        break;
                                    case PartsParamSubtype.Players:
                                        var newMPP = new MsbPartsPlayer();
                                        newMPP.Read(bin);
                                        OriginalIndexOrder_Parts.Add(newMPP);
                                        Parts.Players.Add(newMPP);
                                        break;
                                    case PartsParamSubtype.ConnectHits:
                                        var newMPUC = new MsbPartsConnectHit();
                                        newMPUC.Read(bin);
                                        OriginalIndexOrder_Parts.Add(newMPUC);
                                        Parts.ConnectHits.Add(newMPUC);
                                        break;
                                    case PartsParamSubtype.DummyNPCs:
                                        var newMPUNPC = new MsbPartsNPCDummy();
                                        newMPUNPC.Read(bin);
                                        OriginalIndexOrder_Parts.Add(newMPUNPC);
                                        Parts.DummyNPCs.Add(newMPUNPC);
                                        break;
                                    case PartsParamSubtype.DummyObjects:
                                        var newMPUO = new MsbPartsObjectDummy();
                                        newMPUO.Read(bin);
                                        OriginalIndexOrder_Parts.Add(newMPUO);
                                        Parts.DummyObjects.Add(newMPUO);
                                        break;
                                }

                                break;
                        }
                    }
                    bin.StepOut();
                }

                if (currentSectorFormat == MsbSectorFormat.PARTS_PARAM_ST)
                {
                    break;
                }



                int sectionEndOffset = bin.ReadInt32();

                bin.Position = sectionEndOffset + 4;

                //if (sectionEndOffset == 0)
                //{
                //    //LAST SECTION YEET
                //    return;
                //}
                //else
                //{
                //    bin.Position = sectionEndOffset + 4;
                //}



            }
            while (true); //Maybe double check here so it doesnt keep reading on dumb files

            bool hasRegionNameConflicts = true;
            do
            {
                hasRegionNameConflicts = false;
                foreach (var region in Regions.GlobalList)
                {
                    var allRegionsWithThisName = Regions.GlobalList
                        .Where(x => x.Name == region.Name)
                        .ToList();

                    if (allRegionsWithThisName.Count > 1)
                    {
                        hasRegionNameConflicts = true;
                        int dupeNum = 0;
                        foreach (var regionWithConflictingName in allRegionsWithThisName)
                        {
                            regionWithConflictingName.Name += $" (Duplicate {(++dupeNum)})";
                        }
                    }
                }
            }
            while (hasRegionNameConflicts);

            string GetNameFromIndex_Model(int index)
            {
                if (index >= 0)
                    return OriginalIndexOrder_Model[index].Name;

                return "";
            }

            string GetNameFromIndex_Part(int index)
            {
                if (index >= 0)
                    return OriginalIndexOrder_Parts[index].Name;

                return "";
            }

            string GetNameFromIndex_Region(int index)
            {
                if (index >= 0)
                {
                    if (index < OriginalIndexOrder_Regions.Count)
                        return OriginalIndexOrder_Regions[index].Name;
                    else
                        return $"[INVALID REGION INDEX: {index}]";
                }
                    

                return "";
            }

            string GetNameFromIndex_Event(int index)
            {
                if (index >= 0)
                    return OriginalIndexOrder_Events[index].Name;

                return "";
            }

            foreach (var part in Parts.GlobalList)
                part.ModelName = GetNameFromIndex_Model(part.i_ModelName);

            foreach (var ev in Events.GlobalList)
                ev.Part = GetNameFromIndex_Part(ev.i_Part);

            foreach (var thing in Parts.NPCs)
                thing.HitName = GetNameFromIndex_Part(thing.i_HitName);

            foreach (var thing in Parts.DummyNPCs)
                thing.HitName = GetNameFromIndex_Part(thing.i_HitName);

            foreach (var thing in Parts.Objects)
                thing.PartName = GetNameFromIndex_Part(thing.i_PartName);

            foreach (var thing in Parts.DummyObjects)
                thing.PartName = GetNameFromIndex_Part(thing.i_PartName);

            foreach (var thing in Events.ObjActs)
                thing.ObjName = GetNameFromIndex_Part(thing.i_ObjName);

            foreach (var thing in Events.GlobalList)
            {
                if (thing.i_Region >= 0)
                {
                    thing.Region = GetNameFromIndex_Region(thing.i_Region);
                }
                
            }

            foreach (var thing in Parts.NPCs)
            {
                thing.MovePoint1 = GetNameFromIndex_Region(thing.SolvedMovePointIndex1);
                thing.MovePoint2 = GetNameFromIndex_Region(thing.SolvedMovePointIndex2);
                thing.MovePoint3 = GetNameFromIndex_Region(thing.SolvedMovePointIndex3);
                thing.MovePoint4 = GetNameFromIndex_Region(thing.SolvedMovePointIndex4);
            }

            foreach (var thing in Parts.DummyNPCs)
            {
                thing.MovePoint1 = GetNameFromIndex_Region(thing.SolvedMovePointIndex1);
                thing.MovePoint2 = GetNameFromIndex_Region(thing.SolvedMovePointIndex2);
                thing.MovePoint3 = GetNameFromIndex_Region(thing.SolvedMovePointIndex3);
                thing.MovePoint4 = GetNameFromIndex_Region(thing.SolvedMovePointIndex4);
            }

            foreach (var thing in Events.Generators)
            {
                thing.SpawnPoint1 = GetNameFromIndex_Region(thing.InternalSpawnPoint1);
                thing.SpawnPoint2 = GetNameFromIndex_Region(thing.InternalSpawnPoint2);
                thing.SpawnPoint3 = GetNameFromIndex_Region(thing.InternalSpawnPoint3);
                thing.SpawnPoint4 = GetNameFromIndex_Region(thing.InternalSpawnPoint4);

                thing.SpawnPart1 = GetNameFromIndex_Part(thing.InternalSpawnPart1);
                thing.SpawnPart2 = GetNameFromIndex_Part(thing.InternalSpawnPart2);
                thing.SpawnPart3 = GetNameFromIndex_Part(thing.InternalSpawnPart3);
                thing.SpawnPart4 = GetNameFromIndex_Part(thing.InternalSpawnPart4);
                thing.SpawnPart5 = GetNameFromIndex_Part(thing.InternalSpawnPart5);
                thing.SpawnPart6 = GetNameFromIndex_Part(thing.InternalSpawnPart6);
                thing.SpawnPart7 = GetNameFromIndex_Part(thing.InternalSpawnPart7);
                thing.SpawnPart8 = GetNameFromIndex_Part(thing.InternalSpawnPart8);
                thing.SpawnPart9 = GetNameFromIndex_Part(thing.InternalSpawnPart9);
                thing.SpawnPart10 = GetNameFromIndex_Part(thing.InternalSpawnPart10);
                thing.SpawnPart11 = GetNameFromIndex_Part(thing.InternalSpawnPart11);
                thing.SpawnPart12 = GetNameFromIndex_Part(thing.InternalSpawnPart12);
                thing.SpawnPart13 = GetNameFromIndex_Part(thing.InternalSpawnPart13);
                thing.SpawnPart14 = GetNameFromIndex_Part(thing.InternalSpawnPart14);
                thing.SpawnPart15 = GetNameFromIndex_Part(thing.InternalSpawnPart15);
                thing.SpawnPart16 = GetNameFromIndex_Part(thing.InternalSpawnPart16);
                thing.SpawnPart17 = GetNameFromIndex_Part(thing.InternalSpawnPart17);
                thing.SpawnPart18 = GetNameFromIndex_Part(thing.InternalSpawnPart18);
                thing.SpawnPart19 = GetNameFromIndex_Part(thing.InternalSpawnPart19);
                thing.SpawnPart20 = GetNameFromIndex_Part(thing.InternalSpawnPart20);
                thing.SpawnPart21 = GetNameFromIndex_Part(thing.InternalSpawnPart21);
                thing.SpawnPart22 = GetNameFromIndex_Part(thing.InternalSpawnPart22);
                thing.SpawnPart23 = GetNameFromIndex_Part(thing.InternalSpawnPart23);
                thing.SpawnPart24 = GetNameFromIndex_Part(thing.InternalSpawnPart24);
                thing.SpawnPart25 = GetNameFromIndex_Part(thing.InternalSpawnPart25);
                thing.SpawnPart26 = GetNameFromIndex_Part(thing.InternalSpawnPart26);
                thing.SpawnPart27 = GetNameFromIndex_Part(thing.InternalSpawnPart27);
                thing.SpawnPart28 = GetNameFromIndex_Part(thing.InternalSpawnPart28);
                thing.SpawnPart29 = GetNameFromIndex_Part(thing.InternalSpawnPart29);
                thing.SpawnPart30 = GetNameFromIndex_Part(thing.InternalSpawnPart30);
                thing.SpawnPart31 = GetNameFromIndex_Part(thing.InternalSpawnPart31);
                thing.SpawnPart32 = GetNameFromIndex_Part(thing.InternalSpawnPart32);
            }

            foreach (var thing in Events.Treasures)
            {
                thing.AttachObj = GetNameFromIndex_Part(thing.i_AttachObj);
            }

            foreach (var thing in Events.Navimeshes)
            {
                thing.NvmRegion = GetNameFromIndex_Part(thing.i_NvmRegion);
            }

            foreach (var thing in Parts.Hits)
            {
                thing.EnvLightMapSpot = Events.EnvLightMapSpotNameOf(thing.i_EnvLightMapSpot);
            }

            foreach (var thing in Events.NpcWorldInvitations)
            {
                thing.SpawnPoint = GetNameFromIndex_Region(thing.i_SpawnPoint);
            }

            foreach (var thing in Events.SpawnPoints)
            {
                thing.SpawnPoint = GetNameFromIndex_Region(thing.i_SpawnPoint);
            }

            //TEMP_DEBUG//

            //foreach (var r in Events.GlobalList)
            //{
            //    r.Name = $"[DBG_IDX: {Events.GlobalList.IndexOf(r)}] {r.Name}";
            //}

            //TEMP_DEBUG//
        }

        protected override void Write(DSBinaryWriter bin, IProgress<(int, int)> prog)
        {
            var LIST_EVENT = Events.GlobalList;
            var LIST_REGION = Regions.GlobalList;
            var LIST_PARTS = Parts.GlobalList;

            var partsIndexCorrectionCurIndex = 0;
            var partsIndexCorrectionLastPartType = PartsParamSubtype.MapPieces;
            foreach (var part in Parts.GlobalList)
            {
                if (part.Type != partsIndexCorrectionLastPartType)
                    partsIndexCorrectionCurIndex = 0;
                part.i_ModelName = Models.IndexOf(part.ModelName);
                part.Index = partsIndexCorrectionCurIndex++;
                partsIndexCorrectionLastPartType = part.Type;
            }

            var modelIndexCorrectionCurIndex = 0;
            var modelIndexCorrectionLastPartType = ModelParamSubtype.MapPiece;
            foreach (var model in Models.GlobalList)
            {
                if (model.ModelType != modelIndexCorrectionLastPartType)
                    modelIndexCorrectionCurIndex = 0;
                model.Index = modelIndexCorrectionCurIndex++;
                modelIndexCorrectionLastPartType = model.ModelType;
            }

            foreach (var ev in Events.GlobalList)
                ev.i_Part = Parts.IndexOf(ev.Part);

            foreach (var thing in Parts.NPCs)
                thing.i_HitName = Parts.IndexOf(thing.HitName);

            foreach (var thing in Parts.DummyNPCs)
                thing.i_HitName = Parts.IndexOf(thing.HitName);

            foreach (var thing in Parts.Objects)
                thing.i_PartName = Parts.IndexOf(thing.PartName);

            foreach (var thing in Parts.DummyObjects)
                thing.i_PartName = Parts.IndexOf(thing.PartName);

            foreach (var thing in Events.ObjActs)
                thing.i_ObjName = Parts.IndexOf(thing.ObjName);

            foreach (var thing in Events.GlobalList)
                thing.i_Region = Regions.IndexOf(thing.Region);
            

            foreach (var thing in Parts.NPCs)
            {
                thing.SolvedMovePointIndex1 = (short)Regions.IndexOf(thing.MovePoint1);
                thing.SolvedMovePointIndex2 = (short)Regions.IndexOf(thing.MovePoint2);
                thing.SolvedMovePointIndex3 = (short)Regions.IndexOf(thing.MovePoint3);
                thing.SolvedMovePointIndex4 = (short)Regions.IndexOf(thing.MovePoint4);
            }

            foreach (var thing in Parts.DummyNPCs)
            {
                thing.SolvedMovePointIndex1 = (short)Regions.IndexOf(thing.MovePoint1);
                thing.SolvedMovePointIndex2 = (short)Regions.IndexOf(thing.MovePoint2);
                thing.SolvedMovePointIndex3 = (short)Regions.IndexOf(thing.MovePoint3);
                thing.SolvedMovePointIndex4 = (short)Regions.IndexOf(thing.MovePoint4);
            }

            foreach (var thing in Events.Generators)
            {
                thing.InternalSpawnPoint1 = Regions.IndexOf(thing.SpawnPoint1);
                thing.InternalSpawnPoint2 = Regions.IndexOf(thing.SpawnPoint2);
                thing.InternalSpawnPoint3 = Regions.IndexOf(thing.SpawnPoint3);
                thing.InternalSpawnPoint4 = Regions.IndexOf(thing.SpawnPoint4);

                thing.InternalSpawnPart1 =   Parts.IndexOf(thing.SpawnPart1 );
                thing.InternalSpawnPart2 =   Parts.IndexOf(thing.SpawnPart2 );
                thing.InternalSpawnPart3 =   Parts.IndexOf(thing.SpawnPart3 );
                thing.InternalSpawnPart4 =   Parts.IndexOf(thing.SpawnPart4 );
                thing.InternalSpawnPart5 =   Parts.IndexOf(thing.SpawnPart5 );
                thing.InternalSpawnPart6 =   Parts.IndexOf(thing.SpawnPart6 );
                thing.InternalSpawnPart7 =   Parts.IndexOf(thing.SpawnPart7 );
                thing.InternalSpawnPart8 =   Parts.IndexOf(thing.SpawnPart8 );
                thing.InternalSpawnPart9 =   Parts.IndexOf(thing.SpawnPart9 );
                thing.InternalSpawnPart10 =  Parts.IndexOf(thing.SpawnPart10);
                thing.InternalSpawnPart11 =  Parts.IndexOf(thing.SpawnPart11);
                thing.InternalSpawnPart12 =  Parts.IndexOf(thing.SpawnPart12);
                thing.InternalSpawnPart13 =  Parts.IndexOf(thing.SpawnPart13);
                thing.InternalSpawnPart14 =  Parts.IndexOf(thing.SpawnPart14);
                thing.InternalSpawnPart15 =  Parts.IndexOf(thing.SpawnPart15);
                thing.InternalSpawnPart16 =  Parts.IndexOf(thing.SpawnPart16);
                thing.InternalSpawnPart17 =  Parts.IndexOf(thing.SpawnPart17);
                thing.InternalSpawnPart18 =  Parts.IndexOf(thing.SpawnPart18);
                thing.InternalSpawnPart19 =  Parts.IndexOf(thing.SpawnPart19);
                thing.InternalSpawnPart20 =  Parts.IndexOf(thing.SpawnPart20);
                thing.InternalSpawnPart21  = Parts.IndexOf(thing.SpawnPart21);
                thing.InternalSpawnPart22 =  Parts.IndexOf(thing.SpawnPart22);
                thing.InternalSpawnPart23 =  Parts.IndexOf(thing.SpawnPart23);
                thing.InternalSpawnPart24 =  Parts.IndexOf(thing.SpawnPart24);
                thing.InternalSpawnPart25 =  Parts.IndexOf(thing.SpawnPart25);
                thing.InternalSpawnPart26 =  Parts.IndexOf(thing.SpawnPart26);
                thing.InternalSpawnPart27 =  Parts.IndexOf(thing.SpawnPart27);
                thing.InternalSpawnPart28 =  Parts.IndexOf(thing.SpawnPart28);
                thing.InternalSpawnPart29 =  Parts.IndexOf(thing.SpawnPart29);
                thing.InternalSpawnPart30 =  Parts.IndexOf(thing.SpawnPart30);
                thing.InternalSpawnPart31 =  Parts.IndexOf(thing.SpawnPart31);
                thing.InternalSpawnPart32 =  Parts.IndexOf(thing.SpawnPart32);
            }

            foreach (var thing in Events.Treasures)
            {
                thing.i_AttachObj = Parts.IndexOf(thing.AttachObj);
            }

            foreach (var thing in Events.Navimeshes)
            {
                thing.i_NvmRegion = Parts.IndexOf(thing.NvmRegion);
            }

            foreach (var thing in Parts.Hits)
            {
                thing.i_EnvLightMapSpot = (short)Events.EnvLightMapSpotIndexOf(thing.EnvLightMapSpot);
            }

            foreach (var thing in Events.NpcWorldInvitations)
            {
                thing.i_SpawnPoint = Regions.IndexOf(thing.SpawnPoint);
            }

            foreach (var thing in Events.SpawnPoints)
            {
                thing.i_SpawnPoint = Regions.IndexOf(thing.SpawnPoint);
            }

            bin.Write(Unknown1);


            bin.Placeholder("MODEL_PARAM_ST");
            bin.Write(Models.Count + 1);
            for (int i = 0; i < Models.Count; i++)
            {
                bin.Placeholder($"MODEL_PARAM_ST_{i}");
            }
            bin.Placeholder("MODEL_PARAM_ST_END");
            //bin.Write((int)0);

            bin.Replace("MODEL_PARAM_ST", (int)bin.Position);
            bin.WriteStringAscii("MODEL_PARAM_ST", terminate: true);
            bin.Pad(align: 0x4);
            for (int i = 0; i < Models.Count; i++)
            {
                bin.Replace($"MODEL_PARAM_ST_{i}", (int)bin.Position);
                Models[i].Write(bin);
            }
            bin.Replace("MODEL_PARAM_ST_END", (int)bin.Position);
            bin.Write((int)0);


            


            bin.Placeholder("EVENT_PARAM_ST");
            bin.Write(LIST_EVENT.Count + 1);
            for (int i = 0; i < LIST_EVENT.Count; i++)
            {
                bin.Placeholder($"EVENT_PARAM_ST_{i}");
            }
            bin.Placeholder("EVENT_PARAM_ST_END");
            //bin.Write((int)0);

            bin.Replace("EVENT_PARAM_ST", (int)bin.Position);
            bin.WriteStringAscii("EVENT_PARAM_ST", terminate: true);
            bin.Pad(align: 0x4);
            for (int i = 0; i < LIST_EVENT.Count; i++)
            {
                bin.Replace($"EVENT_PARAM_ST_{i}", (int)bin.Position);
                LIST_EVENT[i].Write(bin);
            }
            bin.Replace("EVENT_PARAM_ST_END", (int)bin.Position);
            bin.Write((int)0);






            bin.Placeholder("POINT_PARAM_ST");
            bin.Write(LIST_REGION.Count + 1);
            for (int i = 0; i < LIST_REGION.Count; i++)
            {
                bin.Placeholder($"POINT_PARAM_ST_{i}");
            }
            bin.Placeholder("POINT_PARAM_ST_END");
            //bin.Write((int)0);

            bin.Replace("POINT_PARAM_ST", (int)bin.Position);
            bin.WriteStringAscii("POINT_PARAM_ST", terminate: true);
            bin.Pad(align: 0x4);
            for (int i = 0; i < LIST_REGION.Count; i++)
            {
                bin.Replace($"POINT_PARAM_ST_{i}", (int)bin.Position);
                LIST_REGION[i].Write(bin);
            }
            bin.Replace("POINT_PARAM_ST_END", (int)bin.Position);
            bin.Write((int)0);



            bin.Placeholder("PARTS_PARAM_ST");
            bin.Write(LIST_PARTS.Count + 1);
            for (int i = 0; i < LIST_PARTS.Count; i++)
            {
                bin.Placeholder($"PARTS_PARAM_ST_{i}");
            }
            //NO END MARKER BECAUSE LAST SECTION
            bin.Write((int)0);

            bin.Replace("PARTS_PARAM_ST", (int)bin.Position);
            bin.WriteStringAscii("PARTS_PARAM_ST", terminate: true);
            bin.Pad(align: 0x4);
            for (int i = 0; i < LIST_PARTS.Count; i++)
            {
                bin.Replace($"PARTS_PARAM_ST_{i}", (int)bin.Position);
                LIST_PARTS[i].Write(bin);
            }
            bin.Write((int)0);
        }
    }
}
