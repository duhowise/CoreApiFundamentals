using System;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Internal;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public CampsController(ICampRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<CampModel[]>> Get(bool includeTalks = false)
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

        [HttpGet("search")]
        public async Task<ActionResult<CampModel[]>> SearchByDate(DateTime date, bool includeTalks = false)
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

        [HttpPost]
        public async Task<ActionResult<CampModel>> Post(CampModel model)
        {
            try
            {
                var existing = await _repository.GetCampAsync(model.Moniker);
                if (existing != null)
                {
                    return BadRequest($"moniker {model.Moniker} already exists");
                }

                var location = _linkGenerator.GetPathByAction("Get", "Camps", new {moniker = model.Moniker});
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("could not use current moniker");
                }

                var camp = _mapper.Map<Camp>(model);
                _repository.Add(camp);
                await _repository.SaveChangesAsync();
                return Created(location, _mapper.Map<CampModel>(camp));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut("{moniker}")]
        public async Task<ActionResult<CampModel>> Put(string moniker, CampModel model)
        {
            try
            {
                var oldcamp = await _repository.GetCampAsync(moniker);
                if (oldcamp == null) return NotFound("could not locate provided moniker");

                _mapper.Map(model, oldcamp);

                await _repository.SaveChangesAsync();

                return _mapper.Map<CampModel>(oldcamp);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }



      [HttpDelete("{moniker}")]  public async Task<IActionResult> Delete(string moniker)
      {
          try
          {
              var oldcamp = await _repository.GetCampAsync(moniker);
              if (oldcamp == null) return NotFound("could not locate provided moniker");

               _repository.Delete(oldcamp);
              await _repository.SaveChangesAsync();

              return Ok();
            }
          catch (Exception e)
          {
              return BadRequest(e);
          }
      }
    }
}
