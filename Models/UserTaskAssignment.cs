using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace To_Do_List.Models
{
    public class UserExerciseAssignment
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required int TaskId { get; set; }
        [DataType(DataType.Date)]
        public DateTime AssignedDate { get; set; }

        public required IdentityUser User { get; set; }

    }
}
