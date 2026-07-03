using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;
using Services.Interfaces;
using Domain.Repositories;
using shiftmaster.models;

namespace Services.Implementation
{

    public class LeaveBlockService : ILeaveBlockService
    {
        private readonly ILeaveBlockRepository repository;

        public LeaveBlockService(ILeaveBlockRepository repository)
        {
            this.repository = repository;
        }

        public async Task<LeaveBlock> AddLeaveBlockAsync(LeaveBlock leave)
        {
            if (leave == null)
                return null;

            // ✅ Business validation
            if (leave.EndDate < leave.StartDate)
                throw new ArgumentException("End date cannot be earlier than start date");

            // ✅ Business rules
            leave.Status = LeaveStatus.Active;
            leave.ApprovedByID = null;

            await repository.AddAsync(leave);
            await repository.SaveAsync();

            return leave;
        }

        public async Task<bool> UpdateLeaveStatusAsync(int id, LeaveStatus status, int approvedBy)
        {
            var leave = await repository.GetByIdAsync(id);

            if (leave == null)
                return false;

            if (leave.Status == status)
                throw new Exception("Status is already set");

            leave.Status = status;

            if (status == LeaveStatus.Active)
            {
                if (leave.ApprovedByID != null)
                    throw new Exception("Leave already approved");

                leave.ApprovedByID = approvedBy;
            }

            await repository.SaveAsync();
            return true;
        }
    }

}
