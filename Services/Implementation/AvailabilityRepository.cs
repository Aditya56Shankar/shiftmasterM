using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Data.Context;
using Services.Interfaces;
using shiftmaster.models;

namespace Services.Implementation
{
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private readonly ApplicationDbContext db;
        public AvailabilityRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<AvailabilitySubmission> AddAvailableAsync(AvailabilitySubmission avail)
        {
            await db.AvailabilitySubmissions.AddAsync(avail);
            await db.SaveChangesAsync();
            return avail;
        }
    }
}
