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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tools;
using Model;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace gmc2_TM1_Documentation_Tool
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        Tools.Tools tool;
        //private ObservableCollection<string> serverList;

        private ObservableCollection<string> timestampList;
        public ObservableCollection<string> TimestampList
        {
            get
            {
                return timestampList;
            }
            set
            {
                timestampList = value;
                NotifyPropertyChanged("TimestampList"); // method implemented below
            }
        }

        private ObservableCollection<string> tm1ServerList;
        public ObservableCollection<string> TM1ServerList
        {
            get
            {
                return tm1ServerList;
            }
            set
            {
                tm1ServerList = value;
                NotifyPropertyChanged("TM1ServerList"); // method implemented below
            }
        }

        private ObservableCollection<string> cubeList;
        public ObservableCollection<string> CubeList
        {
            get
            {
                return cubeList;
            }
            set
            {
                cubeList = value;
                NotifyPropertyChanged("CubeList"); // method implemented below
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            //savePathTextBox.Text = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"gmc2\");
            //savePathTextBox.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string savePath = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"gmc2\");

            if (!System.IO.Directory.Exists(savePath))
                System.IO.Directory.CreateDirectory(savePath);

            DocumentationServer server = (this.checkboxXML.IsChecked.Value ? 
                tool.createDocServer(savePath, this.xmlFileNameTextBox.Text, timestampsComboBox.SelectedIndex, tm1ServerComboBox.SelectedIndex) : 
                tool.CreateDocServer(timestampsComboBox.SelectedIndex, tm1ServerComboBox.SelectedIndex));
            VisioDrawer.DocServerToADAPT.Draw(server, this.checkboxSysCubes.IsChecked.Value, this.checkboxSysDims.IsChecked.Value, savePath);

            string msg = "ADAPT Export " + (checkboxXML.IsChecked.Value ? "and XML File " : "") + "saved in " + savePath;
            MessageBox.Show(msg);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Tools.Tools.Kill();
            this.tool = Tools.Tools.Init(this.adminHostTextBox.Text);
            if(tool.ServerConnect(this.serverTextBox.Text, this.loginTextBox.Text, this.pwBox.Password))
                cubesComboBox.IsEnabled = true;
        }

        private void CubesComboBox_OnDropDownOpened(object sender, EventArgs e)
        {
            CubeList = new ObservableCollection<string>(tool.GetCubes());
            cubesComboBox.ItemsSource = CubeList;

            if (CubeList.Contains(@"}gmc2_server_documentation"))
            {
                cubesComboBox.Text = @"}gmc2_server_documentation";
                cubesComboBox.SelectedValue = @"}gmc2_server_documentation";
            }
        }

        private void TimestampsComboBox_OnDropDownOpened(object sender, EventArgs e)
        {
            TimestampList = new ObservableCollection<string>(tool.GetTimeStamps());
            timestampsComboBox.ItemsSource = TimestampList;         
        }

        private void TM1ServerComboBox_OnDropDownOpened(object sender, EventArgs e)
        {
            TM1ServerList = new ObservableCollection<string>(tool.GetTM1Servers());
            tm1ServerComboBox.ItemsSource = TM1ServerList;
        }

        private void CubesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tool.SetDocCube(cubesComboBox.SelectedValue.ToString());
            timestampsComboBox.IsEnabled = true;
        }

        private void TimestampsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tm1ServerComboBox.IsEnabled = true;
        }

        private void TM1ServerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkboxXML.IsEnabled = true;
            checkboxSysCubes.IsEnabled = true;
            checkboxSysDims.IsEnabled = true;
            goBtn.IsEnabled = true;
        }

        private void CheckboxXML_Checked(object sender, RoutedEventArgs e)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Tools.Tools.Kill();
        }

        private void gmc2Logo_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
