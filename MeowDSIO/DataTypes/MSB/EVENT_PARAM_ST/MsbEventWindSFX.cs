using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB.EVENT_PARAM_ST
{
    public class MsbEventWindSFX : MsbEventBase
    {
        internal override void DebugPushUnknownFieldReport_Subtype(out string subtypeName, Dictionary<string, object> dict)
        {
            subtypeName = "WindSFX";

            dict.Add(nameof(WindVecMinX), WindVecMinX);
            dict.Add(nameof(WindVecMinY), WindVecMinY);
            dict.Add(nameof(WindVecMinZ), WindVecMinZ);
            dict.Add(nameof(SubUnk4), SubUnk4);
            dict.Add(nameof(WindVecMaxX), WindVecMaxX);
            dict.Add(nameof(WindVecMaxY), WindVecMaxY);
            dict.Add(nameof(WindVecMaxZ), WindVecMaxZ);
            dict.Add(nameof(SubUnk8), SubUnk8);
            dict.Add(nameof(WindSwingCycle0), WindSwingCycle0);
            dict.Add(nameof(WindSwingCycle1), WindSwingCycle1);
            dict.Add(nameof(WindSwingCycle2), WindSwingCycle2);
            dict.Add(nameof(WindSwingCycle3), WindSwingCycle3);
            dict.Add(nameof(WindSwingPow0), WindSwingPow0);
            dict.Add(nameof(WindSwingPow1), WindSwingPow1);
            dict.Add(nameof(WindSwingPow2), WindSwingPow2);
            dict.Add(nameof(WindSwingPow3), WindSwingPow3);
        }

        public float WindVecMinX { get; set; } = 0;
        public float WindVecMinY { get; set; } = 0;
        public float WindVecMinZ { get; set; } = 0;
        public float SubUnk4 { get; set; } = 0;
        public float WindVecMaxX { get; set; } = 0;
        public float WindVecMaxY { get; set; } = 0;
        public float WindVecMaxZ { get; set; } = 0;
        public float SubUnk8 { get; set; } = 0;
        public float WindSwingCycle0 { get; set; } = 0;
        public float WindSwingCycle1 { get; set; } = 0;
        public float WindSwingCycle2 { get; set; } = 0;
        public float WindSwingCycle3 { get; set; } = 0;
        public float WindSwingPow0 { get; set; } = 0;
        public float WindSwingPow1 { get; set; } = 0;
        public float WindSwingPow2 { get; set; } = 0;
        public float WindSwingPow3 { get; set; } = 0;

        protected override EventParamSubtype GetSubtypeValue()
        {
            return EventParamSubtype.WindSFX;
        }

        protected override void SubtypeRead(DSBinaryReader bin)
        {
            WindVecMinX = bin.ReadSingle();
            WindVecMinY = bin.ReadSingle();
            WindVecMinZ = bin.ReadSingle();
            SubUnk4 = bin.ReadSingle();
            WindVecMaxX = bin.ReadSingle();
            WindVecMaxY = bin.ReadSingle();
            WindVecMaxZ = bin.ReadSingle();
            SubUnk8 = bin.ReadSingle();
            WindSwingCycle0 = bin.ReadSingle();
            WindSwingCycle1 = bin.ReadSingle();
            WindSwingCycle2 = bin.ReadSingle();
            WindSwingCycle3 = bin.ReadSingle();
            WindSwingPow0 = bin.ReadSingle();
            WindSwingPow1 = bin.ReadSingle();
            WindSwingPow2 = bin.ReadSingle();
            WindSwingPow3 = bin.ReadSingle();
        }

        protected override void SubtypeWrite(DSBinaryWriter bin)
        {
            bin.Write(WindVecMinX);
            bin.Write(WindVecMinY);
            bin.Write(WindVecMinZ);
            bin.Write(SubUnk4);
            bin.Write(WindVecMaxX);
            bin.Write(WindVecMaxY);
            bin.Write(WindVecMaxZ);
            bin.Write(SubUnk8);
            bin.Write(WindSwingCycle0);
            bin.Write(WindSwingCycle1);
            bin.Write(WindSwingCycle2);
            bin.Write(WindSwingCycle3);
            bin.Write(WindSwingPow0);
            bin.Write(WindSwingPow1);
            bin.Write(WindSwingPow2);
            bin.Write(WindSwingPow3);
        }
    }
}
