﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB.EVENT_PARAM_ST
{
    public class MsbEventBloodMsg : MsbEventBase
    {
        internal override void DebugPushUnknownFieldReport_Subtype(out string subtypeName, Dictionary<string, object> dict)
        {
            subtypeName = "Messages";

            dict.Add(nameof(SUB_CONST_1), SUB_CONST_1);
            dict.Add(nameof(SeekGuidanceOnly), SeekGuidanceOnly);
            dict.Add(nameof(SUB_CONST_2), SUB_CONST_2);
            dict.Add(nameof(SUB_CONST_3), SUB_CONST_3);
        }

        public short MsgID { get; set; } = 0;

        internal short SUB_CONST_1 { get; set; } = 2;

        public bool SeekGuidanceOnly { get; set; } = false;
        internal byte SUB_CONST_2 { get; set; } = 0;

        internal short SUB_CONST_3 { get; set; } = 0;

        //DeS only
        public int MsgParam { get; set; } = 0;

        protected override EventParamSubtype GetSubtypeValue()
        {
            return EventParamSubtype.BloodMsg;
        }

        protected override void SubtypeRead(DSBinaryReader bin)
        {
            if (bin.IsDeS)
            {
                SubtypeReadDeS(bin);
            }
            else
            {
                MsgID = bin.ReadInt16();
                SUB_CONST_1 = bin.ReadInt16();
                SeekGuidanceOnly = bin.ReadBoolean();
                SUB_CONST_2 = bin.ReadByte();
                SUB_CONST_3 = bin.ReadInt16();
            }
        }

        private void SubtypeReadDeS(DSBinaryReader bin)
        {
            SUB_CONST_1 = bin.ReadInt16();
            MsgID = bin.ReadInt16();
            MsgParam = bin.ReadInt32();
        }

        protected override void SubtypeWrite(DSBinaryWriter bin)
        {
            if (bin.IsDeS)
            {
                SubtypeWriteDeS(bin);
            }
            else
            {
                bin.Write(MsgID);
                bin.Write(SUB_CONST_1);
                bin.Write(SeekGuidanceOnly);
                bin.Write(SUB_CONST_2);
                bin.Write(SUB_CONST_3);
            }   
        }

        private void SubtypeWriteDeS(DSBinaryWriter bin)
        {
            bin.Write(SUB_CONST_1);
            bin.Write(MsgID);
            bin.Write(MsgParam);
        }
    }
}
