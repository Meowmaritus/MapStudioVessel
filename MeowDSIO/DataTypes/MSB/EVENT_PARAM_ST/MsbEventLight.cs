using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB.EVENT_PARAM_ST
{
    public class MsbEventLight : MsbEventBase
    {
        internal override void DebugPushUnknownFieldReport_Subtype(out string subtypeName, Dictionary<string, object> dict)
        {
            subtypeName = "Light";

            dict.Add(nameof(PointLightID), PointLightID);
        }

        public int PointLightID { get; set; } = 0;

        protected override EventParamSubtype GetSubtypeValue()
        {
            return EventParamSubtype.Lights;
        }

        protected override void SubtypeRead(DSBinaryReader bin)
        {
            PointLightID = bin.ReadInt32();
        }

        protected override void SubtypeWrite(DSBinaryWriter bin)
        {
            bin.Write(PointLightID);
            if (bin.LongOffsets)
            {
                bin.Jump(4);
            }
        }
    }
}
