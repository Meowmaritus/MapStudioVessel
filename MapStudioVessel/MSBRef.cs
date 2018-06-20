using MeowDSIO.DataFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowsBetterParamEditor
{
    public class MSBRef
    {
        private static Dictionary<MSBRef, string> FancyDisplayNameCache = new Dictionary<MSBRef, string>();

        public string FancyDisplayName
        {
            get
            {
                return Key.Replace(".msb", "");
                //try
                //{
                //    return FancyDisplayNameCache[this];
                //}
                //catch (KeyNotFoundException)
                //{
                //    if (ParamDataContext.SpecialInternalParamNameOverrides.ContainsKey(Key))
                //        FancyDisplayNameCache.Add(this, ParamDataContext.SpecialInternalParamNameOverrides[Key]);
                //    else
                //        FancyDisplayNameCache.Add(this, Value.ID);

                //    // If it somehow throw the KeyNotFoundException here, then there's a problem lol
                //    return FancyDisplayNameCache[this];
                //}
            }
        }

        public string Key { get; set; } = null;
        public MSB Value { get; set; } = null;

        public MSBRef() { }

        public MSBRef(string Key, MSB Value)
        {
            this.Key = Key;
            this.Value = Value;
        }

        public override string ToString()
        {
            return $"{Key}";
        }
    }
}
