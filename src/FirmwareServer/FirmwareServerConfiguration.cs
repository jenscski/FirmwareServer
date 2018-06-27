namespace FirmwareServer
{
    public class FirmwareServerConfiguration
    {
        public string Password { get; set; }

        public string AppData { get; set; }

        public bool IsRunningInContainer { get; set; }
    }
}