using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB.PARTS_PARAM_ST
{
    public class MsbPartsNavimesh : MsbPartsBase
    {
        internal override void DebugPushUnknownFieldReport_Subtype(out string subtypeName, Dictionary<string, object> dict)
        {
            subtypeName = "Navimesh";

            dict.Add(nameof(SUB_CONST_1), SUB_CONST_1);
            dict.Add(nameof(SUB_CONST_2), SUB_CONST_2);
            dict.Add(nameof(SUB_CONST_3), SUB_CONST_3);
            dict.Add(nameof(SUB_CONST_4), SUB_CONST_4);
        }

        public int NavimeshGroup1 { get; set; } = -1;
        public int NavimeshGroup2 { get; set; } = -1;
        public int NavimeshGroup3 { get; set; } = -1;
        public int NavimeshGroup4 { get; set; } = -1;

        internal int SUB_CONST_1 { get; set; } = 0;
        internal int SUB_CONST_2 { get; set; } = 0;
        internal int SUB_CONST_3 { get; set; } = 0;
        internal int SUB_CONST_4 { get; set; } = 0;

        internal override PartsParamSubtype GetSubtypeValue()
        {
            return PartsParamSubtype.Navimeshes;
        }

        protected override void SubtypeRead(DSBinaryReader bin)
        {
            NavimeshGroup1 = bin.ReadInt32();
            NavimeshGroup2 = bin.ReadInt32();
            NavimeshGroup3 = bin.ReadInt32();
            NavimeshGroup4 = bin.ReadInt32();

            SUB_CONST_1 = bin.ReadInt32();
            SUB_CONST_2 = bin.ReadInt32();
            SUB_CONST_3 = bin.ReadInt32();
            SUB_CONST_4 = bin.ReadInt32();
        }

        protected override void SubtypeWrite(DSBinaryWriter bin)
        {
            bin.Write(NavimeshGroup1);
            bin.Write(NavimeshGroup2);
            bin.Write(NavimeshGroup3);
            bin.Write(NavimeshGroup4);

            bin.Write(SUB_CONST_1);
            bin.Write(SUB_CONST_2);
            bin.Write(SUB_CONST_3);
            bin.Write(SUB_CONST_4);
        }
    }
}
