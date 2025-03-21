namespace apbd_02;

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

    public void AddDevice(Device device)
    {
        if (devices.Count >= MaxDevices) throw new Exception("Storage is full!");
        devices.Add(device);
    }
    public void RemoveDevice(int id) => devices.RemoveAll(d => d.Id == id);
    public void ShowDevices() => devices.ForEach(Console.WriteLine);
    public void SaveDevices() => File.WriteAllLines(filePath, devices.ConvertAll(d => d.ToString()));
    public void TurnOn(int id) => devices.First(i => i.Id == id).TurnOn();
    public void TurnOff(int id) => devices.First(i => i.Id == id).TurnOff();
    public void EditDeviceData(int id, int newId, string name, bool isTurnedOn){
        devices.First(i => i.Id == id).Id = newId;
        devices.First(i => i.Id == id).Name = name;
        devices.First(i => i.Id == id).IsTurnedOn = isTurnedOn;
    }
}