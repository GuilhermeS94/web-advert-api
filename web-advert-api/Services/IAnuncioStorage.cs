using System;
using System.Threading.Tasks;
using advertapi_models;

namespace web_advert_api.Services
{
    public interface IAnuncioStorage
    {
        Task<string> Adicionar(AdvertModel anuncio);
        Task Corfirmar(ConfirmarAnuncio confirmar);
    }
}
