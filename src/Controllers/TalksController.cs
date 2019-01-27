using System;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace CoreCodeCamp.Controllers
{
    [Route("api/camps/{moniker}/[controller]")]
    [ApiController]
    public class TalksController : ControllerBase
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public TalksController(ICampRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<TalkModel[]>> Get(string moniker)
        {
            try
            {
                var talks = await _repository.GetTalksByMonikerAsync(moniker, true);
                return _mapper.Map<TalkModel[]>(talks);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<TalkModel>> Get(string moniker, int id)
        {
            try
            {
                var talks = await _repository.GetTalkByMonikerAsync(moniker, id, true);
                if (talks == null) return NotFound("could not find talk");
               
                return _mapper.Map<TalkModel>(talks);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpPost]
        public async Task<ActionResult<TalkModel>> Post(string moniker, TalkModel talkModel)
        {
            try
            {
                var camp = await _repository.GetCampAsync(moniker);
                if (camp == null) return BadRequest("no camp exists for provided moniker");
                var talk = _mapper.Map<Talk>(talkModel);
                talk.Camp = camp;

                if (talkModel.Speaker == null) return BadRequest("Speaker Id is required");
                var speaker = await _repository.GetSpeakerAsync(talkModel.Speaker.SpeakerId);
                if (speaker == null) return BadRequest("Speaker could not be found");

                talk.Speaker = speaker;

                _repository.Add(talk);
                if (await _repository.SaveChangesAsync())
                {
                    var link = _linkGenerator.GetPathByAction(HttpContext, "Get",
                        values: new {moniker, id = talk.TalkId});

                    return Created(link, _mapper.Map<TalkModel>(talk));
                }

                return BadRequest("Failed to add talk to camp");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TalkModel>> Put(string moniker, int id, TalkModel model)
        {
            try
            {
                var talk = await _repository.GetTalkByMonikerAsync(moniker, id, true);
                if (talk == null) return BadRequest("could not find talk ");
                _mapper.Map(model, talk);

                if (model.Speaker != null)
                {
                    var speaker = await _repository.GetSpeakerAsync(model.Speaker.SpeakerId);
                    if (speaker != null)
                    {
                        talk.Speaker = speaker;
                    }
                }


                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<TalkModel>(talk);
                }

                return BadRequest("failed to update database");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(string moniker,int id)
        {
            try
            {
                var talk = await _repository.GetTalkByMonikerAsync(moniker, id);
                if (talk == null) return BadRequest("could not find talk for delete");
                _repository.Delete(talk);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }

                return BadRequest("failed to delete talk");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
