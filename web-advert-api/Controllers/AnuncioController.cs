using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using advertapi_models;
using Microsoft.AspNetCore.Mvc;
using web_advert_api.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace web_advert_api.Controllers
{
    [ApiController]
    [Route("api/v1/anuncios")]
    public class AnuncioController : ControllerBase
    {
        private readonly IAnuncioStorage _storage;

        public AnuncioController(IAnuncioStorage storage)
        {
            _storage = storage;
        }

        [HttpPost]
        [Route("Criar")]
        [ProducesResponseType(400)]
        [ProducesResponseType(201, Type = typeof(CriarAnuncioResponse))]
        public async Task<IActionResult> Criar(AdvertModel anuncio)
        {
            string registroId = string.Empty;
            try
            {
                registroId = await _storage.Adicionar(anuncio);

            }
            catch (KeyNotFoundException knfex)
            {
                return new NotFoundResult();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return StatusCode(201, new CriarAnuncioResponse { Id = registroId });
        }

        [HttpPut]
        [Route("Confirmar")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Confirmar(ConfirmarAnuncio anuncio)
        {
            try
            {
                await _storage.Corfirmar(anuncio);

            }
            catch (KeyNotFoundException knfex)
            {
                return new NotFoundResult();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return new OkResult();
        }
    }
}
