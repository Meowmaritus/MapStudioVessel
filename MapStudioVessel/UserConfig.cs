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
        
        private string _interrootPath = null;
        private string _mapFolder = null;
        private string _game = null;

        [JsonIgnore]
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
           => IOHelper.Frankenpath(MapFolder, "MapStudio\\");

        public string MapFolder
        {
            get => _mapFolder;
            set
            {
                _mapFolder = value;
                //NotifyPropertyChanged(nameof(InterrootPath));
                NotifyPropertyChanged(nameof(MapFolder));
                NotifyPropertyChanged(nameof(MSBFolder));
            }
        }

        public string Game
        {
            get => _game;
            set
            {
                _game = value;
                //NotifyPropertyChanged(nameof(InterrootPath));
                NotifyPropertyChanged(nameof(Game));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
