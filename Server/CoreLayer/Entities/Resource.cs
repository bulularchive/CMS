using CoreLayer.Enums;

namespace CoreLayer.Entities
{
    public class Resource
    {
        public Guid ResourceId { get; set; }
        public ResourceType ResourceType { get; set; }
        public Guid CourseId { get; set; }
        public string? FileURL { get; set; }
        public string? Content { get; set; }
        public DateTime? OpenTime { get; set; }
        public DateTime? CloseTime { get; set; }
        public Status Status { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime? Since { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}