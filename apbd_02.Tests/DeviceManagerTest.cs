using apbd_02;
using System;
using System.IO;
using Xunit;
using apbd_02;
using System.Collections.Generic;


namespace DeviceManagerTests
{
    public class DeviceManagerUnitTests
    {
        private const string TestFilePath = "test_devices.txt";

        private void CreateTestFile(params string[] lines)
        {
            File.WriteAllLines(TestFilePath, lines);
        }

        [Fact]
        public void Loads_Valid_Smartwatch()
        {
            CreateTestFile("SW-1,Galaxy Watch,true,45%");

            var manager = new DeviceManager(TestFilePath);
            Assert.Single(GetDevices(manager));
        }

        [Fact]
        public void Skips_Invalid_Lines()
        {
            CreateTestFile(
                "SW-1,Apple Watch,true,90%",
                "ED-1,Pi,NotAnIP,MD Ltd. WiFi", // invalid IP
                "P-1,MyPC,false" // no OS
            );

            var manager = new DeviceManager(TestFilePath);
            var devices = GetDevices(manager);
            Assert.Single(devices); // only SW should load
        }

        [Fact]
        public void TurnOn_Smartwatch_Reduces_Battery()
        {
            var watch = new Smartwatch { Id = 1, Name = "Fitbit", BatteryPercentage = 50 };
            watch.TurnOn();
            Assert.True(watch.IsTurnedOn);
            Assert.Equal(40, watch.BatteryPercentage);
        }

        [Fact]
        public void TurnOn_PC_Without_OS_Throws()
        {
            var pc = new PersonalComputer { Id = 1, Name = "NoOSPC" };
            Assert.Throws<Exception>(() => pc.TurnOn());
        }

        [Fact]
        public void AddDevice_Increases_Count()
        {
            var manager = new DeviceManager(TestFilePath);
            int countBefore = GetDevices(manager).Count;

            manager.AddDevice(new PersonalComputer { Id = 99, Name = "TestPC", OperatingSystem = "Linux" });

            int countAfter = GetDevices(manager).Count;
            Assert.Equal(countBefore + 1, countAfter);
        }

        [Fact]
        public void RemoveDevice_Decreases_Count()
        {
            CreateTestFile("P-1,Laptop,false,Windows");

            var manager = new DeviceManager(TestFilePath);
            int initial = GetDevices(manager).Count;

            manager.RemoveDevice(1);
            int after = GetDevices(manager).Count;

            Assert.Equal(initial - 1, after);
        }

        private List<Device> GetDevices(DeviceManager manager)
        {
            // HACK: Uses reflection to access the private device list
            var field = typeof(DeviceManager).GetField("devices", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field?.GetValue(manager) as List<Device> ?? new List<Device>();
        }
    }
}
