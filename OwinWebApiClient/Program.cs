using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;
using OwinWebApiClient.ApiClients;
using OwinWebApiClient.Helper;


namespace OwinWebApiClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Wait for the async stuff to run...
            Run().Wait();

            // Then Write Done...
            Console.WriteLine("");
            Console.WriteLine("Done! Press the Enter key to Exit...");
            Console.ReadLine();
        }

        private static async Task Run()
        {
            try
            {
                // Create an rest service http client provider:
                string restUrl = "http://localhost:8080";
                Uri restUri = new Uri(restUrl);
                ApiClientProvider restServiceProvider = new ApiClientProvider(restUrl);

                // Pass in the credentials and retrieve a token dictionary:
                Dictionary<string, string> tokenDictionary = await restServiceProvider.GetTokenDictionary("admin", "AdminPassword");
                string restAccessToken = tokenDictionary["access_token"];
                
                // Write the contents of the dictionary:
                foreach (var kvp in tokenDictionary)
                {
                    Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
                    Console.WriteLine("");
                }


                // Create an vSphere http client provider:
                string vSphereUrl = @"https://192.168.75.195/sdk";
                string vSphereUserName = "root";
                string vSphereUserPassword = "P@ssw0rd";
                VSphereClientProvider vSpherePovider = new VSphereClientProvider(restUri, restAccessToken);

                // Create vSphere Session
                string vSphereSessionId = await vSpherePovider.GetSessionId(vSphereUrl, vSphereUserName, vSphereUserPassword);
                
                
                // Create a virtual machines info client instance:
                VirtualMachineClient vmClient = new VirtualMachineClient(restUri, restAccessToken, vSphereUrl, vSphereSessionId);
                
                // Read initial virtual machines name:
                Console.WriteLine("Read all the virtual machines...");
                List<string> vms = await vmClient.GetVirtualMachinesAsync();
                WriteVmList(vms);

                // Read initial virtual machines virtual disk info:
                string vm = vms.First();
                Console.WriteLine($"Read the virtual machine {vm} info...");
                var vmInfo = await vmClient.GetVirtualMachineAsync(vm);
                WriteVmInfo(vmInfo);                
            }
            catch (AggregateException ex)
            {
                // If it's an aggregate exception, an async error occurred:
                Console.WriteLine(ex.InnerExceptions[0].Message);
                Console.WriteLine("Press the Enter key to Exit...");
                Console.ReadLine();
                return;
            }
            catch (Exception ex)
            {
                // Something else happened:
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press the Enter key to Exit...");
                Console.ReadLine();
                return;
            }
        }

        private static void WriteVmInfo(VmInfoDto vmInfo)
        {
            if (vmInfo == null)
            {
                Console.WriteLine("VmInfo is null");
                return;
            }

            Console.WriteLine("Virtual disk info:");
            foreach (var disk in vmInfo.VirtualDisks)
            {
                Console.WriteLine(
                    $"Path: {disk.Path} Capacity: {FormatHelper.FormatBytes(disk.Capacity)} Free space: {FormatHelper.FormatBytes(disk.Capacity)}");
            }

            Console.WriteLine("CD-ROM info:");
            foreach (var disk in vmInfo.CdDisks)
            {
                Console.WriteLine($"Path: {disk.Path}");
            }
        }
        
        private static void WriteVmList(List<string> vms)
        {
            foreach (var vm in vms)
            {
                Console.WriteLine($"Name: {vm}");
            }
            Console.WriteLine("");
        }
        
    }
}