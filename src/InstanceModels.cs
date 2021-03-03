using System;
using System.Collections.Generic;
using System.Text;

namespace Bleess.Azure.VM.Metadata
{

    /// <summary>
    /// The vm instance metadata
    /// </summary>
    /// <param name="Compute"></param>
    /// <param name="Network"></param>
    public record VmInstance(Compute Compute, Network Network) : RecordBase;

    /// <summary>
    /// Compute information
    /// </summary>
    /// <param name="AzEnvironment"></param>
    /// <param name="EvictionPolicy"></param>
    /// <param name="Priority"></param>
    /// <param name="IsHostCompatibilityLayerVm"></param>
    /// <param name="LicenseType"></param>
    /// <param name="Location"></param>
    /// <param name="Name"></param>
    /// <param name="Offer"></param>
    /// <param name="OsProfile"></param>
    /// <param name="OsType"></param>
    /// <param name="PlacementGroupId"></param>
    /// <param name="Plan"></param>
    /// <param name="PlatformFaultDomain"></param>
    /// <param name="PlatformUpdateDomain"></param>
    /// <param name="PublicKeys"></param>
    /// <param name="Publisher"></param>
    /// <param name="ResourceGroupName"></param>
    /// <param name="ResourceId"></param>
    /// <param name="SecurityProfile"></param>
    /// <param name="Sku"></param>
    /// <param name="StorageProfile"></param>
    /// <param name="SubscriptionId"></param>
    /// <param name="Tags"></param>
    /// <param name="Version"></param>
    /// <param name="VmId"></param>
    /// <param name="VmScaleSetName"></param>
    /// <param name="VmSize"></param>
    /// <param name="Zone"></param>
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
        string Zone) : RecordBase;

    /// <summary>
    /// Network information
    /// </summary>
    /// <param name="Interface"></param>
    public record Network(IList<Interface> Interface) : RecordBase;

    /// <summary>
    /// Network Interface information
    /// </summary>
    public record Interface(Ip Ipv4, Ip Ipv6, string MacAddress) : RecordBase;

    /// <summary>
    /// IP Information
    /// </summary>
    public record Ip(IList<IpAddress> IpAddress, IList<Subnet> Subnet) : RecordBase;

    /// <summary>
    /// IP Adress
    /// </summary>
    public record IpAddress(string PrivateIpAddress, string PublicIpAddress) : RecordBase;

    /// <summary>
    /// Network Subnet
    /// </summary>
    public record Subnet(string Address, string Prefix) : RecordBase;

    /// <summary>
    /// The OS Profile
    /// </summary>
    public record OsProfile (string AdminUsername, string ComputerName, bool DisablePasswordAuthentication) : RecordBase;

    /// <summary>
    /// The Plan information
    /// </summary>
    public record Plan(string Name, string Product, string Publisher) : RecordBase;
    
    /// <summary>
    /// Public Key information
    /// </summary>
    public record PublicKey(string KeyData, string Path) : RecordBase;

    /// <summary>
    /// The security profile
    /// </summary>
    public record SecurityProfile(bool SecureBootEnabled, bool VirtualTpmEnabled) : RecordBase;

    /// <summary>
    /// The storage profile
    /// </summary>
    public record StorageProfile(IList<DataDisk> DataDisks, ImageReference ImageReference, OsDisk OsDisk) : RecordBase;

    /// <summary>
    /// Data disk information
    /// </summary>
    public record DataDisk(string Caching, string CreationOption, int DiskSizeGB, Image Image, int Lun, ManagedDisk ManagedDisk, string Name, Vhd Vhd, bool writeAcceleratorEnabled) : RecordBase;

    /// <summary>
    /// VM Image information
    /// </summary>
    public record Image(string Uri) : RecordBase;

    /// <summary>
    /// Managed disk information
    /// </summary>
    public record ManagedDisk(string Id, string StorageAccountType) : RecordBase;

    /// <summary>
    /// VHD information
    /// </summary>
    public record Vhd(string uri) : RecordBase;

    /// <summary>
    /// VM Image refernce
    /// </summary>
    public record ImageReference(string Id, string Offer, string Publisher, string Sku, string Version) : RecordBase;

    /// <summary>
    /// OS Disk information
    /// </summary>
    public record OsDisk(string Caching, string CreationOption, int DiskSizeGB, DiffDiskSettings DiffDiskSettings, EncryptionSettings EncryptionSettings, Image Image, ManagedDisk ManadedDisk, string Name, string OsType, Vhd Vhd, bool writeAcceleratorEnabled) : RecordBase;

    /// <summary>
    /// Diff disk settings
    /// </summary>
    public record DiffDiskSettings(string Options) : RecordBase;

    /// <summary>
    /// Encryption settings
    /// </summary>
    public record EncryptionSettings(bool Enabled) : RecordBase;
}
