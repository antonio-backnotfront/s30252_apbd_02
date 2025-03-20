using apbd_02;

class Program
{
    static void Main()
    {
        DeviceManager manager = new("input.txt");
        try
        {
            manager.ShowDevices();
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
    }
}