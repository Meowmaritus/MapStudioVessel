using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB.POINT_PARAM_ST
{
    public class MsbRegionCylinder : MsbRegionBase
    {
        internal int UNK1 { get; set; } = 0;
        internal int UNK2 { get; set; } = 0;
        public float Radius { get; set; } = 1;
        public float Height { get; set; } = 2;
        public int EntityID { get; set; } = -1;

        public MsbRegionCylinder(MsbRegionList parentList)
        {
            this.Index = parentList.Count;
        }

        internal override (int, int, int) GetOffsetDeltas()
        {
            return (4, 8, 16);
        }

        internal override PointParamSubtype GetSubtypeValue()
        {
            return PointParamSubtype.Cylinders;
        }

        protected override void SubtypeRead(DSBinaryReader bin)
        {
            UNK1 = bin.ReadInt32();
            UNK2 = bin.ReadInt32();
            Radius = bin.ReadSingle();
            Height = bin.ReadSingle();
            EntityID = bin.ReadInt32();
        }

        protected override void SubtypeWrite(DSBinaryWriter bin)
        {
            bin.Write(UNK1);
            bin.Write(UNK2);
            bin.Write(Radius);
            bin.Write(Height);
            bin.Write(EntityID);
        }
    }
}
