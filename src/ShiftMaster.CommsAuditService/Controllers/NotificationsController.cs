using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShiftMaster.CommsAuditService.DTOs;
using ShiftMaster.CommsAuditService.Services;

namespace ShiftMaster.CommsAuditService.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotificationsByUserId(int userId)
        {
            var list = await _service.GetNotificationsByUserIdAsync(userId);
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult<NotificationDto>> CreateNotification(CreateNotificationDto dto)
        {
            var result = await _service.CreateNotificationAsync(dto);
            return CreatedAtAction(nameof(GetNotificationsByUserId), new { userId = result.UserID }, result);
        }

        [HttpPut("{id}/read")]
        public async Task<ActionResult> MarkAsRead(int id)
        {
            var successful = await _service.MarkAsReadAsync(id);
            if (!successful) return NotFound(new { message = "Notification not found." });

            return NoContent();
        }

        [HttpPut("{id}/dismiss")]
        public async Task<ActionResult> DismissNotification(int id)
        {
            var successful = await _service.DismissNotificationAsync(id);
            if (!successful) return NotFound(new { message = "Notification not found." });

            return NoContent();
        }
    }
}
