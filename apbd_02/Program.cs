using apbd_02;

class Program
{
    static void Main()
    {
        DeviceManager manager = new("input.txt");
        try
        {
            Console.WriteLine("Connecting...");
            manager.ShowDevices();
            manager.EditDeviceData(1,100,"Apple",true);
            manager.ShowDevices();
            manager.SaveDevices();
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
    }
}

