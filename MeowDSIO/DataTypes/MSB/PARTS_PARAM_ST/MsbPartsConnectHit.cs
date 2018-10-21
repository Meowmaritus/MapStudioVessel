using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB.PARTS_PARAM_ST
{
    public class MsbPartsConnectHit : MsbPartsBase
    {
        internal byte UNK1 { get; set; } = 0;
        internal byte UNK2 { get; set; } = 0;
        internal byte UNK3 { get; set; } = 0;
        internal byte UNK4 { get; set; } = 0;

        public string MapName { get; set; } = "?MeowDSIO_MsbPartsConnectHit_MapName?";

        internal int UNK7 { get; set; } = 0;
        internal int UNK8 { get; set; } = 0;

        internal override PartsParamSubtype GetSubtypeValue()
        {
            return PartsParamSubtype.ConnectHits;
        }

        protected override void SubtypeRead(DSBinaryReader bin)
        {
            UNK1 = bin.ReadByte();
            UNK2 = bin.ReadByte();
            UNK3 = bin.ReadByte();
            UNK4 = bin.ReadByte();

            sbyte mapId1 = bin.ReadSByte();
            sbyte mapId2 = bin.ReadSByte();
            sbyte mapId3 = bin.ReadSByte();
            sbyte mapId4 = bin.ReadSByte();

            string mapNamePart1, mapNamePart2, mapNamePart3, mapNamePart4;

            if (mapId1 == -1)
                mapNamePart1 = "XX";
            else if (mapId1 <= 99)
                mapNamePart1 = $"{mapId1:D2}";
            else
                mapNamePart1 = "??";

            if (mapId2 == -1)
                mapNamePart2 = "XX";
            else if (mapId2 <= 99)
                mapNamePart2 = $"{mapId2:D2}";
            else
                mapNamePart2 = "??";

            if (mapId3 == -1)
                mapNamePart3 = "XX";
            else if (mapId3 <= 99)
                mapNamePart3 = $"{mapId3:D2}";
            else
                mapNamePart3 = "??";

            if (mapId4 == -1)
                mapNamePart4 = "XX";
            else if (mapId4 <= 99)
                mapNamePart4 = $"{mapId4:D2}";
            else
                mapNamePart4 = "??";

            MapName = $"m{mapNamePart1}_{mapNamePart2}_{mapNamePart3}_{mapNamePart4}";

            UNK7 = bin.ReadInt32();
            UNK8 = bin.ReadInt32();
        }

        private void InvalidNameException()
        {
            throw new Exception($"[Map Connections] \"{MapName}\" is not a valid map name.");
        }

        protected override void SubtypeWrite(DSBinaryWriter bin)
        {
            bin.Write(UNK1);
            bin.Write(UNK2);
            bin.Write(UNK3);
            bin.Write(UNK4);

            sbyte m1 = -1, m2 = -1, m3 = -1, m4 = -1;

            string[] mapNameParts = MapName
                .Substring(1)
                .Split('_')
                .Select(x => x.ToUpper())
                .ToArray();

            if (mapNameParts.Length != 4)
                InvalidNameException();

            if (mapNameParts[0].Length != 2
                || mapNameParts[1].Length != 2
                || mapNameParts[2].Length != 2
                || mapNameParts[3].Length != 2
                )
            {
                InvalidNameException();
            }

            try
            {
                if (mapNameParts[0] == "XX")
                {
                    m1 = -1;
                }
                else
                {
                    m1 = (sbyte)(int.Parse(mapNameParts[0]));
                    if (m1 < 0)
                        InvalidNameException();
                }

                if (mapNameParts[1] == "XX")
                {
                    m2 = -1;
                }
                else
                {
                    m2 = (sbyte)(int.Parse(mapNameParts[1]));
                    if (m2 < 0)
                        InvalidNameException();
                }

                if (mapNameParts[2] == "XX")
                {
                    m3 = -1;
                }
                else
                {
                    m3 = (sbyte)(int.Parse(mapNameParts[2]));
                    if (m3 < 0)
                        InvalidNameException();
                }

                if (mapNameParts[3] == "XX")
                {
                    m4 = -1;
                }
                else
                {
                    m4 = (sbyte)(int.Parse(mapNameParts[3]));
                    if (m4 < 0)
                        InvalidNameException();
                }
            }
            catch
            {
                InvalidNameException();
            }

            bin.Write(m1);
            bin.Write(m2);
            bin.Write(m3);
            bin.Write(m4);


            bin.Write(UNK7);
            bin.Write(UNK8);
        }
    }
}
