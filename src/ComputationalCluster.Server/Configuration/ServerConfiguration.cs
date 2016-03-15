using System;
using System.Configuration;

namespace ComputationalCluster.Server.Configuration
{
    public class ServerConfiguration : IServerConfiguration
    {
        public ServerConfiguration()
        {
            LoadSetings();
        }

        public int ListeningPort { get; set; }

        public ServerMode Mode { get; set; }

        public string MasterServerAddress { get; set; }

        public int MasterServerPort { get; set; }

        public uint Timeout { get; set; }

        private void LoadSetings()
        {
            //TODO: validation
            var settings = ConfigurationManager.AppSettings;
            ListeningPort = int.Parse(settings[nameof(ListeningPort)]);
            Mode = (ServerMode)Enum.Parse(typeof(ServerMode), settings[nameof(Mode)]);
            if (Mode == ServerMode.Backup)
            {
                MasterServerAddress = settings[nameof(MasterServerAddress)];
                MasterServerPort = int.Parse(settings[nameof(MasterServerPort)]);
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
