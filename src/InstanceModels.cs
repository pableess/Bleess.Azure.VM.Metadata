using System;
using System.Collections.Generic;
using System.Text;

namespace Bleess.Azure.VM.Metadata
{
    public record VmInstance(Compute Compute, Network network);

    public record Compute(string AzEnvironment,
        string EvictionPolicy,
        string Priority,
        bool IsHostCompatibilityLayerVm, 
        string LicenseType,
        string Location, 
        string Name, 
        string Offer,
        OsProfile OsProfile,
        string OsType,
        string PlacementGroupId,
        Plan Plan,
        int PlatformFaultDomain,
        int PlatformUpdateDomain,
        IList<PublicKey> PublicKeys,
        string Publisher,
        string ResourceGroupName,
        string ResourceId,
        SecurityProfile SecurityProfile,
        string Sku,
        StorageProfile StorageProfile,
        string SubscriptionId,
        string Tags,
        string Version,
        string VmId,
        string VmScaleSetName,
        string VmSize,
        string Zone);

    public record Network(IList<Interface> Interface);

    public record Interface(Ip Ipv4, Ip Ipv6, string MacAddress);

    public record Ip(IList<IpAddress> IpAddress, IList<Subnet> Subnet);

    public record IpAddress(string PrivateIpAddress, string PublicIpAddress);

    public record Subnet(string Address, string Prefix);


    public record OsProfile (string AdminUsername, string ComputerName, bool DisablePasswordAuthentication);

    public record Plan(string Name, string Product, string Publisher);
    
    public record PublicKey(string KeyData, string Path);

    public record SecurityProfile(bool SecureBootEnabled, bool VirtualTpmEnabled);

    public record StorageProfile(IList<DataDisk> DataDisks, ImageReference ImageReference, OsDisk OsDisk);

    public record DataDisk(string Caching, string CreationOption, int DiskSizeGB, Image Image, int Lun, ManagedDisk ManagedDisk, string Name, Vhd Vhd, bool writeAcceleratorEnabled);

    public record Image(string Uri);

    public record ManagedDisk(string Id, string StorageAccountType);

    public record Vhd(string uri);

    public record ImageReference(string Id, string Offer, string Publisher, string Sku, string Version);

    public record OsDisk(string Caching, string CreationOption, int DiskSizeGB, DiffDiskSettings DiffDiskSettings, EncryptionSettings EncryptionSettings, Image Image, ManagedDisk ManadedDisk, string Name, string OsType, Vhd Vhd, bool writeAcceleratorEnabled);

    public record DiffDiskSettings(string Options);

    public record EncryptionSettings(bool Enabled);
}
