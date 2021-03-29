using System;
using advertapi_models;
using AutoMapper;
namespace web_advert_api.Services
{
    public class AnuncioPerfil : Profile
    {
        public AnuncioPerfil()
        {
            CreateMap<AdvertModel, AnuncioDBModel>();
        }
    }
}
