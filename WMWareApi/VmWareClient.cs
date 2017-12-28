using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using VMware.Vim;
using WMWareApi.Model;

namespace WMWareApi
{
    public class VmWareClient
    {
        private readonly string _serviceUrl;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _sessionId;
        private readonly VimClient _client;

        private VmWareClient()
        {
            _client = new VimClientImpl();
            // отключаем на время разработки
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }


        public VmWareClient(string serviceUrl, string sessionId) : this()
        {
            _serviceUrl = serviceUrl;
            _sessionId = sessionId;
            
        }

        public VmWareClient(string serviceUrl, string userName, string password):this()
        {
            _serviceUrl = serviceUrl;
            _userName = userName;
            _password = password;           
        }
        
        public List<string> GetVirtualMachines()
        {
            ConnectToSession();
            List<EntityViewBase> vmList = GetAllVirtualMachines();
            List<string> vmNames = vmList?.Select(x => ((VirtualMachine)x).Name).ToList();
            return vmNames;
        }

        public string Connect()
        {
            // connect to vSphere service
            _client.Connect(_serviceUrl);
            // Login using username/_password credentials
            _client.Login(_userName, _password);
            return _client.SessionSecret;
        }

        private void ConnectToSession()
        {
            _client.ConnectToSession(_serviceUrl, _sessionId);            
        }

        private List<EntityViewBase> GetAllVirtualMachines()
        {
            return _client.FindEntityViews(typeof(VirtualMachine), null, null, null);
        }
        
        private List<EntityViewBase> GetAllVirtualMachineByName(string name)
        {
            var filter = new NameValueCollection { { "name", $"^{Regex.Escape(name)}$" } };
            return _client.FindEntityViews(typeof(VirtualMachine), null, filter, null);
        }
        
        public VirtualMachineInfo GetVirtualMachineInfo(string vmName)
        {
            ConnectToSession();
            VirtualMachine vm = GetVirtualMachineByName(vmName);
            if (vm == null) throw new ArgumentNullException("VM not found");

            VirtualMachineInfo vmInfo = new VirtualMachineInfo();
            vmInfo.Name = vm.Name;
            vmInfo.VirtualDisks = new List<VirtualDiskInfo>();
            vmInfo.CdDisks = new List<CdDiskInfo>();

            if (vm.Guest?.Disk != null)
                foreach (GuestDiskInfo diskInfo in vm.Guest?.Disk)
                {
                    vmInfo.VirtualDisks.Add(new VirtualDiskInfo()
                    {
                        Name = diskInfo.ToString(),
                        Path = diskInfo.DiskPath,
                        Capacity = diskInfo.Capacity,
                        FreeSpace = diskInfo.FreeSpace
                    });
                }

            return vmInfo;
        }

        private VirtualMachine GetVirtualMachineByName(string vmName)
        {
            List<EntityViewBase> vmList = GetAllVirtualMachineByName(vmName);
            return vmList?.Select(x => (VirtualMachine)x).FirstOrDefault();
        }
        
    }
}