namespace apbd_02;

class DeviceManager
{
    private const int MaxDevices = 15;
    static int NumberOfDevices = 0;
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
            Console.WriteLine("File not found: " + filePath);
            return;
        }
        foreach (var line in File.ReadAllLines(filePath))
        {
            Console.WriteLine(line);
            if (NumberOfDevices >= MaxDevices)
            {
                Console.WriteLine("Exceeding max devices");
                return;
            }
            try
            {
                var parts = line.Split(',');
                if (parts.Length < 3) continue;
                Device device = parts[0] switch
                {
                    "SW" => new Smartwatch { Id = ++NumberOfDevices, Name = parts[1], IsTurnedOn = bool.Parse(parts[2]), BatteryPercentage = int.Parse(parts[3]) },
                    "P" => new PersonalComputer { Id = ++NumberOfDevices, Name = parts[1], IsTurnedOn = bool.Parse(parts[2]), OperatingSystem = parts.Length >= 3 ? parts[3] : null },
                    "ED" => new EmbeddedDevice { Id = ++NumberOfDevices, Name = parts[1], IPAddress = parts[2], NetworkName = parts[3] },
                    _ => throw new Exception("Invalid device type")
                };
                devices.Add(device);
            }
            catch { /* Ignore corrupted lines */ }
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
}