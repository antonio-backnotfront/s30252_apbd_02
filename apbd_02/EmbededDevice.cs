

using System.Text.RegularExpressions;

namespace apbd_02;

class EmbeddedDevice : Device
{
    private  string IpAddress;
    public string IPAddress
    {
        get => IpAddress;
        set
        {
            if (!Regex.IsMatch(value, "^\\b(?:\\d{1,3}\\.){3}\\d{1,3}\\b$"))
                throw new ArgumentException("Invalid IP Address format.");
            IpAddress = value;
        }
    }
    public string NetworkName { get; set; }

    public void Connect()
    {
        if (!NetworkName.Contains("MD Ltd."))
            throw new Exception("ConnectionException: Network name must contain 'MD Ltd.'.");
    }

    public override void TurnOn()
    {
        Connect();
        IsTurnedOn = true;
    }
    public override string ToString() => $"Embedded Device [ID: {Id}, Name: {Name}, IP: {IPAddress}, Network: {NetworkName}, On: {IsTurnedOn}]";
}

