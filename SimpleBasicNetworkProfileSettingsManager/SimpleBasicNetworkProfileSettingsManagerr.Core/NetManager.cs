using SimpleBasicNetworkProfileSettingsManager.Kernel.Data.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;

namespace SimpleBasicNetworkProfileSettingsManager.Core
{
    public class NetManager
    {

        public void SetProfile(Ip4Profile profile)
        {
            try
            {
                string commandstatic = String.Format("interface ip set address {0) static {1} {2} {3}",profile.ConnectionName
                    ,profile.IPAddress,profile.Mask,profile.GateWay);
                if (profile != null)
                {
                    string commanddhcp = String.Format("interface ip set address {0) dhcp", profile.ConnectionName);
                    Process pr = new Process();
                    ProcessStartInfo inf;
                    if (profile.Static)
                    {
                        inf  = new ProcessStartInfo("netsh", commandstatic);
                    }
                    else
                    {
                          inf = new ProcessStartInfo("netsh", commanddhcp);
                    }
                    pr.StartInfo = inf;
                    pr.Start();
                }
			}
			catch (Exception)
			{

				throw;
			}
        }
        public List<string> getAllintrfaces()
        {
            try
            {
                List<string> ap = null;
                var interfaces = NetworkInterface.GetAllNetworkInterfaces();
                if(interfaces != null)
                {
                    ap= new List<string>();
                    foreach(NetworkInterface intf in interfaces)
                    {
                        ap.Add(intf.Name);
                    }

                }

                return ap;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public NetworkInterface GetNetworkInterface(string name)
        {
            try
            {
                NetworkInterface ap = null;
                if(name!=null)
                {
                   var lst=NetworkInterface.GetAllNetworkInterfaces();
                    if(lst!= null)
                    {
                        foreach(NetworkInterface intf in lst)
                        {
                            if( intf.Name==name)
                            {
                                ap= intf;
                                break;
                            }
                        }
                    }
                }


                return ap;


            }
            catch (Exception)
            {

                throw;
            }
        }
        public void CreateProfileFolder()
        {
            try
            {
                ;//System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                string path,ap;//= System.IO.Path.GetDirectoryName(pathwithextention).Replace("file:\\", "");
                //ap = pathwithextention.Replace("file:\\", "");
                path = AppContext.BaseDirectory;
                string pathwithextention = AppContext.BaseDirectory;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    ap = pathwithextention.Replace("file:\\", "");
                }
                else
                {
                    ap = pathwithextention.Replace("file:", "");
                }
                path = Path.Combine(ap, "Profiles");
                if( !Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public string GeteProfileFolder()
        {
            try
            {
                ;//System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                string path, ap;//= System.IO.Path.GetDirectoryName(pathwithextention).Replace("file:\\", "");
                //ap = pathwithextention.Replace("file:\\", "");
                path = AppContext.BaseDirectory;
                string pathwithextention = AppContext.BaseDirectory;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    ap = pathwithextention.Replace("file:\\", "");
                }
                else
                {
                    ap = pathwithextention.Replace("file:", "");
                }
                path = Path.Combine(ap, "Profiles");
               
                return path;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void SaveProfile(Ip4Profile profile,string name)
        {
            try
            {
                if(profile!=null && name!=null)
                {
                    string json = JsonConvert.SerializeObject(profile);
                    File.WriteAllText(Path.Combine(GeteProfileFolder(),name+".profile"), json);
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
        public void DeleteProfile( string name)
        {
            try
            {
                if (  name != null)
                {
                    
                    if( File.Exists(Path.Combine(GeteProfileFolder(), name + ".profile")))
                    {
                        File.Delete(Path.Combine(GeteProfileFolder(), name + ".profile"));
                    
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
        public Ip4Profile GetProfile(string name)
        {
            try
            {
                Ip4Profile ap = null;
                string filename = Path.Combine(GeteProfileFolder(), name + ".profile");
                if (File.Exists(filename))
                {
                    ap=JsonConvert.DeserializeObject<Ip4Profile>(File.ReadAllText(filename));
                }


                return ap;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<string> GetProfiles()
        {
            try
            {
                List<string> ap = new List<string>();
                var files= Directory.GetFiles(GeteProfileFolder(),"*.profile");
               foreach(var file in files)
                {
                    string tfile= Path.GetFileNameWithoutExtension(file);
                    ap.Add(tfile);
                }

                return ap;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}