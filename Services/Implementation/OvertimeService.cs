using AutoMapper;
using Domain.Enums;
using Services.DTOs;
using Services.Implementation.Exceptions;
using Services.Interfaces;
using Domain.Repositories;
using shiftmaster.models;

namespace Services.Implementation
{
	public class OvertimeService : IOvertimeService
	{
		private readonly IOvertimeRepository _repository;
		private readonly IMapper _mapper;

		public OvertimeService(IOvertimeRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<List<OvertimeAuthorisationDto>> GetPendingOvertimeAsync(int locationId)
		{
			var pendingRecords = await _repository.GetPendingOvertimeByLocationAsync(locationId);

			return pendingRecords.Select(oa => new OvertimeAuthorisationDto
			{
				OTID = oa.OTID,
				UserID = oa.UserID,
				EmployeeName = oa.Employee.Name,
				WeekStartDate = oa.WeekStartDate,
				PlannedOTHours = oa.PlannedOTHours,
				ActualOTHours = oa.ActualOTHours,
				OTType = oa.OTType,
				Status = oa.Status
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
