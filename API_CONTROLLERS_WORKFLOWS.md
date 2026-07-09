# ShiftMaster API Controller Workflows

This document lists every controller in `API/Controllers`, the routes they expose, the expected role-based access, and the workflow each endpoint follows.

## `AttendanceController`

- **Base route:** `POST /api/attendance`
- **Authorization:** `FrontLine Employee`
- **Purpose:** Records a new attendance event and returns the saved attendance response.

### Workflow
1. Accept a `CreateAttendanceDto` in the request body.
2. Map the DTO to `AttendanceRecord`.
3. Call `IAttendanceService.CreateAttendanceAsync(...)`.
4. Map the saved entity to `AttendanceDtoResponse`.
5. Return `200 OK` with the response DTO.

### Endpoints
- `POST /api/attendance` — create attendance record.

## `AuditLogsController`

- **Base route:** `GET /api/auditlogs`
- **Authorization:** `Shift Supervisor`
- **Purpose:** Returns all audit logs.

### Workflow
1. Call `IAuditService.GetAllAuditLogsAsync()`.
2. Return `200 OK` with the log list.

### Endpoints
- `GET /api/auditlogs` — fetch all audit logs.

## `AvailabilityController`

- **Base route:** `api/availability`
- **Authorization:**
  - `FrontLine Employee` for create and read
  - `Shift Supervisor` for status updates
- **Purpose:** Lets employees submit availability and supervisors manage availability status.

### Workflow
#### `POST /api/availability`
1. Accept `AvailabilityRequestDto`.
2. Map to `AvailabilitySubmission`.
3. Call `IAvailabilityService.AddAvailableAsync(...)`.
4. Map result to `AvailabilityResponseDto`.
5. Return `200 OK`.

#### `PUT /api/availability/{id}?status=...`
1. Read availability `id` and `AvailabilityStatus` from query.
2. Call `IAvailabilityService.UpdateAvailabilityStatusAsync(id, status)`.
3. Return `404 Not Found` if no record exists.
4. Return `200 OK` with an update confirmation payload.

#### `GET /api/availability`
1. Read the current user ID from the `nameid` claim.
2. Call `IAvailabilityService.GetMyScheduleAsync(userId)`.
3. Return `401 Unauthorized` if the claim is missing.
4. Return `200 OK` with the user’s schedule/availability data.

### Endpoints
- `POST /api/availability` — submit availability.
- `PUT /api/availability/{id}` — update availability status.
- `GET /api/availability` — retrieve the current employee schedule/availability.

## `CoverAssignmentController`

- **Base route:** `api/covers`
- **Authorization:**
  - `Shift Supervisor` for eligibility lookup and assignment
  - `FrontLine Employee` for confirmation
- **Purpose:** Handles cover discovery, cover assignment, and employee confirmation.

### Workflow
#### `GET /api/covers/eligible?shiftAssignmentId=...`
1. Accept a shift assignment ID as query input.
2. Call `ICoverAssignmentService.GetEligibleCoversAsync(shiftAssignmentId)`.
3. Return `404 Not Found` if the assignment cannot be found.
4. Return `200 OK` with eligible cover options.

#### `POST /api/covers/assign`
1. Accept `CreateCoverAssignmentDto`.
2. Validate model state.
3. Call `ICoverAssignmentService.AssignCoverAsync(dto)`.
4. Return `404 Not Found` if the referenced resource is missing.
5. Return `201 Created` with the created cover assignment.

#### `POST /api/covers/{coverId}/confirm?actorUserId=...`
1. Validate that `coverId` and `actorUserId` are greater than zero.
2. Call `ICoverAssignmentService.ConfirmCoverAsync(coverId, actorUserId)`.
3. Return `404 Not Found` if the cover assignment does not exist.
4. Return `400 Bad Request` if the workflow state is invalid.
5. Return `200 OK` when the confirmation succeeds.

### Endpoints
- `GET /api/covers/eligible` — list eligible employees for a cover.
- `POST /api/covers/assign` — assign a cover.
- `POST /api/covers/{coverId}/confirm` — confirm a cover.

## `DepartmentsController`

- **Base route:** `api/departments`
- **Authorization:** `HR`
- **Purpose:** Full CRUD for departments.

### Workflow
#### `GET /api/departments`
1. Call `IDepartmentService.GetAllDepartmentsAsync()`.
2. Return `200 OK` with the department list.

#### `GET /api/departments/{id}`
1. Call `IDepartmentService.GetDepartmentByIdAsync(id)`.
2. Return `404 Not Found` if missing.
3. Return `200 OK` with the department.

#### `POST /api/departments`
1. Accept `CreateDepartmentDto`.
2. Call `IDepartmentService.CreateDepartmentAsync(newDepartment)`.
3. Return `201 Created` using `GetDepartmentById` as the location target.

#### `PUT /api/departments/{id}`
1. Accept `UpdateDepartmentDto`.
2. Call `IDepartmentService.UpdateDepartmentAsync(id, dto)`.
3. Return `404 Not Found` if no department exists.
4. Return `200 OK` with the updated department.

#### `DELETE /api/departments/{id}`
1. Call `IDepartmentService.DeleteDepartmentAsync(id)`.
2. Return `404 Not Found` if the department is missing.
3. Return `204 No Content` when deletion succeeds.
4. Return `400 Bad Request` if the service rejects deletion.

### Endpoints
- `GET /api/departments`
- `GET /api/departments/{id}`
- `POST /api/departments`
- `PUT /api/departments/{id}`
- `DELETE /api/departments/{id}`

## `EmployeeSkillController`

- **Base route:** `api/employeeskill`
- **Authorization:** `FrontLine Employee`
- **Purpose:** Lets employees add their skills.

### Workflow
1. Accept `EmployeeSkillRequestDto`.
2. Map to `EmployeeSkill`.
3. Call `IEmployeeSkillService.AddEmployeeSkillAsync(...)`.
4. Map the saved entity to `EmployeeSkillResponseDto`.
5. Return `200 OK`.

### Endpoints
- `POST /api/employeeskill` — add a skill to the current employee profile.

## `LeaveBlocksController`

- **Base route:** `api/leave`
- **Authorization:**
  - `FrontLine Employee` for creation
  - `Shift Supervisor` for approval/status updates
- **Purpose:** Handles leave block submission and supervisor approval.

### Workflow
#### `POST /api/leave`
1. Accept `LeaveBlockRequestDto`.
2. Validate that the body is not null.
3. Map to `LeaveBlock`.
4. Call `ILeaveBlockService.AddLeaveBlockAsync(...)`.
5. Map to `LeaveBlockResponseDto`.
6. Return `200 OK`.

#### `PUT /api/leave/{id}?status=...`
1. Read the leave block ID and `LeaveStatus` from query.
2. Read the approver user ID from the `NameIdentifier` claim.
3. Call `ILeaveBlockService.UpdateLeaveStatusAsync(id, status, approvedBy)`.
4. Return `401 Unauthorized` if the approver cannot be identified.
5. Return `404 Not Found` if the leave block does not exist.
6. Return `200 OK` with approval details when successful.

### Endpoints
- `POST /api/leave` — create a leave block.
- `PUT /api/leave/{id}` — update leave status.

## `NotificationsController`

- **Base route:** `api/notifications`
- **Authorization:** none declared in the controller
- **Purpose:** Supports creating, reading, and dismissing notifications.

### Workflow
#### `GET /api/notifications/{userId}`
1. Read the target user ID.
2. Call `INotificationService.GetNotificationsByUserIdAsync(userId)`.
3. Return `200 OK` with the user’s notifications.

#### `POST /api/notifications`
1. Accept `CreateNotificationDto`.
2. Call `INotificationService.CreateNotificationAsync(dto)`.
3. Return `201 Created` pointing to `GetNotificationsByUserId`.

#### `PUT /api/notifications/{id}/read`
1. Call `INotificationService.MarkAsReadAsync(id)`.
2. Return `404 Not Found` if the notification does not exist.
3. Return `204 No Content` when the update succeeds.

#### `PUT /api/notifications/{id}/dismiss`
1. Call `INotificationService.DismissNotificationAsync(id)`.
2. Return `404 Not Found` if the notification does not exist.
3. Return `204 No Content` when the dismissal succeeds.

### Endpoints
- `GET /api/notifications/{userId}` — list notifications for a user.
- `POST /api/notifications` — create a notification.
- `PUT /api/notifications/{id}/read` — mark as read.
- `PUT /api/notifications/{id}/dismiss` — dismiss notification.

## `OvertimeController`

- **Base route:** `api/overtime`
- **Authorization:**
  - `Shift Supervisor` for review/approval
  - `FrontLine Employee` for logging overtime
- **Purpose:** Lets employees log overtime and supervisors authorize it.

### Workflow
#### `GET /api/overtime/pending?locationId=...`
1. Read the location ID from query.
2. Call `IOvertimeService.GetPendingOvertimeAsync(locationId)`.
3. Return `200 OK` with the pending overtime list.

#### `POST /api/overtime/log`
1. Accept `CreateOvertimeDto`.
2. Validate model state.
3. Call `IOvertimeService.LogOvertimeAsync(dto)`.
4. Return `201 Created` with the created overtime record.

#### `PUT /api/overtime/{otId}/authorize`
1. Accept `AuthoriseOvertimeDto`.
2. Validate model state.
3. Call `IOvertimeService.AuthoriseOvertimeAsync(otId, dto.AuthorisedByID, dto.Approved)`.
4. Return `404 Not Found` if the overtime record is missing.
5. Return `200 OK` when authorization succeeds.

### Endpoints
- `GET /api/overtime/pending` — list pending overtime by location.
- `POST /api/overtime/log` — submit overtime.
- `PUT /api/overtime/{otId}/authorize` — authorize overtime.

## `RolesController`

- **Base route:** `api/roles`
- **Authorization:** `Admin`
- **Purpose:** Full CRUD for roles.

### Workflow
#### `GET /api/roles`
1. Call `IRoleService.GetAllRolesAsync()`.
2. Return `200 OK` with the role list.

#### `GET /api/roles/{id}`
1. Call `IRoleService.GetRoleByIdAsync(id)`.
2. Return `404 Not Found` if missing.
3. Return `200 OK` with the role.

#### `POST /api/roles`
1. Accept `CreateRoleDto`.
2. Call `IRoleService.CreateRoleAsync(newRole)`.
3. Return `201 Created` using `GetRoleById` as the location target.

#### `PUT /api/roles/{id}`
1. Accept `UpdateRoleDto`.
2. Call `IRoleService.UpdateRoleAsync(id, dto)`.
3. Return `404 Not Found` if no role exists.
4. Return `200 OK` with the updated role.

#### `DELETE /api/roles/{id}`
1. Call `IRoleService.DeleteRoleAsync(id)`.
2. Return `404 Not Found` if the role is missing.
3. Return `204 No Content` when deletion succeeds.
4. Return `400 Bad Request` if the service rejects deletion.

### Endpoints
- `GET /api/roles`
- `GET /api/roles/{id}`
- `POST /api/roles`
- `PUT /api/roles/{id}`
- `DELETE /api/roles/{id}`

## `RostersController`

- **Base route:** `api/rosters`
- **Authorization:**
  - `Shift Supervisor` for create and employee-detail lookup
  - `Shift Supervisor,Admin` for roster retrieval
  - `Supervisor` for status updates
- **Purpose:** Manages weekly roster creation, lookup, employee detail lookup, and status updates.

### Workflow
#### `POST /api/rosters`
1. Accept `CreateRosterDto`.
2. Map to `WeeklyRoster`.
3. Call `IWeeklyRosterService.AddAsync(...)`.
4. Map the result to `RosterResponseDto`.
5. Return `200 OK`.
6. Return `400 Bad Request` for invalid workflow state.
7. Return `404 Not Found` when referenced resources are missing.

#### `GET /api/rosters/{locationId}/{week}`
1. Parse `week` as a date.
2. Call `IWeeklyRosterService.GetRosterAsync(locationId, parsedDate)`.
3. Return `400 Bad Request` for invalid date format.
4. Return `404 Not Found` if there is no roster.
5. Return `200 OK` with the roster data.

#### `GET /api/rosters/{locationId}/employees/{date}`
1. Parse `date` as a date.
2. Call `IEmployeeService.GetEmployeesFullData(locationId, parsedDate)`.
3. Return `400 Bad Request` for invalid date format.
4. Return `404 Not Found` if no employees are found.
5. Return `200 OK` with employee detail data.

#### `PUT /api/rosters?id=...&action=...`
1. Read `id` and `action` from query parameters.
2. Read the current user ID from the `nameid` claim.
3. Call `IWeeklyRosterService.UpdateRosterStatusAsync(id, action, userId)`.
4. Return `401 Unauthorized` if the claim is missing.
5. Return `404 Not Found` if the roster does not exist.
6. Return `200 OK` with the update confirmation.

### Endpoints
- `POST /api/rosters` — create a roster.
- `GET /api/rosters/{locationId}/{week}` — get a roster for a week.
- `GET /api/rosters/{locationId}/employees/{date}` — get employee details for a location/date.
- `PUT /api/rosters` — update roster status.

## `ShiftAssignmentController`

- **Base route:** `api/shiftassignment`
- **Authorization:** `Shift Supervisor`
- **Purpose:** Assigns shifts with duplicate checks and post-save validation.

### Workflow
#### `POST /api/shiftassignment`
1. Accept `CreateAssignmentDto`.
2. Validate the model state.
3. Check for duplicate assignments using `IShiftRepository.ShiftExistsAsync(...)`.
4. Return `400 Bad Request` if a duplicate shift already exists.
5. Map DTO to `ShiftAssignment`.
6. Save the assignment through the repository.
7. Run `IRosterValidationService.ValidateAssignmentConstraintsAsync(...)`.
8. Reload the assignment with details.
9. Map to `AssignmentResponseDto` and return `200 OK`.
10. Return `400 Bad Request`, `404 Not Found`, or `500 Internal Server Error` for workflow, resource, or unexpected failures.

### Endpoints
- `POST /api/shiftassignment` — assign a shift.

## `ShiftPatternsController`

- **Base route:** `api/shiftpattern`
- **Authorization:** `Scheduling Admin`
- **Purpose:** Full CRUD for shift patterns.

### Workflow
#### `GET /api/shiftpattern`
1. Call `IShiftPatternService.GetAllPatternsAsync()`.
2. Return `200 OK` with all patterns.

#### `GET /api/shiftpattern/{id}`
1. Call `IShiftPatternService.GetPatternByIdAsync(id)`.
2. Return `404 Not Found` if missing.
3. Return `200 OK` with the pattern.

#### `POST /api/shiftpattern`
1. Accept `CreateShiftPatternDto`.
2. Call `IShiftPatternService.CreatePatternAsync(dto)`.
3. Return `201 Created` using `GetById` as the location target.

#### `PUT /api/shiftpattern/{id}`
1. Accept `CreateShiftPatternDto`.
2. Call `IShiftPatternService.UpdatePatternAsync(id, dto)`.
3. Return `404 Not Found` if missing.
4. Return `200 OK` with the updated pattern.

#### `DELETE /api/shiftpattern/{id}`
1. Call `IShiftPatternService.DeletePatternAsync(id)`.
2. Return `404 Not Found` if the pattern is missing.
3. Return `204 No Content` on success.
4. Return `400 Bad Request` if deletion is rejected.

### Endpoints
- `GET /api/shiftpattern`
- `GET /api/shiftpattern/{id}`
- `POST /api/shiftpattern`
- `PUT /api/shiftpattern/{id}`
- `DELETE /api/shiftpattern/{id}`

## `ShiftSwapController`

- **Base route:** `api/swaps`
- **Authorization:**
  - `FrontLine Employee` for swap discovery, request, and response
  - `Shift Supervisor` for approval
- **Purpose:** Manages swap targeting, swap requests, responses, and approval.

### Workflow
#### `GET /api/swaps/eligible-targets?shiftAssignmentId=...`
1. Read the shift assignment ID.
2. Call `IShiftSwapService.GetEligibleSwapTargetsAsync(shiftAssignmentId)`.
3. Return `200 OK` with eligible targets.

#### `POST /api/swaps/request`
1. Accept `CreateSwapRequestDto`.
2. Validate the model state.
3. Call `IShiftSwapService.CreateSwapRequestAsync(dto)`.
4. Return `201 Created` with the created swap request.

#### `PUT /api/swaps/{swapId}/respond`
1. Accept `RespondToSwapDto`.
2. Validate the model state.
3. Call `IShiftSwapService.RespondToSwapAsync(swapId, dto.Accepted)`.
4. Return `404 Not Found` if the swap request does not exist.
5. Return `400 Bad Request` if the workflow state is invalid.
6. Return `200 OK` with the updated swap request.

#### `PUT /api/swaps/{swapId}/approve`
1. Accept `ApproveSwapDto`.
2. Validate the model state.
3. Call `IShiftSwapService.ApproveSwapAsync(swapId, dto.ApprovedByID, dto.Approved)`.
4. Return `404 Not Found` if the swap ID is missing.
5. Return `400 Bad Request` if the workflow cannot move forward.
6. Return `200 OK` with the approved swap request.

### Endpoints
- `GET /api/swaps/eligible-targets` — list eligible swap targets.
- `POST /api/swaps/request` — create a swap request.
- `PUT /api/swaps/{swapId}/respond` — respond to a swap request.
- `PUT /api/swaps/{swapId}/approve` — approve a swap request.

## `SkillRequirementsController`

- **Base route:** `api/skillrequirements`
- **Authorization:** `Admin`
- **Purpose:** Full CRUD for skill requirements.

### Workflow
#### `GET /api/skillrequirements`
1. Call `ISkillRequirementService.GetAllRequirementsAsync()`.
2. Return `200 OK` with all requirements.

#### `GET /api/skillrequirements/{id}`
1. Call `ISkillRequirementService.GetRequirementByIdAsync(id)`.
2. Return `404 Not Found` if missing.
3. Return `200 OK` with the requirement.

#### `POST /api/skillrequirements`
1. Accept `CreateSkillRequirementDto`.
2. Call `ISkillRequirementService.CreateRequirementAsync(newReq)`.
3. Return `201 Created` using `GetRequirementById` as the location target.

#### `PUT /api/skillrequirements/{id}`
1. Accept `UpdateSkillRequirementDto`.
2. Call `ISkillRequirementService.UpdateRequirementAsync(id, dto)`.
3. Return `404 Not Found` if missing.
4. Return `200 OK` with the updated requirement.

#### `DELETE /api/skillrequirements/{id}`
1. Call `ISkillRequirementService.DeleteRequirementAsync(id)`.
2. Return `404 Not Found` if the requirement is missing.
3. Return `204 No Content` on success.

### Endpoints
- `GET /api/skillrequirements`
- `GET /api/skillrequirements/{id}`
- `POST /api/skillrequirements`
- `PUT /api/skillrequirements/{id}`
- `DELETE /api/skillrequirements/{id}`

## `TimesheetController`

- **Base route:** `api/timesheets`
- **Authorization:**
  - open for submission
  - `Payroll` for payroll routing
  - `HR` for approval
- **Purpose:** Creates timesheets and moves them through payroll/approval states.

### Workflow
#### `POST /api/timesheets`
1. Accept `CreateTimesheetDto`.
2. Call `IAttendanceService.CreateTimesheetAsync(dto.UserID, dto.WeekStartDate)`.
3. Map the result to `TimesheetDtoResponse`.
4. Return `200 OK` with the timesheet response.

#### `PUT /api/timesheets/{id}/payroll`
1. Read the current user ID from the `nameid` claim.
2. Call `IAttendanceService.UpdateTimesheetStatusAsync(id, TimesheetStatus.SentToPayroll, userId)`.
3. Return `404 Not Found` if the timesheet does not exist.
4. Return `400 Bad Request` if the status transition is invalid.
5. Return `200 OK` with the updated record.

#### `PUT /api/timesheets/{id}/approve`
1. Read the current user ID from the `nameid` claim.
2. Call `IAttendanceService.UpdateTimesheetStatusAsync(id, TimesheetStatus.Approved, userId)`.
3. Return `404 Not Found` if the timesheet does not exist.
4. Return `400 Bad Request` if the status transition is invalid.
5. Return `200 OK` with the updated record.

### Endpoints
- `POST /api/timesheets` — create a timesheet.
- `PUT /api/timesheets/{id}/payroll` — send timesheet to payroll.
- `PUT /api/timesheets/{id}/approve` — approve timesheet.

## `UsersController`

- **Base route:** `api/users`
- **Authorization:** `Scheduling Admin,Shift Supervisor` for user lookup
- **Purpose:** Handles registration, login, and admin-style user lookup.

### Workflow
#### `POST /api/users/register`
1. Read client IP address and user agent.
2. Call `IAuthService.RegisterAsync(dto)`.
3. Resolve the user ID with `IAuthService.GetUserIdByEmailAsync(dto.Email)`.
4. Write a registration audit log through `IAuditService.LogRegistrationAsync(...)`.
5. Return `200 OK` with a success message.
6. On failure, log the failed registration and return `400 Bad Request`.

#### `POST /api/users/login`
1. Validate model state.
2. Read client IP address and user agent.
3. Call `IAuthService.LoginAsync(dto)`.
4. Resolve the user ID with `IAuthService.GetUserIdByEmailAsync(dto.Email)`.
5. Write a login audit log through `IAuditService.LogLoginAttemptAsync(...)`.
6. Return `200 OK` with a JWT token.
7. On failure, log the failed attempt and return `400 Bad Request`.

#### `GET /api/users/{id}`
1. Call `IAuthService.GetAdminUserByIdAsync(id)`.
2. Return `404 Not Found` if the user does not exist.
3. Return `200 OK` with the user details.

### Endpoints
- `POST /api/users/register` — register a user.
- `POST /api/users/login` — authenticate a user.
- `GET /api/users/{id}` — fetch user details.

## `WorkLocationsController`

- **Base route:** `api/worklocations`
- **Authorization:** `Admin`
- **Purpose:** Full CRUD for work locations.

### Workflow
#### `GET /api/worklocations`
1. Call `IWorkLocationService.GetAllLocationsAsync()`.
2. Return `200 OK` with the list.

#### `GET /api/worklocations/{id}`
1. Call `IWorkLocationService.GetLocationByIdAsync(id)`.
2. Return `404 Not Found` if the location does not exist.
3. Return `200 OK` with the location.

#### `POST /api/worklocations`
1. Validate the incoming `CreateWorkLocationDto`.
2. Call `IWorkLocationService.CreateLocationAsync(newLocation)`.
3. Return `201 Created` using `GetById` as the location target.

#### `PUT /api/worklocations/{id}`
1. Accept `UpdateWorkLocationDto`.
2. Call `IWorkLocationService.UpdateLocationAsync(id, dto)`.
3. Return `404 Not Found` if the location is missing.
4. Return `200 OK` with the updated location.

#### `DELETE /api/worklocations/{id}`
1. Call `IWorkLocationService.DeleteLocationAsync(id)`.
2. Return `404 Not Found` if the location does not exist.
3. Return `204 No Content` on success.
4. Return `400 Bad Request` if the service rejects the deletion.

### Endpoints
- `GET /api/worklocations`
- `GET /api/worklocations/{id}`
- `POST /api/worklocations`
- `PUT /api/worklocations/{id}`
- `DELETE /api/worklocations/{id}`

## Coverage Summary

- **Controllers covered:** 17
- **Total endpoint groups documented:** all endpoints discovered in `API/Controllers`
- **Notes:** Routes and role names are documented exactly as defined in the controller attributes.