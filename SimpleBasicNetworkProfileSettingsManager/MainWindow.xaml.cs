using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Net.NetworkInformation;
using SimpleBasicNetworkProfileSettingsManager.Core;
using SimpleBasicNetworkProfileSettingsManager.Kernel.Data.Models;

namespace SimpleBasicNetworkProfileSettingsManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NetManager core = new NetManager();
        public MainWindow()
        {
            InitializeComponent();
            var lst = core.getAllinterfaces();
            core.CreateProfileFolder();

            if( lst != null )
            {
                foreach(var item in lst )
                {
                    cbxInterfaces.Items.Add( item );

                }
            }
            this.Title = String.Format("{0} - Version : {1} Kernel Version: {2}", Application.Current.MainWindow.GetType().Assembly.GetName().Name, 
                Application.Current.MainWindow.GetType().Assembly.GetName().Version,core.GetVersion());
            //this.stpProfileSelection.Width = this.Width - this.stpButtons.Width;
            //this.stpButtons.HorizontalAlignment = HorizontalAlignment.Left;
        }

        private void cbxInterfaces_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var netinnf = core.GetNetworkInterface((string)cbxInterfaces.SelectedValue);
            this.Title = (string)cbxInterfaces.SelectedValue;
            if( netinnf != null )
            {

                foreach (UnicastIPAddressInformation ip in netinnf.GetIPProperties().UnicastAddresses)
                {
                    //if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        lblIpAddress.Content=ip.Address.ToString();
                        lblMask.Content=ip.IPv4Mask.ToString();
                        lblGateWay.Content=netinnf.GetIPProperties().GatewayAddresses.FirstOrDefault()?.Address.ToString();
                        lblPrimaryDNS.Content= netinnf.GetIPProperties().DnsAddresses[0].ToString();
                        if (netinnf.GetIPProperties().DnsAddresses.Count > 1)
                        {
                            lblSecondaryDNS.Content = netinnf.GetIPProperties().DnsAddresses[1].ToString();
                        }
                    }
                }



            }
            
        }

        private void btnCreateProlfile_Click(object sender, RoutedEventArgs e)
        {
            this.stpProfileMaker.Visibility = Visibility.Visible;
        }

        private void btnSaveProfile_Click(object sender, RoutedEventArgs e)
        {
            Ip4Profile ip4Profile= new Ip4Profile();
            ip4Profile.ConnectionName = (string)cbxInterfaces.SelectedValue;
            ip4Profile.Mask = (string)lblMask.Content;
            ip4Profile.IPAddress = (string)lblIpAddress.Content;
            ip4Profile.GateWay=(string)lblGateWay.Content;
            ip4Profile.Static = (bool)chkStatic.IsChecked;
            ip4Profile.PrimaryDns=(string) lblPrimaryDNS.Content ;
            ip4Profile.SecondaryDns= (string)lblSecondaryDNS.Content;
            core.SaveProfile(ip4Profile,  txtProFileName.Text);
            this.stpProfileMaker.Visibility = Visibility.Collapsed;
        }

        private void cbxProfiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var profile = core.GetProfile((string)cbxProfiles.SelectedValue);
           // this.Title = (string)cbxInterfaces.SelectedValue;
            if (profile != null)
            {

               
                    //if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                    txtIpAddress.Text = profile.IPAddress;
                        txtMask.Text =profile.Mask;
                    txtGateWay.Text = profile.GateWay;
                    chkProfStatic.IsChecked = profile.Static;
                    txtPrimaryDNS.Text = profile.PrimaryDns;
                    txtSecondaryDNS.Text = profile.SecondaryDns;
                    }
               foreach(var item in cbxProfInterfaces.Items)
                {
                    if( item.Equals(profile.ConnectionName))
                    {
                        cbxProfInterfaces.SelectedValue = item;
                        break;
                    }
                         
                }



            }

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
       {
        //    if (this.tbControl.SelectedIndex == 1)
        //    {
                
        //    }
        //    else if (this.tbControl.SelectedIndex == 0)
        //    {

        //        cbxProfiles.Items.Clear();
        //        cbxProfInterfaces.Items.Clear();
        //    }
            
        }

        private void txtApplyProfile_Click(object sender, RoutedEventArgs e)
        {
            var profile = core.GetProfile((string)cbxProfiles.SelectedValue);

            core.SetProfile(profile);
        }

        private void txtEditProfile_Click(object sender, RoutedEventArgs e)
        {
            core.DeleteProfile((string)cbxProfiles.SelectedValue);

            Ip4Profile ip4Profile = new Ip4Profile();
            ip4Profile.ConnectionName = (string)cbxProfInterfaces.SelectedValue;
            ip4Profile.Mask = (string)txtMask.Text;
            ip4Profile.IPAddress = (string)txtIpAddress.Text;
            ip4Profile.GateWay = (string)txtGateWay.Text;
            ip4Profile.Static = (bool)chkStatic.IsChecked;
            ip4Profile.PrimaryDns = txtPrimaryDNS.Text;
            ip4Profile.SecondaryDns =  txtSecondaryDNS.Text;

            core.SaveProfile(ip4Profile, (string)cbxProfiles.SelectedValue);
        }

        private void txtDeleteProfile_Click(object sender, RoutedEventArgs e)
        {
            core.DeleteProfile((string)cbxProfiles.SelectedValue);
        }

        private void cbxProfInterfaces_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            var lst = core.GetProfiles();
            if (lst != null)
            {
                //cbxProfiles.Items.Clear();
                foreach (var profile in lst)
                {
                    cbxProfiles.Items.Add(profile);
                }
            }
            var iflst = core.getAllinterfaces();


            if (iflst != null)
            {
                // cbxProfInterfaces.Items.Clear();
                foreach (var item in iflst)
                {
                    cbxProfInterfaces.Items.Add(item);

                }
            }
        }
    }
}
