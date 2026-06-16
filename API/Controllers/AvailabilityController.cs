using AutoMapper;
using Data.Context;
using Microsoft.AspNetCore.Mvc;
using Services.DTOs;
using Services.Implementation;
using Services.Interfaces;
using shiftmaster.models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController:ControllerBase
    {
        private readonly IAvailabilityRepository repository;
        private readonly IMapper mapper;

        public AvailabilityController(IMapper mapper, IAvailabilityRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Availablity([FromBody] AvailabilityRequestDto avail)
        {
            var res = mapper.Map<AvailabilitySubmission>(avail);
            await repository.AddAvailableAsync(res);
            return Ok(mapper.Map<AvailabilityResponseDto>(res));
        }
    }
}
