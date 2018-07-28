using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB.PARTS_PARAM_ST
{
    public class MsbPartsNPC : MsbPartsBase
    {
        public int UNK1 { get; set; } = 0;
        public int UNK2 { get; set; } = 0;

        public int ThinkParamID { get; set; } = 0;
        public int NPCParamID { get; set; } = 0;
        public int TalkID { get; set; } = 0;

        public int UNK3 { get; set; } = 0;

        public int CharaInitID { get; set; } = 0;

        internal int PartIndex { get; set; } = 0;

        public string CollisionName { get; set; } = "";

        public int UNK4 { get; set; } = 0;
        public int UNK5 { get; set; } = 0;

        internal short SolvedMovePointIndex1 { get; set; } = -1;
        internal short SolvedMovePointIndex2 { get; set; } = -1;
        internal short SolvedMovePointIndex3 { get; set; } = -1;
        internal short SolvedMovePointIndex4 { get; set; } = -1;

        public short MovePointIndex1 { get; set; } = -1;
        public short MovePointIndex2 { get; set; } = -1;
        public short MovePointIndex3 { get; set; } = -1;
        public short MovePointIndex4 { get; set; } = -1;

        public int UNK10 { get; set; } = 0;
        public int UNK11 { get; set; } = 0;

        public int InitAnimID { get; set; } = 0;

        public int UNK12 { get; set; } = 0;



        internal override PartsParamSubtype GetSubtypeValue()
        {
            return PartsParamSubtype.NPCs;
        }

        protected override void SubtypeRead(DSBinaryReader bin)
        {
            UNK1 = bin.ReadInt32();
            UNK2 = bin.ReadInt32();

            ThinkParamID = bin.ReadInt32();
            NPCParamID = bin.ReadInt32();
            TalkID = bin.ReadInt32();

            UNK3 = bin.ReadInt32();

            CharaInitID = bin.ReadInt32();
            PartIndex = bin.ReadInt32();

            UNK4 = bin.ReadInt32();
            UNK5 = bin.ReadInt32();

            SolvedMovePointIndex1 = bin.ReadInt16();
            SolvedMovePointIndex2 = bin.ReadInt16();
            SolvedMovePointIndex3 = bin.ReadInt16();
            SolvedMovePointIndex4 = bin.ReadInt16();

            UNK10 = bin.ReadInt32();
            UNK11 = bin.ReadInt32();

            InitAnimID = bin.ReadInt32();

            UNK12 = bin.ReadInt32();
        }

        protected override void SubtypeWrite(DSBinaryWriter bin)
        {
            bin.Write(UNK1);
            bin.Write(UNK2);

            bin.Write(ThinkParamID);
            bin.Write(NPCParamID);
            bin.Write(TalkID);

            bin.Write(UNK3);

            bin.Write(CharaInitID);
            bin.Write(PartIndex);

            bin.Write(UNK4);
            bin.Write(UNK5);

            bin.Write(SolvedMovePointIndex1);
            bin.Write(SolvedMovePointIndex2);
            bin.Write(SolvedMovePointIndex3);
            bin.Write(SolvedMovePointIndex4);

            bin.Write(UNK10);
            bin.Write(UNK11);

            bin.Write(InitAnimID);

            bin.Write(UNK12);
        }
    }
}
