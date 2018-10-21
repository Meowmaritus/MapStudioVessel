using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB
{
    public abstract class MsbEventBase : MsbStruct
    {
        public string Name { get; set; } = "";
        public int EventIndex { get; set; } = -1;
        public int Index { get; set; } = -1;
        public int Ux18 { get; set; } = 0;

        //First Pointer
        internal int i_Part { get; set; } = 0;
        public string Part { get; set; } = "";

        internal int i_Region { get; set; } = 0;
        public string Region { get; set; } = "";

        public int EntityID { get; set; } = 0;

        public int Unknown2 { get; set; } = 0;

        //Second Pointer
        protected abstract void SubtypeRead(DSBinaryReader bin);
        protected abstract void SubtypeWrite(DSBinaryWriter bin);
        protected abstract EventParamSubtype GetSubtypeValue();

        internal EventParamSubtype Type => GetSubtypeValue();

        protected override void InternalRead(DSBinaryReader bin)
        {
            Name = bin.ReadMsbString();
            EventIndex = bin.ReadInt32();
            bin.AssertInt32((int)Type);
            Index = bin.ReadInt32();

            int baseDataOffset = bin.ReadInt32();
            int subtypeDataOffset = bin.ReadInt32();

            Ux18 = bin.ReadInt32();

            bin.StepInMSB(baseDataOffset);
            {
                i_Part = bin.ReadInt32();
                i_Region = bin.ReadInt32();
                EntityID = bin.ReadInt32();
                Unknown2 = bin.ReadInt32();
            }
            bin.StepOut();

            bin.StepInMSB(subtypeDataOffset);
            {
                SubtypeRead(bin);
            }
            bin.StepOut();
        }

        protected override void InternalWrite(DSBinaryWriter bin)
        {
            bin.Placeholder($"EVENT_PARAM_ST|{Type}|Name");
            bin.Write(EventIndex);

            bin.Write((int)Type);

            bin.Write(Index);

            bin.Placeholder($"EVENT_PARAM_ST|{Type}|(BASE DATA OFFSET)");
            bin.Placeholder($"EVENT_PARAM_ST|{Type}|(SUBTYPE DATA OFFSET)");

            bin.Write(Ux18);

            //bin.StartMSBStrings();
            {
                bin.Replace($"EVENT_PARAM_ST|{Type}|Name", bin.MsbOffset);
                bin.WriteMsbString(Name);

                bin.Pad(align: 0x04);
            }
            //bin.EndMSBStrings(blockSize: 0x10);

            bin.Replace($"EVENT_PARAM_ST|{Type}|(BASE DATA OFFSET)", bin.MsbOffset);
            bin.Write(i_Part);
            bin.Write(i_Region);
            bin.Write(EntityID);
            bin.Write(Unknown2);

            //PADDING
            bin.Write((int)0);

            bin.Replace($"EVENT_PARAM_ST|{Type}|(SUBTYPE DATA OFFSET)", bin.MsbOffset);
            SubtypeWrite(bin);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
