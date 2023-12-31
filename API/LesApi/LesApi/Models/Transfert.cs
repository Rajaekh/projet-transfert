﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using LesApi.Services;

namespace LesApi.Models
{
    public class Transfert
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = String.Empty;
        [BsonElement("notified")]
        public bool Notified { get; set; }
        [BsonElement("typeTransfert")]
        public string TypeTransfert { get; set; }

        [BsonElement("idagent")]
        public string Idagent { get; set; }
        [BsonElement("frais")]
        public string Frais { get; set; }
        [BsonElement("motifsTransfert")]
        public string MotifsTransfert { get; set; }
        [BsonElement("otp")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "L'OTP doit être une chaîne de 4 chiffres.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "L'OTP doit contenir uniquement des chiffres.")]
        public string oTP { get; set; }
        [BsonElement("dataeTransfert")]
        public DateTime DataeTransfert { get; set; }
        [BsonElement("montant")]
        public double Montant { get; set; }
        [BsonElement("plafondmaximal")]
        public double PlafondMaximal { get; set; } = 2000;
        [BsonElement("PlafondAnnuel")]
        public double PlafondAnnuel { get; set; } = 20000;
        [BsonElement("Idbeneficiaire")]
        public string IdBeneficiaire { get; set; }

        [BsonElement("IdClient")]
        public string IdClient { get; set; }

    }
}
