using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace PulseOM.Data
{
    public class PulseDataService : IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IEnumerable<PulseDataItem> _hbData = new List<PulseDataItem>();
        private IdentityUser? _user;


        public PulseDataService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

            Client = new UdpClient(54251);
            Client.BeginReceive(OnReceive, null);
        }

        private UdpClient Client { get; }

        public IEnumerable<PulseDataItem> Data
        {
            get
            {
                if (_user is null) return _hbData;

                using (var scope = _scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    _hbData = db.PulseData
                        .Where(x => x.IdentityUserId != null && x.IdentityUserId.Equals(_user.Id))
                        .OrderByDescending(x => x.Time)
                        .Take(100)
                        .ToList();
                }

                return _hbData;
            }
        }

        public void Dispose()
        {
            Client.Dispose();
        }

        public void SetUser(IdentityUser? user)
        {
            _user = user;
        }

        private void OnReceive(IAsyncResult res)
        {
            try
            {
                var remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                var received = Client.EndReceive(res, ref remoteIpEndPoint);

                var msg = Encoding.UTF8.GetString(received)
                    .Split(',').Select(long.Parse).ToArray();

                using (var scope = _scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    db.PulseData.Add(new PulseDataItem
                    {
                        //TODO from 1609459200
                        Time = DateTimeOffset.FromUnixTimeMilliseconds(msg[0]).DateTime.ToLocalTime(),
                        HeartBeat = msg[1],
                        Oxygen = msg[2],
                        IdentityUserId = _user?.Id
                    });

                    db.SaveChanges();
                }
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
    }
}