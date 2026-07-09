# ShiftMaster DTO and Repository Inventory

This file documents the DTOs in `Services/DTOs` and the repositories in `Data/Repositories`.

## DTOs

### Identity, Auth, and Users

| File | Purpose |
|---|---|
| `LoginDto.cs` | Login credentials used by the authentication endpoint. |
| `RegisterDto.cs` | Registration payload for creating a new account. |
| `UserDto.cs` | General user profile/details response. |
| `CreateUserDto.cs` | Input model for creating a user. |
| `UpdateUserDto.cs` | Input model for updating user details. |
| `AdminUserDto.cs` | Admin-facing user details response. |
| `ActorDto.cs` | Lightweight user/actor identity model used in workflow actions and audit-style operations. |

### Roles, Departments, and Locations

| File | Purpose |
|---|---|
| `RoleDto.cs` | Role details response model. |
| `CreateRoleDto.cs` | Input model for creating a role. |
| `UpdateRoleDto.cs` | Input model for updating a role. |
| `DepartmentDto.cs` | Department details response model. |
| `CreateDepartmentDto.cs` | Input model for creating a department. |
| `UpdateDepartmentDto.cs` | Input model for updating a department. |
| `WorkLocationDto.cs` | Work-location details response model. |
| `CreateWorkLocationDto.cs` | Input model for creating a work location. |
| `UpdateWorkLocationDto.cs` | Input model for updating a work location. |

### Scheduling, Rosters, and Shift Planning

| File | Purpose |
|---|---|
| `CreateRosterDto.cs` | Input model for creating a weekly roster. |
| `RosterResponseDto.cs` | Response model for roster creation or retrieval. |
| `EmployeeRosterResponseDto.cs` | Employee-focused roster view. |
| `SupervisorRosterResponseDto.cs` | Supervisor-focused roster view. |
| `EmployeeScheduleDto.cs` | Employee schedule summary model. |
| `ShiftPatternDto.cs` | Shift pattern details response model. |
| `CreateShiftPatternDto.cs` | Input model for creating shift patterns. |
| `CreateAssignmentDto.cs` | Input model for assigning a shift. |
| `AssignmentResponseDto.cs` | Response model for a created shift assignment. |
| `UpdateAssignmentDto.cs` | Input model for updating an assignment. |
| `EmployeeFullDto.cs` | Expanded employee detail model used for roster lookups. |

### Attendance and Timesheets

| File | Purpose |
|---|---|
| `CreateAttendanceDto.cs` | Input model for recording attendance. |
| `AttendanceDtoResponse.cs` | Response model for recorded attendance. |
| `CreateTimesheetDto.cs` | Input model for creating a timesheet. |
| `TimesheetDtoResponse.cs` | Response model for timesheet creation or status changes. |
| `AuditLogDto.cs` | Audit log entry response model used by supervisor reporting. |

### Availability and Leave

| File | Purpose |
|---|---|
| `AvailabilityRequestDto.cs` | Input model for submitting availability. |
| `AvailabilityResponseDto.cs` | Response model for submitted availability. |
| `AvailabilityDto.cs` | General availability summary model. |
| `LeaveBlockRequestDto.cs` | Input model for requesting leave. |
| `LeaveBlockResponseDto.cs` | Response model for leave requests and approvals. |

### Swap, Cover, and Overtime Workflows

| File | Purpose |
|---|---|
| `CreateSwapRequestDto.cs` | Input model for requesting a shift swap. |
| `SwapRequestResponseDto.cs` | Response model for a swap request. |
| `RespondToSwapDto.cs` | Input model used by an employee responding to a swap request. |
| `ApproveSwapDto.cs` | Input model used by a supervisor approving a swap request. |
| `SwapEligibilityDto.cs` | Result model describing whether a swap target is eligible. |
| `CreateCoverAssignmentDto.cs` | Input model for creating a cover assignment. |
| `CoverAssignmentResponseDto.cs` | Response model for cover assignment operations. |
| `CoverEligibilityDto.cs` | Result model describing eligible cover candidates. |
| `CreateOvertimeDto.cs` | Input model for logging overtime. |
| `AuthoriseOvertimeDto.cs` | Input model for authorizing overtime. |
| `OvertimeAuthorisationDto.cs` | Alternate overtime authorization payload used by the service layer. |
| `OvertimeAuthorisationResponseDto.cs` | Response model for overtime authorization. |

### Skills and Requirements

| File | Purpose |
|---|---|
| `EmployeeSkillRequestDto.cs` | Input model for adding an employee skill. |
| `EmployeeSkillResponseDto.cs` | Response model for saved employee skills. |
| `EmployeeSkillDto.cs` | Employee skill summary model. |
| `CreateSkillRequirementDto.cs` | Input model for creating a skill requirement. |
| `UpdateSkillRequirementDto.cs` | Input model for updating a skill requirement. |
| `SkillRequirementDto.cs` | Skill requirement response model. |

### Notifications and Miscellaneous

| File | Purpose |
|---|---|
| `CreateNotificationDto.cs` | Input model for creating a notification. |
| `NotificationDto.cs` | Notification response/details model. |

## Repositories

### Core User, Auth, and Audit

| File | Responsibility |
|---|---|
| `AuthRepository.cs` | Authentication and login/register persistence operations. |
| `AuditRepository.cs` | Audit logging persistence and retrieval. |
| `AttendanceRepository.cs` | Attendance record persistence and queries. |
| `EmployeeRepository.cs` | Employee profile and employee-related queries. |

### Scheduling and Rosters

| File | Responsibility |
|---|---|
| `ShiftRepository.cs` | Shift assignment persistence and shift lookup operations. |
| `WeeklyRosterRepository.cs` | Weekly roster persistence and retrieval. |
| `ShiftPatternRepository.cs` | Shift pattern CRUD and lookup. |
| `StatusCheckRepository.cs` | Shared workflow/status validation checks. |
| `ViolationRepository.cs` | Stores or checks workflow rule violations. |

### Availability, Leave, and Overtime

| File | Responsibility |
|---|---|
| `AvailabilityRepository.cs` | Availability submission and lookup. |
| `LeaveBlockRepository.cs` | Leave-block persistence and retrieval. |
| `LeaveRepository.cs` | Leave-related querying and operational checks. |
| `OvertimeRepository.cs` | Overtime logging and authorization persistence. |

### Swap, Cover, and Notifications

| File | Responsibility |
|---|---|
| `ShiftSwapRepository.cs` | Swap-request persistence and eligibility checks. |
| `CoverAssignmentRepository.cs` | Cover assignment persistence and workflow lookups. |
| `NotificationRepository.cs` | Notification persistence and retrieval. |

### Roles, Departments, Skills, and Locations

| File | Responsibility |
|---|---|
| `RoleRepository.cs` | Role CRUD and role lookup. |
| `DepartmentRepository.cs` | Department CRUD and department lookup. |
| `WorkLocationRepository.cs` | Work-location CRUD and lookup. |
| `SkillRepository.cs` | Skill catalog storage and lookup. |
| `SkillRequirementRepository.cs` | Skill requirement rule storage and lookup. |
| `EmployeeSkillRepository.cs` | Employee-skill relationship persistence. |

## Notes

- Some DTOs are paired request/response models for the same workflow, such as `CreateCoverAssignmentDto.cs` and `CoverAssignmentResponseDto.cs`.
- `LeaveBlockRepository.cs` and `LeaveRepository.cs` appear to support different leave concepts or layers; `LeaveBlockRepository` is likely the transactional workflow repository, while `LeaveRepository` is likely a query/helper repository.
- `StatusCheckRepository.cs` and `ViolationRepository.cs` are cross-cutting repositories used by validation and workflow enforcement.
- Several DTOs are used directly by controller endpoints, while others are used deeper in the service layer for mapping and response shaping.