using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowsBetterParamEditor
{
    public class UserConfig : INotifyPropertyChanged
    {
        [JsonIgnore]
        private string _interrootPath = null;

        public string InterrootPath
        {
            get => _interrootPath;
            set
            {
                _interrootPath = value;
                NotifyPropertyChanged(nameof(InterrootPath));
                NotifyPropertyChanged(nameof(MSBFolder));
            }
        }

        [JsonIgnore]
        public string MSBFolder
            => IOHelper.Frankenpath(InterrootPath, "map\\MapStudio\\");

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
