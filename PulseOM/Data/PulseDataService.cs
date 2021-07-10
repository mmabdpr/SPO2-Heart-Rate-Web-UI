using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PulseOM.Data
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
    public class DataItem
    {
        public DateTime Time { get; set; }
        public long HearBeat { get; set; }
        public long Oxygen { get; set; }
    }
    
    public class PulseDataService: IDisposable
    {
        private UdpClient Client { get; }
        private List<DataItem> _hbData = new();
        public IEnumerable<DataItem> Data => _hbData;
        
        public PulseDataService()
        {
            Client = new UdpClient(54251);
            Client.BeginReceive(OnReceive, null);
        }
        
        private void OnReceive(IAsyncResult res)
        {
            try
            {
                var remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                var received = Client.EndReceive(res, ref remoteIpEndPoint);

                var msg = Encoding.UTF8.GetString(received)
                    .Split(',').Select(long.Parse).ToArray();

                _hbData.Add(new DataItem
                {
                    Time = DateTimeOffset.FromUnixTimeMilliseconds(msg[0]).DateTime.ToLocalTime(),
                    HearBeat = msg[1],
                    Oxygen = msg[2],
                });

                if (_hbData.Count > 10)
                    _hbData = _hbData.Skip(_hbData.Count - 10).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Client.BeginReceive(OnReceive, null);
            }
        }

        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}