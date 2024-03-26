using AutoMapper;
using Clenka.Benelvis.BackendRsvp.Constants;
using Clenka.Benelvis.BackendRsvp.DTOs;
using Clenka.Benelvis.BackendRsvp.Models;
using Clenka.Benelvis.BackendRsvp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clenka.Benelvis.BackendRsvp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageRsvpsController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITableStorageService<RsvpEntity> _tableService;
        private readonly IMapper _mapper;

        public ManageRsvpsController(ILogger<HomeController> logger, ITableStorageService<RsvpEntity> tableService, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _tableService = tableService ?? throw new ArgumentNullException(nameof(tableService));
        }

        [HttpPost]
        public async Task<string> CreateRsvp(CreateRsvpEntityDto rsvpEntity)
        {
            _logger.LogInformation("Creating Rsvps");            

            var toCreate = _mapper.Map<RsvpEntity>(rsvpEntity);
            toCreate.PartitionKey = GlobalConstants.RSVPENTITYTABLEPARTITIONKEY;
            toCreate.RowKey = $"{toCreate.Lname}-{toCreate.Email}";
            toCreate.LastUpdated = DateTime.UtcNow;
            toCreate.Created = DateTime.UtcNow;

            var result = await _tableService.AddAsync(toCreate);
            if(result.Status != 204)
            {
                return "Error";
               // _logger.LogInformation("Created RSVPs");
            }

            return "Ok";
        }

       

    }
}
