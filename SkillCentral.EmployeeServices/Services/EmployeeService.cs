﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ps.RabbitMq.Client;
using SkillCentral.Dtos;
using SkillCentral.EmployeeServices.Data.DbModels;
using SkillCentral.EmployeeServices.Utils;
using SkillCentral.Repository;

namespace SkillCentral.EmployeeServices.Services
{
    public class EmployeeService(IRepository repository, 
                                IMapper mapper, 
                                ILogger<EmployeeService> logger, 
                                IHttpContextAccessor context, 
                                IMqRequestService requestQueueService, 
                                IMqPubSubService pubSubQueueService) : ServiceBase(context), IEmployeeService
    {
        public async Task<EmployeeDto> GetAsync(string userId)
        {
            var emp = await repository.GetSingleAsync<Employee>(x => x.UserId.ToLower() == userId.ToLower());
            if (emp is not null)
            {
                return mapper.Map<EmployeeDto>(emp);
            }
            return null;
        }

        public async Task<List<EmployeeDto>> GetAsync()
        {
            var lst = await repository.GetListAsync<Employee>(x => x.IsActive);
            if (lst is not null && lst.Count() > 0)
            {
                return mapper.Map<List<EmployeeDto>>(lst);
            }
            return new List<EmployeeDto>();
        }

        public async Task<EmployeeDto> CreateAsync(EmployeeCreateDto employee, bool isBatch = false)
        {
            if (employee is null)
                throw new ArgumentNullException(GlobalConstants.EMPLOYEE_OBJ_NULL);

            var dbEmp = mapper.Map<Employee>(employee);
            dbEmp.DateCreated = DateTime.Now;
            dbEmp.CreatedUserId = GetLoginUserId();
            dbEmp = await repository.CreateAsync(dbEmp);

            //updating user id for new employee
            dbEmp.UserId = $"EMP{dbEmp.Id}";
            await repository.UpdateAsync(dbEmp);

            var dto = mapper.Map<EmployeeDto>(dbEmp);

            if(!isBatch)
            {
                //To User
                await SendNotification(dto.UserId, string.Format(EmployeeServiceConstants.EMPLOYEE_CREATED_USER, dto.UserId), MQConstants.EMPLOYEE_CREATE_ROUTE_KEY);
                //To Admin or support
                await SendNotification("admin", string.Format(EmployeeServiceConstants.EMPLOYEE_CREATED, dto.UserId), MQConstants.EMPLOYEE_CREATE_ROUTE_KEY, isUser: false);
            }
            return dto;
        }

        public async Task ProcessEmployeeBatchAsync(List<EmployeeDto> employees)
        {
            if(employees is null)
                throw new ArgumentNullException(GlobalConstants.EMPLOYEE_LIST_OBJ_NULL);

            int invalidCount = 0;
            int createdCount = 0;
            int updatedCount = 0;
            foreach (var emp in employees)
            {
                if (string.IsNullOrWhiteSpace(emp.UserId))
                    invalidCount++;
                else
                {
                    var dbEmp = await repository.GetSingleAsync<Employee>(x => x.UserId.ToLower() == emp.UserId.ToLower());
                    if (dbEmp is null)
                    {
                        EmployeeCreateDto createDto = mapper.Map<EmployeeCreateDto>(emp);
                        await CreateAsync(createDto, true);
                        createdCount++;
                    }
                    else
                    {
                        await UpdateAsync(emp, true);
                        updatedCount++;
                    }
                }
            }

            if(invalidCount > 0)
            {
                await SendNotification("admin", string.Format(EmployeeServiceConstants.EMPLOYEE_BATCH_INVALID_ROWS, invalidCount), MQConstants.EMPLOYEE_BATCH_ROUTE_KEY, isUser: false);
            }
            if (createdCount > 0)
            {
                await SendNotification("admin", string.Format(EmployeeServiceConstants.EMPLOYEE_BATCH_CREATED_ROWS, createdCount), MQConstants.EMPLOYEE_BATCH_ROUTE_KEY, isUser: false);
            }
            if (updatedCount > 0)
            {
                await SendNotification("admin", string.Format(EmployeeServiceConstants.EMPLOYEE_BATCH_UPDATED_ROWS, updatedCount), MQConstants.EMPLOYEE_BATCH_ROUTE_KEY, isUser: false);
            }
        }
        public async Task<EmployeeDto> UpdateAsync(EmployeeDto updatedEmployee, bool isBatch = false)
        {

            if (updatedEmployee is null)
                throw new ArgumentNullException(GlobalConstants.EMPLOYEE_OBJ_NULL);

            var emp = await repository.GetSingleAsync<Employee>(x => x.UserId.ToLower() == updatedEmployee.UserId.ToLower());

            emp.DateUpdated = DateTime.UtcNow;
            emp.UpdatedUserId = GetLoginUserId();
            emp.MergerEmployee(updatedEmployee);

            int count = await repository.UpdateAsync(emp);

            if (count > 0)
            {
                var dto = await GetAsync(updatedEmployee.UserId);

                if(!isBatch)
                {
                    //To User
                    await SendNotification(dto.UserId, EmployeeServiceConstants.EMPLOYEE_UPDATED_USER, MQConstants.EMPLOYEE_UPDATED_ROUTE_KEY);
                    //To Admin or support
                    await SendNotification("admin", string.Format(EmployeeServiceConstants.EMPLOYEE_UPDATED, dto.UserId), MQConstants.EMPLOYEE_UPDATED_ROUTE_KEY, isUser: false);
                }
                return dto;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(string userId, bool isBatch = false)
        {
            if (userId is null)
                throw new ArgumentNullException(GlobalConstants.USER_ID_NULL);

            var dbEmp = await repository.GetSingleAsync<Employee>(s => s.UserId.ToLower() == userId.ToLower() && s.IsActive);
            dbEmp.IsActive = false;
            dbEmp.DateUpdated = DateTime.Now;
            dbEmp.UpdatedUserId = GetLoginUserId();
            int count = await repository.UpdateAsync(dbEmp);
            var isSuccess = count > 0;

            var empDto = mapper.Map<EmployeeDto>(dbEmp);

            if (isSuccess && !isBatch)
            {
                await pubSubQueueService.PublishTopicAsync(empDto, MQConstants.EMPLOYEE_DELETE_ROUTE_KEY);
                await SendNotification("admin", string.Format(EmployeeServiceConstants.EMPLOYEE_DELETED, empDto.UserId), MQConstants.EMPLOYEE_DELETE_ROUTE_KEY, isUser: false);
            }

            return isSuccess;
        }

        #region PrivateMethods
        private async Task SendNotification(string userId, string notification, string routeKey, bool isUser = true)
        {
            NotificationCreateDto notificationDto = new NotificationCreateDto();
            notificationDto.UserId = userId;
            notificationDto.Notification = notification;

            if(!isUser)
            {
                notificationDto.IsAdmin = true;
                notificationDto.IsSupport = true;
            }
            else
            {
                notificationDto.IsAdmin = false;
                notificationDto.IsSupport = false;
            }
            
            await pubSubQueueService.PublishTopicAsync(notificationDto, routeKey);
        }
        #endregion
    }
}
