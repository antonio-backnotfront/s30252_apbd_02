namespace apbd_02;

/// <summary>
/// <param name="title">Device manage fjaifnainfkafa</param>
/// </summary>
class DeviceManager
{
    private const int MaxDevices = 15;
    private List<Device> devices = new();
    private string filePath;

    public DeviceManager(string filePath)
    {
        this.filePath = filePath;
        LoadDevices();
    }

    private void LoadDevices()
    {
        
       
        
        
        if (!File.Exists(filePath)) 
        {
            return;
        }
        // string[] lines = File.ReadAllLines(filePath);
        foreach (var line in File.ReadAllLines(filePath, System.Text.Encoding.UTF8))
        {
            // Console.WriteLine(line);
            if (devices.Count >= MaxDevices)
            {
                Console.WriteLine("Exceeding max devices");
                return;
            }
            
            
            try
            {
                Device device;
                var parts = line.Split(',');
                if (parts.Length < 3) continue;
                string typePrefix = parts[0].Split('-')[0];

                if (typePrefix.StartsWith("SW") && parts.Length >= 4)
                {
                    device = new Smartwatch
                    {
                        Id = devices.Count+1,
                        Name = parts[1],
                        IsTurnedOn = parts[2].ToLower() == "true",
                        BatteryPercentage = int.Parse(parts[3].TrimEnd('%'))
                    };
                }
                else if (typePrefix.StartsWith("P") && parts.Length > 3)
                {
                    device = new PersonalComputer
                    {
                        Id = devices.Count+1,
                        Name = parts[1],
                        IsTurnedOn = parts[2].ToLower() == "true",
                        OperatingSystem = parts.Length > 3 ? parts[3] : null
                    };
                }
                else if (typePrefix.StartsWith("ED") && parts.Length >= 4 && parts[3].Contains("MD Ltd."))
                {
                    device = new EmbeddedDevice
                    {
                        Id = devices.Count+1,
                        Name = parts[1],
                        IpAddress = parts[2],
                        NetworkName = parts[3]
                    };
                    
                }
                else
                {
                    Console.WriteLine("Skipped invalid or unknown device type line: " + line);
                    continue;
                }
                Console.WriteLine(device);
                devices.Add(device);
            }
            catch (Exception e){Console.WriteLine(e.Message); }
        }
    }
    /// <summary>
    /// Add device to a device manager
    /// </summary>
    /// <param name="device"></param>
    /// <exception cref="Exception"></exception>
    public void AddDevice(Device device)
    {
        if (devices.Count >= MaxDevices) throw new Exception("Storage is full!");
        devices.Add(device);
    }
    /// <summary>
    /// Remove device from the DeviceManager
    /// </summary>
    /// <param name="id"></param>
    public void RemoveDevice(int id) => devices.RemoveAll(d => d.Id == id);
    /// <summary>
    /// Show all the devices of the DeviceManaegr
    /// </summary>
    public void ShowDevices() => devices.ForEach(Console.WriteLine);
    
    /// <summary>
    /// Save all the devices in the file
    /// </summary>
    public void SaveDevices() => File.WriteAllLines(filePath, devices.ConvertAll(d => d.ToString()));
    
    /// <summary>
    /// Turn on the device by the specified id
    /// </summary>
    /// <param name="id"></param>
    public void TurnOn(int id) => devices.First(i => i.Id == id).TurnOn();
    
    /// <summary>
    /// Turn off the device by the specified id
    /// </summary>
    /// <param name="id"></param>
    public void TurnOff(int id) => devices.First(i => i.Id == id).TurnOff();
    
    /// <summary>
    /// Edit the device data by specifying the device id as the first argument
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newId"></param>
    /// <param name="name"></param>
    /// <param name="isTurnedOn"></param>
    public void EditDeviceData(int id, int newId, string name, bool isTurnedOn){
        devices.First(i => i.Id == id).Id = newId;
        devices.First(i => i.Id == id).Name = name;
        devices.First(i => i.Id == id).IsTurnedOn = isTurnedOn;
    }
}