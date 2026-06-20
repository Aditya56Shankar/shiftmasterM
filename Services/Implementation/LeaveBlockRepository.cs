using System;
using System.Collections.Generic;
using System.Text;
using Data.Context;
using Domain.Enums;
using Services.Interfaces;
using shiftmaster.models;

namespace Services.Implementation
{
    public class LeaveBlockRepository : ILeaveBlockRepository
    {
        private readonly ApplicationDbContext db;

        public LeaveBlockRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<LeaveBlock> AddLeaveBlockAsync(LeaveBlock leave)
        {

            if (leave == null) return null;

            if (leave.EndDate < leave.StartDate) return null;

            
                leave.Status = LeaveStatus.Active;
                leave.ApprovedByID = null;

                await db.LeaveBlocks.AddAsync(leave);
                await db.SaveChangesAsync();

                return leave;
            

        }
    }
}
