using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechShop_API_backend_.Data;
using TechShop_API_backend_.Models;

namespace TechShop_API_backend_.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationRepository _repository;

        public NotificationController(NotificationRepository repository)
        {
            _repository = repository;
        }

        // POST: api/notifications/send
        [Authorize(Roles = "Admin")]
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] Notification notification)
        {
            if (notification == null || string.IsNullOrEmpty(notification.Username))
                return BadRequest("Invalid notification data");
            notification.Id = null!;
            notification.IsRead = false;
            await _repository.CreateAsync(notification);
            return Ok(new { message = "Notification created successfully", notificationId = notification.Id });
        }

        // GET: api/notifications/unread/{username}
        [HttpGet("unread/{username}")]
        public async Task<IActionResult> GetUnreadNotifications(string username)
        {
            var currentUsername = User.FindFirst(ClaimTypes.Name)?.Value;
            if (currentUsername != username && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var notifications = await _repository.GetUnreadAsync(username);
            return Ok(notifications);
        }

        // GET: api/notifications/all/{username}
        [HttpGet("all/{username}")]
        public async Task<IActionResult> GetAllNotifications(string username)
        {
            var currentUsername = User.FindFirst(ClaimTypes.Name)?.Value;
            if (currentUsername != username && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var notifications = await _repository.GetAllByUserAsync(username);
            return Ok(notifications);
        }

        // POST: api/notifications/read/{id}
        [HttpPost("read/{id}")]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            await _repository.MarkAsReadAsync(id);
            return Ok(new { message = "Notification marked as read" });
        }

        // POST: api/notifications/readall/{username}
        [HttpPost("readall/{username}")]
        public async Task<IActionResult> MarkAllAsRead(string username)
        {
            var currentUsername = User.FindFirst(ClaimTypes.Name)?.Value;
            if (currentUsername != username && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            await _repository.MarkAllAsReadAsync(username);
            return Ok(new { message = "All notifications marked as read" });
        }
    }
}
