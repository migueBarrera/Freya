using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Core.Clients
{
    public class ClientDetailResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public DateTime? PregnancyDate { get; set; }

        public IEnumerable<Clinic> Clinics { get; set; } = Enumerable.Empty<Clinic>();

        public long Size { get; set; }

        public long SubscriptionPlanSize { get; set; }

        public IEnumerable<VideoMultimedia> Videos { get; set; } = new List<VideoMultimedia>();

        public IEnumerable<ImageMultimedia> Ultrasounds { get; set; } = new List<ImageMultimedia>();

        public IEnumerable<SoundMultimedia> Sounds { get; set; } = new List<SoundMultimedia>();

        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
