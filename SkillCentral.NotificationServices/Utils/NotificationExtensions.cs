using AutoMapper;
using SkillCentral.Dtos;
using SkillCentral.NotificationServices.Data.DbModels;

namespace SkillCentral.NotificationServices.Utils
{
    public static class NotificationExtensions
    {
        public static async Task<IEnumerable<NotificationDto>> BindDtoList(this IEnumerable<Notification> list, IMapper mapper, Func<string, Task<EmployeeDto>> getEmployee)
        {
            var dtoList = new List<NotificationDto>();

            foreach (var item in list)
            {
                var dto = mapper.Map<NotificationDto>(item);
                dto.Employee = await getEmployee(item.UserId);
                dtoList.Add(dto);
            }

            return dtoList;
        }
    }
}
