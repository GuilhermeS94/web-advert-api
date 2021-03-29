using System;
using advertapi_models;
using Amazon.DynamoDBv2.DataModel;

namespace web_advert_api.Services
{
    [DynamoDBTable("Anuncios")]
    public class AnuncioDBModel
    {
        public AnuncioDBModel()
        {
        }

        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty]
        public string Titulo { get; set; }

        [DynamoDBProperty]
        public string Descricao { get; set; }

        [DynamoDBProperty]
        public double Preco { get; set; }

        [DynamoDBProperty]
        public DateTime CriadoEm { get; set; }

        [DynamoDBProperty]
        public StatusAnuncio Status { get; set; }
    }
}
