﻿using AutoMapper;
using Google.Protobuf;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SkillCentral.Dtos;
using SkillCentral.Repository;
using SkillCentral.ServiceDefaults;
using SkillCentral.SkillServices.Data.DbModels;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace SkillCentral.SkillServices.Services
{
    public class EmployeeSkillService(IRepository repository, IMapper mapper, ILogger<EmployeeSkillService> logger, IMQService queueService, ISkillService skillService, IHttpContextAccessor context) : ServiceBase(context), IEmployeeSkillService
    {
        public async Task<EmployeeSkillDto> CreateAsync(EmployeeSkillCreateDto skill)
        {
            if (skill is null)
                throw new ArgumentNullException("employee skill input object cannot be null");
            
            var dbEmpSkill = mapper.Map<EmployeeSkill>(skill);
            dbEmpSkill.DateCreated = DateTime.Now;
            dbEmpSkill.CreatedUserId = GetLoginUserId();
            dbEmpSkill = await repository.CreateAsync(dbEmpSkill);
            var dto = await BindEmployeeSkillData(dbEmpSkill);
            return dto;
        }

        public async Task<List<EmployeeSkillDto>> GetAsync(string userId)
        {
            if (userId is null)
                throw new ArgumentNullException("userid cannot be null");

            var data = (await repository.GetListAsync<EmployeeSkill>(x => x.UserId.ToLower() == userId.ToLower()))?.ToList();
            if (data is not null && data.Count > 0)
            {
                var empDtoList = new List<EmployeeSkillDto>();
                foreach(var item in data)
                {
                    var dto = await BindEmployeeSkillData(item);
                    empDtoList.Add(dto);
                }
                return empDtoList;
            }
            return new List<EmployeeSkillDto>();
        }

        public async Task<bool> RemoveSkill(string userId, int skillId)
        {
            if (userId is null || skillId == 0)
                throw new ArgumentNullException("userid cannot be null");

            var record = await repository.GetSingleAsync<EmployeeSkill>(x => x.UserId.ToLower() == userId.ToLower() && x.SkillId == skillId);
            record.IsActive = false;
            record.DateUpdated = DateTime.UtcNow;
            record.UpdatedUserId = GetLoginUserId();
            int count = await repository.UpdateAsync(record);
            return count > 0;
        }


        #region PrivateMethods
        private async Task<EmployeeSkillDto> BindEmployeeSkillData(EmployeeSkill item)
        {
            EmployeeSkillDto dto = mapper.Map<EmployeeSkillDto>(item);
            dto.Employee = queueService.GetResponse<string, EmployeeDto>(item.UserId);
            dto.Skill = await skillService.GetAsync(item.SkillId);
            return dto;
        }
        #endregion
    }
}
