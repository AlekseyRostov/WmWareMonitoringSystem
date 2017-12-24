using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using VMware.Vim;
using WMWareApi;
using WMWareApi.Model;

namespace DemoClient
{
    class Program
    {
        const string serviceUrl = @"https://192.168.1.69/sdk";
        const string userName = "root";
        const string password = "P@ssw0rd";

        static void Main(string[] args)
        {
            VmApiTest();
            Console.ReadLine();

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            Run();

            Console.ReadLine();
        }
        
        private static void VmApiTest()
        {
            try
            {
                WMWareService service = new WMWareService(serviceUrl, userName, password);
                List<string> vms = service.GetVirtualMachines();
                VirtualMachineInfo vmInfo = service.GetVirtualMachineInfo(vms.SingleOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void Run()
        {
            try
            {
                VimClient client = new VimClientImpl();
                // connect to vSphere web service

                client.Connect(serviceUrl);
                // Login using username/password credentials
                client.Login(userName, password);
                // Get DiagnosticManager

                var vmList = client.FindEntityViews(typeof(VirtualMachine), null, null, null);
                foreach (VirtualMachine vm in vmList)
                {
                    Console.WriteLine(vm.Name);

                    HostSystem host = (HostSystem)client.GetView(vm.Runtime.Host, null);



                    // Datastores
                    foreach (ManagedObjectReference mor in vm.Datastore)
                    {
                        Datastore datastore = (Datastore)client.GetView(mor, null);
                        var vmfsInfo = datastore.Info as VmfsDatastoreInfo;

                        Console.WriteLine($"Name: {vmfsInfo.Name}");
                        Console.WriteLine($"Type: {vmfsInfo.Vmfs.Type}");
                        Console.WriteLine($"Location: {vmfsInfo.Url}");
                        Console.WriteLine($"UUID: {vmfsInfo.Vmfs.Uuid}");
                        Console.WriteLine($"SSD: {vmfsInfo.Vmfs.Ssd}");

                        Console.WriteLine("Memory");
                        Console.WriteLine($"FreeSpace: {FormatBytes(datastore.Info.FreeSpace)}");
                        Console.WriteLine($"Capacity: {FormatBytes(vmfsInfo.Vmfs.Capacity)}");

                        foreach (HostScsiDiskPartition partition in vmfsInfo.Vmfs.Extent)
                        {
                            Console.WriteLine($"Disk name: {partition.DiskName} Partition: {partition.Partition}");
                            //datastore.Host
                        }
                        //Console.WriteLine($"Capacity: {ConvertToMb(vmfsInfo.Vmfs.Capacity)}");

                    }
                }

                //var hostlist = client.FindEntityViews(typeof(HostVStorageObjectManager), null, null, null);

                //Populate the Host names into the Host ListBox
                //foreach (HostSystem vmhost in hostlist)
                //{
                //    Console.WriteLine(vmhost.Name);

                //    foreach (Datastore store in (Datastore)vmhost.Datastore)
                //    {

                //    }
                //}

                // DataCenters
                IList dcList = client.FindEntityViews(typeof(Datacenter), null, null, null);
                foreach (Datacenter dc in dcList)
                {
                    
                }
                
                Console.WriteLine("//--------------------------------------//");

                //DiagnosticManager diagMgr = (DiagnosticManager)client.GetView(client.ServiceContent.DiagnosticManager, null);
                //// Obtain the last line of the logfile by setting an arbitrarily large
                //// line number as the starting point
                //DiagnosticManagerLogHeader log = diagMgr.BrowseDiagnosticLog(null, "hostd", 999999999, null);
                //int lineEnd = log.LineEnd;
                //// Get the last 5 lines of the log first
                //int start = lineEnd - 5;
                //log = diagMgr.BrowseDiagnosticLog(null, "hostd", start, null);
                //foreach (string line in log.LineText)
                //{
                //    Console.WriteLine(line);
                //}
                // Logout from the vSphere server
                client.Disconnect();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private static string FormatBytes(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }

        private static string ConvertToMb(long info)
        {
            if(info == null) return "null";

            double result = (info/1024)/1024;
            return result.ToString();
        }
    }
}
