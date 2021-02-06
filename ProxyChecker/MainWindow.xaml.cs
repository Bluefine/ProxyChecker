using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using ProxyChecker.Models;
using ProxyChecker.View;
using ProxyChecker.ViewModel;

namespace ProxyChecker
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public Main Main;

        public MainWindow()
        {
            InitializeComponent();

            //Prevent tooltip from auto closing
            ToolTipService.ShowDurationProperty.OverrideMetadata(
                typeof(DependencyObject), new FrameworkPropertyMetadata(int.MaxValue));

            Main = new Main();
            DataContext = Main;
        }

        public List<Proxy> Proxies { get; set; }
        public bool IsRunning { get; set; }

        private void ButtonStartStop_OnClick(object sender, RoutedEventArgs e)
        {
            if (Proxies == null || Proxies.Count == 0)
            {
                Task.Run(() => StatusUpdate("Load proxy first"));
                return;
            }

            IsRunning = !IsRunning;
            if (IsRunning)
            {
                ButtonStartStop.Content = "Stop";
                Task.Run(() => StatusUpdate("Starting..."));
                Main.ResetData();
                Main.Total = Proxies.Count;
                Main.Left = Proxies.Count;
                DataGridProxies.Items.Clear();
                Task.Run(Checker);
            }
            else
            {
                Task.Run(() => StatusUpdate("Stopping..."));
                ButtonStartStop.Content = "Start";
            }
        }

        private async Task Checker()
        {
            foreach (var proxy in Proxies)
            {
                _ = Task.Run(() => CheckProxy(proxy));
                await Task.Delay(Main.SpeedSlider);
                if (!IsRunning) return;
            }
        }

        private async Task CheckProxy(Proxy proxy)
        {
            var http = new Http(Main.TimeoutSlider, proxy.Ip, Main.TargetTextBox);
            var result = await http.Get();
            if (!IsRunning) return;
            if (result == "Ok")
            {
                proxy.Status = "Ok";
                proxy.Ping = http.LastPing;
                Main.Good++;
            }
            else if (result == "Timeout")
            {
                proxy.Status = "Timeout";
                proxy.Ping = -1;
                Main.Timeout++;
            }
            else if (result == "Error")
            {
                proxy.Status = "Error";
                Main.Error++;
            }

            Main.Left--;

            Dispatcher.Invoke(() =>
            {
                DataGridProxies.Items.Add(new Proxy(proxy.Id, proxy.Ip, proxy.Ping, proxy.Status));
                if (Main.Left == 0)
                {
                    IsRunning = !IsRunning;
                    ButtonStartStop.Content = "Start";
                }
            });
        }

        private void LoadProxiesMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog {DefaultExt = ".txt"};
            var result = ofd.ShowDialog();
            if (result == true)
            {
                var filename = ofd.FileName;
                var data = File.ReadAllLines(filename);
                Proxies = new List<Proxy>();
                foreach (var p in data) Proxies.Add(new Proxy(Proxies.Count, p, 0, ""));
                Task.Run(() => StatusUpdate("Loaded " + data.Length + " proxies"));
                Main.ResetData();
                Main.Total = Proxies.Count;
                Main.Left = Proxies.Count;
            }
        }

        private async Task StatusUpdate(string msg)
        {
            await Dispatcher.Invoke(async () =>
            {
                StatusTransition.Content = msg;
                await Task.Delay(3000);
                StatusTransition.Content = "";
            });
        }

        private void ButtonExportProxies_OnClick(object sender, RoutedEventArgs e)
        {
            if (Proxies == null)
            {
                Task.Run(() => StatusUpdate("Check some proxies before exporting to .txt"));
                return;
            }

            if (Main.Left > 0)
            {
                Task.Run(() => StatusUpdate("Proxies are being checked wait until it's done before exporting"));
                return;
            }

            var goodProxies = Proxies.Where(x => x.Status == "Ok").Select(y => y.Ip).ToList();
            File.WriteAllLines("proxies.txt", goodProxies);
            Task.Run(() => StatusUpdate("Saved to proxies.txt"));
        }

        private void AboutMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var window = new About();
            window.Show();
        }
    }
}