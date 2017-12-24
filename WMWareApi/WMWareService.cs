using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using VMware.Vim;
using WMWareApi.Model;

namespace WMWareApi
{
    public class WMWareService
    {
        private readonly string _serviceUrl;
        private readonly string _userName;
        private readonly string _password;

        private readonly VimClient _client;

        public WMWareService(string serviceUrl, string userName, string password)
        {
            _serviceUrl = serviceUrl;
            _userName = userName;
            _password = password;
            _client = new VimClientImpl();
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        }

        public List<string> GetVirtualMachines()
        {
            try
            {
                Connect();
                List<EntityViewBase> vmList = GetAllVirtualMachines();
                List<string> vmNames = vmList?.Select(x => ((VirtualMachine) x).Name).ToList();
                return vmNames;
            }
            finally
            {
                _client.Disconnect();
            }
        }

        private void Connect()
        {
            // connect to vSphere web service
            _client.Connect(_serviceUrl);
            // Login using username/_password credentials
            _client.Login(_userName, _password);            
        }

        private List<EntityViewBase> GetAllVirtualMachines()
        {
            return _client.FindEntityViews(typeof(VirtualMachine), null, null, null);
        }

        public VirtualMachineInfo GetVirtualMachineInfo(string vmName)
        {
            try
            {
                Connect();
                VirtualMachine vm = GetVirtualMachineByName(vmName);
                if(vm == null) throw new ArgumentNullException("VM not found");

                VirtualMachineInfo vmInfo = new VirtualMachineInfo();
                vmInfo.Name = vm.Name;
                vmInfo.VirtualDisks = new List<VirtualDiskInfo>();

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
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _client.Disconnect();
            }
        }

        private VirtualMachine GetVirtualMachineByName(string vmName)
        {
            List<EntityViewBase> vmList = GetAllVirtualMachines();
            return vmList?.Where(x => ((VirtualMachine)x).Name == vmName)
                          .Select(x => (VirtualMachine)x)
                          .FirstOrDefault();
        }

        //public VmWareInfo GetVirtualMachineInfo(string vmName)
        //{
        //    try
        //    {
        //        Connect();
        //        VirtualMachine vm = GetVirtualMachineByName(vmName);
        //        if(vm == null) throw new ArgumentNullException("VM not found");

        //        VmWareInfo vmInfo = new VmWareInfo();
        //        vmInfo.Name = vm.Name;

        //        //return GetVmWareInfo(vm);
        //        vmInfo.Datastore = new Model.DatastoreInfo();
        //        foreach (ManagedObjectReference mor in vm.Datastore)
        //        {
        //            Datastore datastore = (Datastore)_client.GetView(mor, null);
        //            var vmfsInfo = datastore.Info as VmfsDatastoreInfo;
        //            if(vmfsInfo == null) continue;

        //            vmInfo.Datastore.Name = vmfsInfo.Name;
        //            vmInfo.Datastore.Type = vmfsInfo.Vmfs.Type;
        //            vmInfo.Datastore.Url = vmfsInfo.Url;
        //            vmInfo.Datastore.Uuid = vmfsInfo.Vmfs.Uuid;
        //            vmInfo.Datastore.Ssd = vmfsInfo.Vmfs.Ssd;
        //            vmInfo.Datastore.FreeSpace = datastore.Info.FreeSpace;
        //            vmInfo.Datastore.Capacity = vmfsInfo.Vmfs.Capacity;

        //            vmInfo.Datastore.VirtualDisk = new VirtualDiskInfo();
        //            foreach (HostScsiDiskPartition partition in vmfsInfo.Vmfs.Extent)
        //            {
        //                vmInfo.Datastore.VirtualDisk.Name = partition.Name;
        //                vmInfo.Datastore.VirtualDisk.Partition = partition.Partition;                            
        //            }                    
        //        }
        //        return vmInfo;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        _client.Disconnect();
        //    }
        //}

        private VmWareInfo GetVmWareInfo(VirtualMachine vm)
        {
            return null;
            //VmWareInfo vmInfo = new VmWareInfo();
            //vmInfo.Name = vm.Name;
            //vmInfo.VirtualDisk = new VirtualDiskInfo();
            //vmInfo.CdDisk = new CdDiskInfo();

            //foreach (var device in vm.Config.Hardware.Device)
            //{
            //    if (device is VMware.Vim.VirtualCdrom)
            //    {

            //    }
            //    if (device is VMware.Vim.VirtualDisk)
            //    {

            //    }
            //}

            //return vmInfo;
        }

    }
}