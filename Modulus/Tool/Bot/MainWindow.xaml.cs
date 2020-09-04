using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Tool.Bot.UI;
using Tool.Bot.Utils;
using Tool.Bot.Protocol;

namespace Tool.Bot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IBotUI
    {
        public MainWindow()
        {
            InitializeComponent();
            Api.BotUI = this;
            Api.WriteDebugLog("Program Start!!!!");

            tbClientCount.Text = Config.Instance.ClientCount.ToString();
            tbServerIp.Text = Config.Instance.LobbyServerIP;
            tbServerPort.Text = Config.Instance.LobbyServerPort.ToString();

            cbRandomCancelMatching.IsChecked = Config.Instance.RandomCancelMatching;
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            Config.Instance.ClientCount = int.Parse(tbClientCount.Text);
            Config.Instance.LobbyServerIP = tbServerIp.Text;
            Config.Instance.LobbyServerPort = Convert.ToUInt16(tbServerPort.Text);

            for (int i = 0; i < Config.Instance.ClientCount; i++)
            {
                var client = new BotClient();
                client.Index = Config.Instance.StartIndex + i;
                client.PlayCount = 0;
                client.Connect(Config.Instance.LobbyServerIP, Config.Instance.LobbyServerPort);
                dgClientList.Items.Add(client);
            }

            Config.SaveToFile();
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            Protocol.Client.DisconnectAll();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Engine.Network.Api.CleanUp();
            Engine.Framework.Api.CleanUp();
        }

        public void ShowLog(string log)
        {
            Application.Current.Dispatcher.Invoke(() => { lbLog.Items.Add(log); });
        }

        public void RemoveClient(Client client)
        {
            Application.Current.Dispatcher.Invoke(() => { dgClientList.Items.Remove(client); });
        }

        private void CbRandomCancelMatching_Click(object sender, RoutedEventArgs e)
        {
            Config.Instance.RandomCancelMatching = (sender as CheckBox).IsChecked ?? false;
        }
    }
}
