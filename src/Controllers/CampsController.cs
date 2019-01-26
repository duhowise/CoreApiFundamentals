using System;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;

        public CampsController(ICampRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CampModel[]>> Get(bool includeTalks=false)
        {
            try
            {
                var results = await _repository.GetAllCampsAsync(includeTalks);

                return _mapper.Map<CampModel[]>(results);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }



        [HttpGet("{moniker}")]
        public async Task<ActionResult<CampModel>> Get(string moniker)
        {
            try
            {
                var results = await _repository.GetCampAsync(moniker);
                if (results == null) return NotFound();

                return _mapper.Map<CampModel>(results);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

       [HttpGet("search")] public async Task<ActionResult<CampModel[]>> SearchByDate(DateTime date,bool includeTalks=false)
       {
           try
           {
               var results = await _repository.GetAllCampsByEventDate(date, includeTalks);
               if (!results.Any())
               {
                   return NotFound();
               }

               return _mapper.Map<CampModel[]>(results);
           }
           catch (Exception e)
           {
               return BadRequest(e);
           }
       }
    }
}
