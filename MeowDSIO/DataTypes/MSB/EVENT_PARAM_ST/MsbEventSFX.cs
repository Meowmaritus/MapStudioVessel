using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB.EVENT_PARAM_ST
{
    public class MsbEventSFX : MsbEventBase
    {
        internal override void DebugPushUnknownFieldReport_Subtype(out string subtypeName, Dictionary<string, object> dict)
        {
            subtypeName = "SFX";
        }

        public int SfxUnkIndex { get; set; } = 0;
        public int SfxID { get; set; } = 0;

        protected override EventParamSubtype GetSubtypeValue()
        {
            return EventParamSubtype.SFX;
        }

        protected override void SubtypeRead(DSBinaryReader bin)
        {
            if (bin.IsDeS)
            {
                SubtypeReadDeS(bin);
            }
            else
            {
                SfxID = bin.ReadInt32();
            }
        }

        private void SubtypeReadDeS(DSBinaryReader bin)
        {
            SfxUnkIndex = bin.ReadInt32();
            SfxID = bin.ReadInt32();
        }

        protected override void SubtypeWrite(DSBinaryWriter bin)
        {
            if (bin.IsDeS)
                bin.Write(SfxUnkIndex);

            bin.Write(SfxID);

            if (bin.LongOffsets)
            {
                bin.Jump(4);
            }
        }
    }
}
