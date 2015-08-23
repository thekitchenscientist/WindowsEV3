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
using Lego.Ev3.Core;
using Lego.Ev3.Desktop;
using System.Diagnostics;

namespace LegoLogger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Brick _brick;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        
        // Create a list to hold sensor data
        public List<string> LogList = new List<string>();
        
        // Stuff that happens when the program is loaded
        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //connect to brick via bluetooth
            _brick = new Brick(new BluetoothCommunication("COM3"));
            _brick.BrickChanged += _brick_BrickChanged;
            await _brick.ConnectAsync();
            await _brick.DirectCommand.PlayToneAsync(50, 1000, 300);
            // Add comma seperated titles to list
            LogList.Add("buttonpressed,xcoordinate,ycoordinate");
        }

        //stuff that happen every time the sensor values change
        private void _brick_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            var buttonpressed = _brick.Ports[InputPort.One].SIValue;
            var xcoordinate = _brick.Ports[InputPort.A].SIValue;
            var ycoordinate = _brick.Ports[InputPort.B].SIValue;
            //create comma sperated list of data
            string LogValues = buttonpressed.ToString() + "," + xcoordinate.ToString() +","+ ycoordinate.ToString();
            //add to list
            LogList.Add(LogValues);

    }
        //action to take on button press
        private void ShowLogButton_Click(object sender, RoutedEventArgs e)
        {
            //method 1 to write log
            System.IO.File.WriteAllLines(@"C:\EV3\Log.csv", LogList);

            //method 2
            //add a carriage return after each string
            //var text = string.Empty;
            //foreach (String s in LogList)
            //{
            //    text += s.ToString() + "\r\n";
            //}
            //display the final list
            //MessageBox.Show(text);

            //System.IO.File.WriteAllText(@"C:\EV3\Log2.csv", text);

        }

    }

}
