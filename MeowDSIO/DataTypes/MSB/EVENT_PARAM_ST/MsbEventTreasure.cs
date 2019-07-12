using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB.EVENT_PARAM_ST
{
    public class MsbEventTreasure : MsbEventBase
    {
        internal override void DebugPushUnknownFieldReport_Subtype(out string subtypeName, Dictionary<string, object> dict)
        {
            subtypeName = "Treasure";

            dict.Add(nameof(SUB_CONST_1), SUB_CONST_1);
            dict.Add(nameof(SUB_CONST_2), SUB_CONST_2);
            dict.Add(nameof(SUB_CONST_3), SUB_CONST_3);
            dict.Add(nameof(SUB_CONST_4), SUB_CONST_4);
            dict.Add(nameof(SUB_CONST_5), SUB_CONST_5);
            dict.Add(nameof(SUB_CONST_6), SUB_CONST_6);
            dict.Add(nameof(SUB_CONST_7), SUB_CONST_7);
            dict.Add(nameof(SUB_CONST_8), SUB_CONST_8);
        }

        //public int SUx00 { get; set; } = 0;
        internal int SUB_CONST_1 { get; set; } = 0;
        internal int i_AttachObj { get; set; } = 0;

        public string AttachObj { get; set; } = MiscUtil.BAD_REF;

        public int ItemLot1 { get; set; } = 0;

        internal int SUB_CONST_2 { get; set; } = -1;

        public int ItemLot2 { get; set; } = 0;

        internal int SUB_CONST_3 { get; set; } = -1;

        public int ItemLot3 { get; set; } = 0;

        internal int SUB_CONST_4 { get; set; } = -1;

        public int ItemLot4 { get; set; } = 0;

        internal int SUB_CONST_5 { get; set; } = -1;

        public int ItemLot5 { get; set; } = 0;

        internal int SUB_CONST_6 { get; set; } = -1;

        public bool InChest { get; set; } = false;
        public bool StartDisabled { get; set; } = false;
        internal byte SUB_CONST_7 { get; set; } = 0;
        internal byte SUB_CONST_8 { get; set; } = 0;

        protected override EventParamSubtype GetSubtypeValue()
        {
            return EventParamSubtype.Treasures;
        }

        protected override void SubtypeRead(DSBinaryReader bin)
        {
            //SUx00 = bin.ReadInt32();
            SUB_CONST_1 = bin.ReadInt32();
            if (bin.LongOffsets)
            {
                bin.Jump(4);
            }
            i_AttachObj = bin.ReadInt32();
            if (bin.LongOffsets)
            {
                bin.Jump(4);
            }
            ItemLot1 = bin.ReadInt32();
            SUB_CONST_2 = bin.ReadInt32();
            ItemLot2 = bin.ReadInt32();
            SUB_CONST_3 = bin.ReadInt32();
            ItemLot3 = bin.ReadInt32();
            SUB_CONST_4 = bin.ReadInt32();
            ItemLot4 = bin.ReadInt32();
            SUB_CONST_5 = bin.ReadInt32();
            ItemLot5 = bin.ReadInt32();
            SUB_CONST_6 = bin.ReadInt32();
            InChest = bin.ReadBoolean();
            StartDisabled = bin.ReadBoolean();
            SUB_CONST_7 = bin.ReadByte();
            SUB_CONST_8 = bin.ReadByte();
        }

        protected override void SubtypeWrite(DSBinaryWriter bin)
        {
            //bin.Write(SUx00);
            bin.Write(SUB_CONST_1);

            if (bin.LongOffsets)
            {
                bin.Jump(4);
            }

            bin.Write(i_AttachObj);

            if (bin.LongOffsets)
            {
                bin.Jump(4);
            }

            bin.Write(ItemLot1);
            bin.Write(SUB_CONST_2);
            bin.Write(ItemLot2);
            bin.Write(SUB_CONST_3);
            bin.Write(ItemLot3);
            bin.Write(SUB_CONST_4);
            bin.Write(ItemLot4);
            bin.Write(SUB_CONST_5);
            bin.Write(ItemLot5);
            bin.Write(SUB_CONST_6);
            bin.Write(InChest);
            bin.Write(StartDisabled);
            bin.Write(SUB_CONST_7);
            bin.Write(SUB_CONST_8);

            if (bin.LongOffsets)
            {
                bin.Jump(4);
            }
        }
    }
}
