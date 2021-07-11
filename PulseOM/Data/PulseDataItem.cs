using System;
using System.Diagnostics.CodeAnalysis;

namespace PulseOM.Data
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
    public class PulseDataItem
    {
        public PulseDataItem()
        {
            PulseDataItemId = Guid.NewGuid().ToString();
        }

        public string PulseDataItemId { get; set; }
        public DateTime Time { get; set; }
        public long HeartBeat { get; set; }

        public long Oxygen { get; set; }

        // public IdentityUser? IdentityUser { get; set; }
        public string? IdentityUserId { get; set; }
    }
}