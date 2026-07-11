using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ShiftMaster.AttendanceService.Enums;
using ShiftMaster.AttendanceService.Models;
using ShiftMaster.AttendanceService.Repositories;
using ShiftMaster.AttendanceService.Clients;
using ShiftMaster.AttendanceService.DTOs;
using ShiftMaster.AttendanceService.Exceptions;

namespace ShiftMaster.AttendanceService.Services
{
    public class OvertimeService : IOvertimeService
    {
        private readonly IOvertimeRepository _repository;
        private readonly IIdentityClient _identityClient;
        private readonly IMapper _mapper;

        public OvertimeService(IOvertimeRepository repository, IIdentityClient identityClient, IMapper _mapper)
        {
            _repository = repository;
            _identityClient = identityClient;
            this._mapper = _mapper;
        }

        public async Task<List<OvertimeAuthorisationDto>> GetPendingOvertimeAsync(int locationId)
        {
            var userIds = await _identityClient.GetUserIdsByLocationAsync(locationId);
            if (userIds == null || userIds.Count == 0)
            {
                return new List<OvertimeAuthorisationDto>();
            }

            var pendingRecords = await _repository.GetPendingOvertimeByUserIdsAsync(userIds);
            var namesDict = await _identityClient.LookupUserNamesAsync(userIds);

            return pendingRecords.Select(oa => new OvertimeAuthorisationDto
            {
                OTID = oa.OTID,
                UserID = oa.UserID,
                EmployeeName = namesDict.TryGetValue(oa.UserID, out var name) ? name : "Unknown Employee",
                WeekStartDate = oa.WeekStartDate,
                PlannedOTHours = oa.PlannedOTHours,
                ActualOTHours = oa.ActualOTHours,
                OTType = oa.OTType.ToString(),
                Status = oa.Status.ToString()
            }).ToList();
        }

        public async Task<OvertimeAuthorisationResponseDto> LogOvertimeAsync(CreateOvertimeDto dto)
        {
            var overtimeAuthorisation = _mapper.Map<OvertimeAuthorisation>(dto);

            await _repository.AddOvertimeAsync(overtimeAuthorisation);

            return _mapper.Map<OvertimeAuthorisationResponseDto>(overtimeAuthorisation);
        }

        public async Task<OvertimeAuthorisationResponseDto> AuthoriseOvertimeAsync(int otId, int authorisedById, bool approved)
        {
            var overtimeAuthorisation = await _repository.GetOvertimeByIdAsync(otId);

            if (overtimeAuthorisation == null)
            {
                throw new ResourceNotFoundException($"Overtime authorisation with ID {otId} not found.");
            }

            overtimeAuthorisation.AuthorisedByID = authorisedById;
            overtimeAuthorisation.Status = approved ? ApprovalStatus.Approved : ApprovalStatus.Rejected;

            await _repository.SaveChangesAsync();

            return _mapper.Map<OvertimeAuthorisationResponseDto>(overtimeAuthorisation);
        }
    }
}
