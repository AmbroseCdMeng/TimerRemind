using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;

namespace TimerRemind {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //创建定时器对象
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        TimeSpan ts = new TimeSpan(0, 0, 0);

        const int WAITING_HOURS = 0;
        const int WAITING_MINUTES = 0;
        const int WAITING_SECONDS = 59;

        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.IsEnabled = true;
        }

        /// <summary>
        /// 窗体加载时开启定时器，读取xml文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
            dispatcherTimer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            var eList = ReadLineXML("reminder.xml");
            var current_time = DateTime.Now.ToString("HH:mm:ss");

            var rm = new Random();

            int r1 = rm.Next(50, 256);
            int r2 = rm.Next(50, 256);
            int r3 = rm.Next(50, 256);

            Top = rm.Next(30, Convert.ToInt32(SystemParameters.PrimaryScreenHeight - Height) - 30);
            Left = rm.Next(30, Convert.ToInt32(SystemParameters.PrimaryScreenWidth - Width) - 30);

            foreach (var item in eList) {
                var time = item["time"];
                var text = item["text"];
                if (current_time == time) {
                    ts = new TimeSpan(WAITING_HOURS, WAITING_MINUTES, WAITING_SECONDS);
                    L_RemindText.Content = text;
                    WindowState = WindowState.Normal;
                }
                L_RemindText.Foreground = new SolidColorBrush(Color.FromArgb(122, Convert.ToByte(r1), Convert.ToByte(r2), Convert.ToByte(r3)));
            }

            ts = ts.Subtract(new TimeSpan(0, 0, 1));
            if (ts.TotalSeconds < 0.0) {
                L_RemindText.Foreground = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                WindowState = WindowState.Minimized;
            }
        }

        /// <summary>
        /// 读取XML文档
        /// </summary>
        /// <param name="fileName"></param>
        private List<Dictionary<string, string>> ReadLineXML(string fileName)
        {
            List<Dictionary<string, string>> eList = new List<Dictionary<string,string>>();

            XmlDocument xml = new XmlDocument();

            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) { return eList; }
            xml.Load(fileName);

            XmlElement root = xml.DocumentElement;//获取根节点
            XmlNodeList xnl = xml.SelectNodes("/reminder/event");

            foreach (XmlNode item in xnl)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("time", item.Attributes["time"].Value);
                dic.Add("text", item.Attributes["text"].Value);
                eList.Add(dic);
            }
            return eList;
        }


        /// <summary>
        /// MouseDown to Close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void L_RemindText_MouseDown(object sender, MouseButtonEventArgs e) {
            L_RemindText.Foreground = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e) {
            L_RemindText.Foreground = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            WindowState = WindowState.Minimized;
        }
    }
}

