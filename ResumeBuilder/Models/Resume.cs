namespace ResumeBuilder.Models
{
    public class Resume
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;

        public string Experience { get; set; } = string.Empty; // plain text or JSON
        public string Education { get; set; } = string.Empty;
        public string Skills { get; set; } = string.Empty;

        public Guid UserId { get; set; }
        public User User { get; set; }
    }

}
