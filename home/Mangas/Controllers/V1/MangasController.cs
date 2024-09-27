using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mangas.Services.Freatures.manga;
using Mangas.Domain.Entities;
using Mangas.Domain.Entities.Dtos;
using AutoMapper;

namespace mangas.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    public class MangaController : ControllerBase
    {
        private readonly MangaService _mangaService;

        private readonly IMapper _mapper;

        public MangaController(MangaService mangaService, IMapper mapper)
        {
            this._mangaService = mangaService;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var mangas = _mangaService.GetAll();
            var MangaDTO = _mapper.Map<IEnumerable<MangaDTO>>(mangas);
            /*return Ok(_mangaService.GetAll());*/

            return Ok(MangaDTO);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var manga = _mangaService.GetById(id);

            if (manga == null)
                return NotFound();

                var dto = _mapper.Map<MangaDTO>(manga);

            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Manga manga)
        {
            var entity = _mapper.Map<Manga>(manga);

            var mangas = _mangaService.GetAll();
            var mangaId = mangas.Count() + 1;

            entity.Id = mangaId;
            _mangaService.Add(entity);

            var dto = _mapper.Map<MangaDTO>(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id },dto);
            /*_mangaService.Add(manga);
            return CreatedAtAction(nameof(GetById), new { id = manga.Id }, manga);*/
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Manga manga)
        {
            if (id != manga.Id)
                return BadRequest();
            
            _mangaService.Update(manga);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _mangaService.Delete(id);
            return NoContent();
        }
    }
}
