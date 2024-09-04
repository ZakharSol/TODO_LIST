using To_Do_List.Enums;

namespace To_Do_List.Models
{

    public class Exercise

    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public ExerciseStatus Status { get; set; }
    }
}
