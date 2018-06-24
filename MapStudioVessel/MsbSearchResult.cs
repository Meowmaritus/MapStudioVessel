using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowsBetterParamEditor
{
    public class MsbSearchResult
    {
        public string MsbName { get; set; } = null;
        public string PrimaryTab { get; set; } = null;
        public string SecondaryTab { get; set; } = null;
        public string Row { get; set; } = null;
        public string PropertyName { get; set; } = null;
        public string PropertyValue { get; set; } = null;
        public object ActualRow { get; set; } = null;
    }
}
