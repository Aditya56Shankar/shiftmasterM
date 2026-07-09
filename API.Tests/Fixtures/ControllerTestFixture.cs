using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace shiftMaster.API.Tests.Fixtures
{
    /// <summary>
    /// Utility class for creating test fixtures and test data
    /// </summary>
    public static class ControllerTestFixture
    {
        /// <summary>
        /// Creates a ClaimsPrincipal with a NameIdentifier claim for testing
        /// </summary>
        public static ClaimsPrincipal CreateTestUser(int userId, string? role = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            if (!string.IsNullOrEmpty(role))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            return new ClaimsPrincipal(identity);
        }

        /// <summary>
        /// Sets up a ControllerContext with authentication
        /// </summary>
        public static void SetupControllerContext(ControllerBase controller, int userId, string? role = null)
        {
            var user = CreateTestUser(userId, role);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        /// <summary>
        /// Verifies that a response is an OkObjectResult
        /// </summary>
        public static OkObjectResult AssertOkResult(IActionResult result)
        {
            var okResult = result as OkObjectResult;
            if (okResult == null)
                throw new InvalidOperationException($"Expected OkObjectResult but got {result?.GetType().Name}");
            return okResult;
        }

        /// <summary>
        /// Verifies that a response is a BadRequestObjectResult
        /// </summary>
        public static BadRequestObjectResult AssertBadRequestResult(IActionResult result)
        {
            var badResult = result as BadRequestObjectResult;
            if (badResult == null)
                throw new InvalidOperationException($"Expected BadRequestObjectResult but got {result?.GetType().Name}");
            return badResult;
        }

        /// <summary>
        /// Verifies that a response is a NotFoundObjectResult
        /// </summary>
        public static NotFoundObjectResult AssertNotFoundResult(IActionResult result)
        {
            var notFoundResult = result as NotFoundObjectResult;
            if (notFoundResult == null)
                throw new InvalidOperationException($"Expected NotFoundObjectResult but got {result?.GetType().Name}");
            return notFoundResult;
        }

        /// <summary>
        /// Verifies that a response is an UnauthorizedResult
        /// </summary>
        public static UnauthorizedResult AssertUnauthorizedResult(IActionResult result)
        {
            var unauthorizedResult = result as UnauthorizedResult;
            if (unauthorizedResult == null)
                throw new InvalidOperationException($"Expected UnauthorizedResult but got {result?.GetType().Name}");
            return unauthorizedResult;
        }

        /// <summary>
        /// Verifies that a response is an ObjectResult with a specific status code
        /// </summary>
        public static ObjectResult AssertObjectResultWithStatus(IActionResult result, int expectedStatusCode)
        {
            var objectResult = result as ObjectResult;
            if (objectResult == null)
                throw new InvalidOperationException($"Expected ObjectResult but got {result?.GetType().Name}");
            if (objectResult.StatusCode != expectedStatusCode)
                throw new InvalidOperationException($"Expected status code {expectedStatusCode} but got {objectResult.StatusCode}");
            return objectResult;
        }
    }
}
