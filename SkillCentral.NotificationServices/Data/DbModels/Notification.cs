using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillCentral.NotificationServices.Data.DbModels;

[Table(name: "tblNotifications")]
public class Notification
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Message { get; set; }
    public bool? IsCompleted { get; set; }
    public string UserId { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsSupport { get; set; }


    public bool IsActive { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string? CreatedUserId { get; set; }
    public string? UpdatedUserId { get; set; }

}
