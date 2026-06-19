using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.DTOs;
using Services.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        // 1. GET /api/notifications/{userId} -> List alerts for a user
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotificationsByUserId(int userId)
        {
            var list = await _service.GetNotificationsByUserIdAsync(userId);
            return Ok(list);
        }

        // 2. POST /api/notifications -> Create notification
        [HttpPost]
        public async Task<ActionResult<NotificationDto>> CreateNotification(CreateNotificationDto dto)
        {
            var result = await _service.CreateNotificationAsync(dto);
            return CreatedAtAction(nameof(GetNotificationsByUserId), new { userId = result.UserID }, result);
        }

        // 3. PUT /api/notifications/{id}/read -> Mark as read
        [HttpPut("{id}/read")]
        public async Task<ActionResult> MarkAsRead(int id)
        {
            var successful = await _service.MarkAsReadAsync(id);
            if (!successful) return NotFound(new { message = "Notification not found." });

            return NoContent();
        }

        // 4. PUT /api/notifications/{id}/dismiss -> Dismiss notification
        [HttpPut("{id}/dismiss")]
        public async Task<ActionResult> DismissNotification(int id)
        {
            var successful = await _service.DismissNotificationAsync(id);
            if (!successful) return NotFound(new { message = "Notification not found." });

            return NoContent();
        }
    }
}