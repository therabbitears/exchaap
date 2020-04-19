using System;
namespace loffers.api.Data
{
    public class SnapshotModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public long? LastLat { get; set; }
        public long? LastLong { get; set; }
    }
}