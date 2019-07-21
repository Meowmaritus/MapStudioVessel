using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB.PARTS_PARAM_ST
{
    public class MsbPartsPlayer : MsbPartsBase
    {
        internal override void DebugPushUnknownFieldReport_Subtype(out string subtypeName, Dictionary<string, object> dict)
        {
            subtypeName = "Player";

            dict.Add(nameof(SUB_CONST_1), SUB_CONST_1);
            dict.Add(nameof(SUB_CONST_2), SUB_CONST_2);
            dict.Add(nameof(SUB_CONST_3), SUB_CONST_3);
            dict.Add(nameof(SUB_CONST_4), SUB_CONST_4);
        }

        public byte PlayerSubUnk { get; set; } = 1;

        internal int SUB_CONST_1 { get; set; } = 0;
        internal int SUB_CONST_2 { get; set; } = 0;
        internal int SUB_CONST_3 { get; set; } = 0;
        internal int SUB_CONST_4 { get; set; } = 0;

        internal override PartsParamSubtype GetSubtypeValue()
        {
            return PartsParamSubtype.Players;
        }

        protected override void SubtypeRead(DSBinaryReader bin)
        {
            if (bin.IsDeS)
            {
                PlayerSubUnk = bin.ReadByte();
                bin.Jump(3);
            } 
            else
            {
                SUB_CONST_1 = bin.ReadInt32();
            }
            
            SUB_CONST_2 = bin.ReadInt32();
            SUB_CONST_3 = bin.ReadInt32();
            SUB_CONST_4 = bin.ReadInt32();
        }

        protected override void SubtypeWrite(DSBinaryWriter bin)
        {
            if (bin.IsDeS)
            {
                bin.Write(PlayerSubUnk);
                bin.Jump(3);
            }
            else
            {
                bin.Write(SUB_CONST_1);
            }
            bin.Write(SUB_CONST_2);
            bin.Write(SUB_CONST_3);
            bin.Write(SUB_CONST_4);
        }
    }
}
