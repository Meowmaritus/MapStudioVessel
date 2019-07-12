using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowDSIO.DataTypes.MSB.EVENT_PARAM_ST
{
    public class MsbEventObjAct : MsbEventBase
    {
        internal override void DebugPushUnknownFieldReport_Subtype(out string subtypeName, Dictionary<string, object> dict)
        {
            subtypeName = "ObjAct";

            dict.Add(nameof(ObjActState), ObjActState);
        }

        public int ObjActEntityID { get; set; } = 0;

        internal int i_ObjName { get; set; } = 0;
        public string ObjName { get; set; } = MiscUtil.BAD_REF;

        public short ObjActParamEntryID { get; set; } = 0;

        public EventsObjActsObjActState ObjActState { get; set; } = 0;

        public int EventFlagID { get; set; } = 0;

        protected override EventParamSubtype GetSubtypeValue()
        {
            return EventParamSubtype.ObjActs;
        }

        protected override void SubtypeRead(DSBinaryReader bin)
        {
            ObjActEntityID = bin.ReadInt32();
            i_ObjName = bin.ReadInt32();
            ObjActParamEntryID = bin.ReadInt16();
            ObjActState = (EventsObjActsObjActState)bin.ReadInt16();
            EventFlagID = bin.ReadInt32();
        }

        protected override void SubtypeWrite(DSBinaryWriter bin)
        {
            bin.Write(ObjActEntityID);
            bin.Write(i_ObjName);
            bin.Write(ObjActParamEntryID);
            bin.Write((short)ObjActState);
            bin.Write(EventFlagID);
        }
    }
}
