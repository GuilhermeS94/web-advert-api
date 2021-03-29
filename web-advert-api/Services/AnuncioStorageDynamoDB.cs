using System;
using System.Threading.Tasks;
using advertapi_models;
using AutoMapper;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace web_advert_api.Services
{
    public class AnuncioStorageDynamoDB : IAnuncioStorage
    {
        private readonly IMapper _mapper;

        public AnuncioStorageDynamoDB(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<string> Adicionar(AdvertModel anuncio)
        {
            AnuncioDBModel dbModel = _mapper.Map<AnuncioDBModel>(anuncio);
            dbModel.Id = new Guid().ToString();
            dbModel.CriadoEm = DateTime.UtcNow;
            dbModel.Status = StatusAnuncio.Pendente;

            using (AmazonDynamoDBClient client =  new AmazonDynamoDBClient())
            {
                using (DynamoDBContext ctx = new DynamoDBContext(client))
                {
                    await ctx.SaveAsync(dbModel);
                }
            }

            return dbModel.Id;
        }

        public async Task Corfirmar(ConfirmarAnuncio confirmar)
        {
            using (AmazonDynamoDBClient client = new AmazonDynamoDBClient())
            {
                using (DynamoDBContext ctx = new DynamoDBContext(client))
                {
                    AnuncioDBModel registro = await ctx.LoadAsync<AnuncioDBModel>(confirmar.Id);
                    if(registro == null)
                    {
                        throw new KeyNotFoundException($"O registro ID: {confirmar.Id} nao foi encontrado.");
                    }

                    if(confirmar.Status == StatusAnuncio.Ativo)
                    {
                        registro.Status = StatusAnuncio.Ativo;
                        await ctx.SaveAsync(registro);
                    }
                    else
                    {
                        await ctx.DeleteAsync(registro);
                    }
                }
            }
        }
    }
}
