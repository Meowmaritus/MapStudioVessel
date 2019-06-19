using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB.PARTS_PARAM_ST
{
    public class MsbPartsNPC : MsbPartsBase
    {
        internal override void DebugPushUnknownFieldReport_Subtype(out string subtypeName, Dictionary<string, object> dict)
        {
            subtypeName = "NPC";

            dict.Add(nameof(SUB_CONST_1), SUB_CONST_1);
            dict.Add(nameof(SUB_CONST_2), SUB_CONST_2);

            dict.Add(nameof(PointMoveType), PointMoveType);
            dict.Add(nameof(SUB_CONST_3), SUB_CONST_3);

            dict.Add(nameof(SUB_CONST_4), SUB_CONST_4);
            dict.Add(nameof(SUB_CONST_5), SUB_CONST_5);
        }

        internal int SUB_CONST_1 { get; set; } = 0;
        internal int SUB_CONST_2 { get; set; } = 0;

        public int ThinkParamID { get; set; } = 0;
        public int NPCParamID { get; set; } = 0;
        public int TalkID { get; set; } = 0;

        public byte PointMoveType { get; set; } = 0;
        internal byte SUB_CONST_3 { get; set; } = 0;
        public ushort PlatoonID { get; set; } = 0;

        public int CharaInitID { get; set; } = -1;

        internal int i_HitName { get; set; } = 0;
        public string HitName { get; set; } = MiscUtil.BAD_REF;

        internal int SUB_CONST_4 { get; set; } = 0;
        internal int SUB_CONST_5 { get; set; } = 0;

        internal short SolvedMovePointIndex1 { get; set; } = -1;
        internal short SolvedMovePointIndex2 { get; set; } = -1;
        internal short SolvedMovePointIndex3 { get; set; } = -1;
        internal short SolvedMovePointIndex4 { get; set; } = -1;
        internal short SolvedMovePointIndex5 { get; set; } = -1;
        internal short SolvedMovePointIndex6 { get; set; } = -1;
        internal short SolvedMovePointIndex7 { get; set; } = -1;
        internal short SolvedMovePointIndex8 { get; set; } = -1;

        public string MovePoint1 { get; set; } = "";
        public string MovePoint2 { get; set; } = "";
        public string MovePoint3 { get; set; } = "";
        public string MovePoint4 { get; set; } = "";
        public string MovePoint5 { get; set; } = "";
        public string MovePoint6 { get; set; } = "";
        public string MovePoint7 { get; set; } = "";
        public string MovePoint8 { get; set; } = "";

        public int InitAnimID { get; set; } = -1;

        public int m17_Butterfly_Anim_Unk { get; set; } = -1;



        internal override PartsParamSubtype GetSubtypeValue()
        {
            return PartsParamSubtype.NPCs;
        }

        protected override void SubtypeRead(DSBinaryReader bin)
        {
            SUB_CONST_1 = bin.ReadInt32();
            SUB_CONST_2 = bin.ReadInt32();

            ThinkParamID = bin.ReadInt32();
            NPCParamID = bin.ReadInt32();
            TalkID = bin.ReadInt32();

            PointMoveType = bin.ReadByte();
            SUB_CONST_3 = bin.ReadByte();
            PlatoonID = bin.ReadUInt16();

            CharaInitID = bin.ReadInt32();
            i_HitName = bin.ReadInt32();

            SUB_CONST_4 = bin.ReadInt32();
            SUB_CONST_5 = bin.ReadInt32();

            SolvedMovePointIndex1 = bin.ReadInt16();
            SolvedMovePointIndex2 = bin.ReadInt16();
            SolvedMovePointIndex3 = bin.ReadInt16();
            SolvedMovePointIndex4 = bin.ReadInt16();
            SolvedMovePointIndex5 = bin.ReadInt16();
            SolvedMovePointIndex6 = bin.ReadInt16();
            SolvedMovePointIndex7 = bin.ReadInt16();
            SolvedMovePointIndex8 = bin.ReadInt16();

            InitAnimID = bin.ReadInt32();

            m17_Butterfly_Anim_Unk = bin.ReadInt32();
        }

        protected override void SubtypeWrite(DSBinaryWriter bin)
        {
            bin.Write(SUB_CONST_1);
            bin.Write(SUB_CONST_2);

            bin.Write(ThinkParamID);
            bin.Write(NPCParamID);
            bin.Write(TalkID);

            bin.Write(PointMoveType);
            bin.Write(SUB_CONST_3);
            bin.Write(PlatoonID);

            bin.Write(CharaInitID);
            bin.Write(i_HitName);

            bin.Write(SUB_CONST_4);
            bin.Write(SUB_CONST_5);

            bin.Write(SolvedMovePointIndex1);
            bin.Write(SolvedMovePointIndex2);
            bin.Write(SolvedMovePointIndex3);
            bin.Write(SolvedMovePointIndex4);
            bin.Write(SolvedMovePointIndex5);
            bin.Write(SolvedMovePointIndex6);
            bin.Write(SolvedMovePointIndex7);
            bin.Write(SolvedMovePointIndex8);

            bin.Write(InitAnimID);

            bin.Write(m17_Butterfly_Anim_Unk);
        }
    }
}
