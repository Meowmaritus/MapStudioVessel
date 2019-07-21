using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MeowsBetterParamEditor
{
    /// <summary>
    /// Interaction logic for SelectGameWindow.xaml
    /// </summary>
    public partial class SelectGameWindow : Window
    {
        private string GameSelect;

        public SelectGameWindow()
        {
            InitializeComponent();
        }

        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxMapPath.Text == "") {
                MessageBox.Show("Path hasn't been set!");
            }
            else
            {
                if (((MainWindow)Application.Current.MainWindow).CheckMapDirValid(TextBoxMapPath.Text)) {
                    ((MainWindow)Application.Current.MainWindow).PARAMDATA.Config.MapFolder = TextBoxMapPath.Text;
                    ((MainWindow)Application.Current.MainWindow).PARAMDATA.Config.Game = GameSelect;
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Couldn't find MapStudio folder in chosen directory.");
                }
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ChangeGameAndPlatform(object sender, RoutedEventArgs e)
        {
            RadioButton radio = (sender as RadioButton);
            GameSelect = radio.Name;
        }

        private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
        {
            var browseDialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Select map folder containing MapStudio folder"
            };

            System.Windows.Forms.DialogResult result = browseDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                TextBoxMapPath.Text = browseDialog.SelectedPath;
            }
        }
    }
}
