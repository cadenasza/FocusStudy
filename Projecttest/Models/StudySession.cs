using System;

namespace Projecttest.Models
{
    public class StudySession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int DurationMinutes { get; set; }
        public string? Subject { get; set; }
        public int? TargetMinutes { get; set; }
        public string? Badge { get; set; } // Foco, Estudo, Exercícios, Videoaulas, Pesquisas, Documentações
    }
}
