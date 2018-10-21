using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB.PARTS_PARAM_ST
{
    public class MsbPartsHit : MsbPartsBase
    {
        public byte HitFilterID { get; set; } = 0;
        public PartsCollisionSoundSpaceType SoundSpaceType { get; set; } = 0;

        public short i_EnvLightMapSpot { get; set; } = 0;
        public string EnvLightMapSpot { get; set; } = MiscUtil.BAD_REF;

        public float ReflectPlaneHeight { get; set; } = 0;

        public int NvmGroup1 { get; set; } = 0;
        public int NvmGroup2 { get; set; } = 0;
        public int NvmGroup3 { get; set; } = 0;
        public int NvmGroup4 { get; set; } = 0;

        public int VagrantID1 { get; set; } = 0;
        public int VagrantID2 { get; set; } = 0;
        public int VagrantID3 { get; set; } = 0;

        public short MapNameID { get; set; } = 0;
        public short DisableStart { get; set; } = 0;
        public int DisableBonfireID { get; set; } = 0;

        public int UNK1 { get; set; } = 0;
        public int UNK2 { get; set; } = 0;
        public int UNK3 { get; set; } = 0;

        public int PlayRegionID { get; set; } = 0;
        public short LockCamID1 { get; set; } = 0;
        public short LockCamID2 { get; set; } = 0;
        
        public int UNK4 { get; set; } = 0;
        public int UNK5 { get; set; } = 0;
        public int UNK6 { get; set; } = 0;
        public int UNK7 { get; set; } = 0;

        internal override PartsParamSubtype GetSubtypeValue()
        {
            return PartsParamSubtype.Hits;
        }

        protected override void SubtypeRead(DSBinaryReader bin)
        {
            HitFilterID = bin.ReadByte();
            SoundSpaceType = (PartsCollisionSoundSpaceType)bin.ReadByte();
            i_EnvLightMapSpot = bin.ReadInt16();
            ReflectPlaneHeight = bin.ReadSingle();

            NvmGroup1 = bin.ReadInt32();
            NvmGroup2 = bin.ReadInt32();
            NvmGroup3 = bin.ReadInt32();
            NvmGroup4 = bin.ReadInt32();

            VagrantID1 = bin.ReadInt32();
            VagrantID2 = bin.ReadInt32();
            VagrantID3 = bin.ReadInt32();

            MapNameID = bin.ReadInt16();
            DisableStart = bin.ReadInt16();
            DisableBonfireID = bin.ReadInt32();

            UNK1 = bin.ReadInt32();
            UNK2 = bin.ReadInt32();
            UNK3 = bin.ReadInt32();

            PlayRegionID = bin.ReadInt32();
            LockCamID1 = bin.ReadInt16();
            LockCamID2 = bin.ReadInt16();
            
            UNK4 = bin.ReadInt32();
            UNK5 = bin.ReadInt32();
            UNK6 = bin.ReadInt32();
            UNK7 = bin.ReadInt32();
        }


        protected override void SubtypeWrite(DSBinaryWriter bin)
        {
            bin.Write(HitFilterID);
            bin.Write((byte)SoundSpaceType);
            bin.Write(i_EnvLightMapSpot);
            bin.Write(ReflectPlaneHeight);

            bin.Write(NvmGroup1);
            bin.Write(NvmGroup2);
            bin.Write(NvmGroup3);
            bin.Write(NvmGroup4);

            bin.Write(VagrantID1);
            bin.Write(VagrantID2);
            bin.Write(VagrantID3);

            bin.Write(MapNameID);
            bin.Write(DisableStart);
            bin.Write(DisableBonfireID);

            bin.Write(UNK1);
            bin.Write(UNK2);
            bin.Write(UNK3);

            bin.Write(PlayRegionID);
            bin.Write(LockCamID1);
            bin.Write(LockCamID2);

            bin.Write(UNK4);
            bin.Write(UNK5);
            bin.Write(UNK6);
            bin.Write(UNK7);
        }
    }
}
