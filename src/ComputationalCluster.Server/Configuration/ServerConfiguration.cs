using System;
using System.Configuration;

namespace ComputationalCluster.Server.Configuration
{
    public class ServerConfiguration : IServerConfiguration
    {
        public ServerConfiguration()
        {
            LoadSettings();
        }

        public int ListeningPort { get; set; }

        public ServerMode Mode { get; set; }

        public string ServerAddress { get; set; }

        public int ServerPort { get; set; }

        public uint Timeout { get; set; }


        private void LoadSettings()
        {
            //TODO: validation
            var settings = ConfigurationManager.AppSettings;
            ListeningPort = int.Parse(settings[nameof(ListeningPort)]);
            Mode = (ServerMode)Enum.Parse(typeof(ServerMode), settings[nameof(Mode)]);
            if (Mode == ServerMode.Backup)
            {
                ServerAddress = settings["MasterServerAddress"];
                ServerPort = int.Parse(settings["MasterServerPort"]);
            }
            Timeout = uint.Parse(settings[nameof(Timeout)]);
        }
    }

    public enum ServerMode
    {
        Primary,
        Backup
    }
}
