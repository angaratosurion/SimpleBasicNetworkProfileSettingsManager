using SimpleBasicNetworkProfileSettingsManager.Kernel.Data.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Text;

namespace SimpleBasicNetworkProfileSettingsManager.Core
{
    public class NetManager
    {
        public string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString(); 
        }
        public void SetProfile(Ip4Profile profile)
        {
            try
            {
                //string commandstatic = String.Format("interface ip set address {0) static {1} {2} {3}",profile.ConnectionName
                //    ,profile.IPAddress,profile.Mask,profile.GateWay);
                if (profile != null)
                {
                    var sb = new StringBuilder();
                    string commandstatic = "interface ip set address " + "\"" + profile.ConnectionName + "\"  static " + profile.IPAddress 
                        + " " +
                          profile.Mask + " " + profile.GateWay,dnsserversdhcpprim= "interface ip set dnsservers " + "\"" 
                          + profile.ConnectionName + "\"  static "+profile.PrimaryDns +" primary",
                          dnsserversstaticsec = "interface ip set dnsservers " + "\""
                          + profile.ConnectionName + "\"  static " + profile.SecondaryDns + " secondary"; ;


                    string commanddhcp = "interface ip set address "+"\""+ profile.ConnectionName+"\" "+" dhcp",
                        dnsserversdhcpcprim = "interface ip set dnsservers " + "\""
                          + profile.ConnectionName + "\"  dhcp " + profile.PrimaryDns + " primary",
                          dnsserversdhcpsec = "interface ip set dnsservers " + "\""
                          + profile.ConnectionName + "\"  dhcp " + profile.SecondaryDns + " secondary"; ; ;
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
                  inf.RedirectStandardOutput = true;
                    inf.RedirectStandardError = true;
                    pr.OutputDataReceived += (sender, args) => sb.AppendLine(args.Data);
                    pr.ErrorDataReceived += (sender, args) => sb.AppendLine(args.Data);
                    inf.UseShellExecute= false;
                    if (System.Environment.OSVersion.Version.Major >= 6)
                    {
                        inf.Verb = "runas";
                    }
                    pr.StartInfo = inf;
                    

                    pr.Start();
                    pr.BeginOutputReadLine();
                    pr.BeginErrorReadLine();
                   

                    // until we are done
                    pr.WaitForExit();
                    string output= sb.ToString();
                    if (profile.Static)
                    {
                        inf.Arguments = dnsserversdhcpprim;
                        pr.Start();
                        //pr.BeginOutputReadLine();
                        //pr.BeginErrorReadLine();


                        // until we are done
                        pr.WaitForExit();

                        inf.Arguments = dnsserversstaticsec;
                        pr.Start();
                        //pr.BeginOutputReadLine();
                        //pr.BeginErrorReadLine();


                        // until we are done
                        pr.WaitForExit();
                    }
                    else
                    {
                        inf.Arguments = dnsserversdhcpprim;
                        pr.Start();
                        //pr.BeginOutputReadLine();
                        //pr.BeginErrorReadLine();


                        // until we are done
                        pr.WaitForExit();

                        inf.Arguments = dnsserversdhcpsec;
                        pr.Start();
                        //pr.BeginOutputReadLine();
                        //pr.BeginErrorReadLine();


                        // until we are done
                        pr.WaitForExit();
                    }
                
                }
			}
			catch (Exception)
			{

				throw;
			}
        }
        public List<string> getAllinterfaces()
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