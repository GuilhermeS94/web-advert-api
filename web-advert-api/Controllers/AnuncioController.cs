using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using advertapi_models;
using advertapi_models.Mensagens;
using Amazon.SimpleNotificationService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using web_advert_api.Services;


namespace web_advert_api.Controllers
{
    [ApiController]
    [Route("api/v1/anuncios")]
    public class AnuncioController : ControllerBase
    {
        private readonly IAnuncioStorage _storage;
        private readonly IConfiguration _configs;

        public AnuncioController(IAnuncioStorage storage, IConfiguration configs)
        {
            _storage = storage;
            _configs = configs;
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
                await DispararMensagemConfirmar(anuncio);

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

        private async Task DispararMensagemConfirmar(ConfirmarAnuncio anuncio)
        {
            AnuncioDBModel anuncioCompleto = await _storage.GetById(anuncio.Id);
            using (AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient())
            {
                AnuncioConfirmado msgObj = new AnuncioConfirmado
                {
                    Id = anuncio.Id,
                    Titulo = anuncioCompleto.Titulo
                };
                string msgJson = JsonSerializer.Serialize(msgObj);
                await snsClient.PublishAsync(_configs.GetValue<string>("MensageriaId"), msgJson);
            }
        }
    }
}
