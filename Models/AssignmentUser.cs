using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace To_Do_List.Models
{
    public class AssignmentUser
    {
        public int Id { get; set; }

        public required string UserId { get; set; } // Внешний ключ на пользователя
        public required int DepartmentId { get; set; } // Внешний ключ на отдел
        public required int VacancyId { get; set; } // Внешний ключ на вакансию
        [DataType(DataType.Date)]
        public required DateTime AssignedDate { get; set; } // Дата назначения
        [DataType(DataType.Date)]
        public DateTime? MovedDate { get; set; } // Дата перемещения (может быть null)

        // Навигационные свойства
        public required IdentityUser User { get; set; }
        public required Department Department { get; set; }
        public required Vacancy Vacancy { get; set; }
    }
}
