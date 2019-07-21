using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB.PARTS_PARAM_ST
{
    public class MsbPartsHit : MsbPartsBase
    {
        internal override void DebugPushUnknownFieldReport_Subtype(out string subtypeName, Dictionary<string, object> dict)
        {
            subtypeName = "Hit";

            dict.Add(nameof(SUB_CONST_1), SUB_CONST_1);
            dict.Add(nameof(SUB_CONST_2), SUB_CONST_2);
            dict.Add(nameof(SUB_CONST_3), SUB_CONST_3);
            dict.Add(nameof(SUB_CONST_4), SUB_CONST_4);
            dict.Add(nameof(SUB_CONST_5), SUB_CONST_5);
            dict.Add(nameof(SUB_CONST_6), SUB_CONST_6);
            dict.Add(nameof(SUB_CONST_7), SUB_CONST_7);
        }

        public byte HitFilterID { get; set; } = 0;
        public PartsCollisionSoundSpaceType SoundSpaceType { get; set; } = 0;

        internal short i_EnvLightMapSpot { get; set; } = 0;
        public string EnvLightMapSpot { get; set; } = MiscUtil.BAD_REF;

        public float ReflectPlaneHeight { get; set; } = 0;

        public int NavimeshGroup1 { get; set; } = -1;
        public int NavimeshGroup2 { get; set; } = -1;
        public int NavimeshGroup3 { get; set; } = -1;
        public int NavimeshGroup4 { get; set; } = -1;

        public int VagrantID1 { get; set; } = 0;
        public int VagrantID2 { get; set; } = 0;
        public int VagrantID3 { get; set; } = 0;

        public short MapNameID { get; set; } = 0;
        public short DisableStart { get; set; } = 0;
        public int DisableBonfireID { get; set; } = 0;

        internal int SUB_CONST_1 { get; set; } = -1;
        internal int SUB_CONST_2 { get; set; } = -1;
        internal int SUB_CONST_3 { get; set; } = -1;

        public int PlayRegionID { get; set; } = 0;
        public short LockCamID1 { get; set; } = 0;
        public short LockCamID2 { get; set; } = 0;
        
        internal int SUB_CONST_4 { get; set; } = 0;
        internal int SUB_CONST_5 { get; set; } = 0;
        internal int SUB_CONST_6 { get; set; } = 0;
        internal int SUB_CONST_7 { get; set; } = 0;

        //DeS only

        public short RefTexID1 { get; set; } = -1;
        public short RefTexID2 { get; set; } = -1;
        public short RefTexID3 { get; set; } = -1;
        public short RefTexID4 { get; set; } = -1;
        public short RefTexID5 { get; set; } = -1;
        public short RefTexID6 { get; set; } = -1;
        public short RefTexID7 { get; set; } = -1;
        public short RefTexID8 { get; set; } = -1;
        public short RefTexID9 { get; set; } = -1;
        public short RefTexID10 { get; set; } = -1;
        public short RefTexID11 { get; set; } = -1;
        public short RefTexID12 { get; set; } = -1;
        public short RefTexID13 { get; set; } = -1;
        public short RefTexID14 { get; set; } = -1;
        public short RefTexID15 { get; set; } = -1;
        public short RefTexID16 { get; set; } = -1;

        internal override PartsParamSubtype GetSubtypeValue()
        {
            return PartsParamSubtype.Hits;
        }

        protected override void SubtypeRead(DSBinaryReader bin)
        {
            if (bin.IsDeS)
            {
                SubtypeReadDeS(bin);
            }
            else
            {
                HitFilterID = bin.ReadByte();
                SoundSpaceType = (PartsCollisionSoundSpaceType)bin.ReadByte();
                i_EnvLightMapSpot = bin.ReadInt16();
                ReflectPlaneHeight = bin.ReadSingle();

                NavimeshGroup1 = bin.ReadInt32();
                NavimeshGroup2 = bin.ReadInt32();
                NavimeshGroup3 = bin.ReadInt32();
                NavimeshGroup4 = bin.ReadInt32();

                VagrantID1 = bin.ReadInt32();
                VagrantID2 = bin.ReadInt32();
                VagrantID3 = bin.ReadInt32();

                MapNameID = bin.ReadInt16();
                DisableStart = bin.ReadInt16();
                DisableBonfireID = bin.ReadInt32();

                SUB_CONST_1 = bin.ReadInt32();
                SUB_CONST_2 = bin.ReadInt32();
                SUB_CONST_3 = bin.ReadInt32();

                PlayRegionID = bin.ReadInt32();
                LockCamID1 = bin.ReadInt16();
                LockCamID2 = bin.ReadInt16();

                SUB_CONST_4 = bin.ReadInt32();
                SUB_CONST_5 = bin.ReadInt32();
                SUB_CONST_6 = bin.ReadInt32();
                SUB_CONST_7 = bin.ReadInt32();
            }
        }

        private void SubtypeReadDeS(DSBinaryReader bin)
        {
            HitFilterID = bin.ReadByte();
            SoundSpaceType = (PartsCollisionSoundSpaceType)bin.ReadByte();
            i_EnvLightMapSpot = bin.ReadInt16();
            ReflectPlaneHeight = bin.ReadSingle();

            NavimeshGroup1 = bin.ReadInt32();
            NavimeshGroup2 = bin.ReadInt32();
            NavimeshGroup3 = bin.ReadInt32();
            NavimeshGroup4 = bin.ReadInt32();

            RefTexID1 = bin.ReadInt16();
            RefTexID2 = bin.ReadInt16();
            RefTexID3 = bin.ReadInt16();
            RefTexID4 = bin.ReadInt16();
            RefTexID5 = bin.ReadInt16();
            RefTexID6 = bin.ReadInt16();
            RefTexID7 = bin.ReadInt16();
            RefTexID8 = bin.ReadInt16();
            RefTexID9 = bin.ReadInt16();
            RefTexID10 = bin.ReadInt16();
            RefTexID11 = bin.ReadInt16();
            RefTexID12 = bin.ReadInt16();
            RefTexID13 = bin.ReadInt16();
            RefTexID14 = bin.ReadInt16();
            RefTexID15 = bin.ReadInt16();
            RefTexID16 = bin.ReadInt16();

            SUB_CONST_1 = bin.ReadInt16();
            MapNameID = bin.ReadInt16();

            SUB_CONST_4 = bin.ReadInt32();
            SUB_CONST_5 = bin.ReadInt32();
            SUB_CONST_6 = bin.ReadInt32();
            SUB_CONST_7 = bin.ReadInt32();
        }

        protected override void SubtypeWrite(DSBinaryWriter bin)
        {
            if (bin.IsDeS)
            {
                SubtypeWriteDeS(bin);
                return;
            }

            bin.Write(HitFilterID);
            bin.Write((byte)SoundSpaceType);
            bin.Write(i_EnvLightMapSpot);
            bin.Write(ReflectPlaneHeight);

            bin.Write(NavimeshGroup1);
            bin.Write(NavimeshGroup2);
            bin.Write(NavimeshGroup3);
            bin.Write(NavimeshGroup4);

            bin.Write(VagrantID1);
            bin.Write(VagrantID2);
            bin.Write(VagrantID3);

            bin.Write(MapNameID);
            bin.Write(DisableStart);
            bin.Write(DisableBonfireID);

            bin.Write(SUB_CONST_1);
            bin.Write(SUB_CONST_2);
            bin.Write(SUB_CONST_3);

            bin.Write(PlayRegionID);
            bin.Write(LockCamID1);
            bin.Write(LockCamID2);

            bin.Write(SUB_CONST_4);
            bin.Write(SUB_CONST_5);
            bin.Write(SUB_CONST_6);
            bin.Write(SUB_CONST_7);
        }

        private void SubtypeWriteDeS(DSBinaryWriter bin)
        {
            bin.Write(HitFilterID);
            bin.Write((byte)SoundSpaceType);
            bin.Write(i_EnvLightMapSpot);
            bin.Write(ReflectPlaneHeight);

            bin.Write(NavimeshGroup1);
            bin.Write(NavimeshGroup2);
            bin.Write(NavimeshGroup3);
            bin.Write(NavimeshGroup4);

            bin.Write(RefTexID1);
            bin.Write(RefTexID2);
            bin.Write(RefTexID3);
            bin.Write(RefTexID4);
            bin.Write(RefTexID5);
            bin.Write(RefTexID6);
            bin.Write(RefTexID7);
            bin.Write(RefTexID8);
            bin.Write(RefTexID9);
            bin.Write(RefTexID10);
            bin.Write(RefTexID11);
            bin.Write(RefTexID12);
            bin.Write(RefTexID13);
            bin.Write(RefTexID14);
            bin.Write(RefTexID15);
            bin.Write(RefTexID16);

            bin.Write((short)SUB_CONST_1);
            bin.Write(MapNameID);

            bin.Write(SUB_CONST_4);
            bin.Write(SUB_CONST_5);
            bin.Write(SUB_CONST_6);
            bin.Write(SUB_CONST_7);
        }
    }
}
