namespace profunion.Shared.Dto.Events
{
    public class UpdateEventDto
    {
        public string? title { get; set; }
        public string? description { get; set; }
        public string? organizer { get; set; }
        public DateTime? date { get; set; }
        public List<string>? imagesId { get; set; }
        public string? link { get; set; }
        public List<string>? categoriesId { get; set; }
        public int? places { get; set; }
        public bool? isActive { get; set; }
        public string? status { get; set; }
    }
}
