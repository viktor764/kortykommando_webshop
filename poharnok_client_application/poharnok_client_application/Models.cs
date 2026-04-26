using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace poharnok_client_application
{
    public class OrderRoot
    {
        [JsonPropertyName("Content")]
        public List<OrderSnapshotDTO> Content { get; set; }
    }
    public class OrderDisplayModel
    {
        public bool Kijelolve { get; set; } = false;
        public long Azonosito { get; set; }
        public string Email { get; set; }
        public string Nev { get; set; }
        public string Keresztnev { get; set; } // Ez kell az emailhez
        public decimal Osszeg { get; set; }
        public DateTime Frissitve { get; set; } // Szűréshez és megjelenítéshez
    }

    public class OrderSnapshotDTO
    {
        public long Id { get; set; }
        public string bvin { get; set; }
        public string OrderNumber { get; set; }
        public string UserEmail { get; set; }
        public decimal TotalGrand { get; set; }
        public bool IsPlaced { get; set; }
        public AddressDTO BillingAddress { get; set; }
        public string LastUpdatedUtc { get; set; } // Az API-ból stringként jön: "/Date(123...)/"
    }

    public class AddressDTO
    {
        public string City { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class GiftCardDTO
    {
        [JsonPropertyName("StoreId")]
        [Browsable(false)]
        public long StoreId { get; set; }

        [JsonPropertyName("CardNumber")]
        public string CardNumber { get; set; }

        [JsonPropertyName("Amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("UsedAmount")]
        public decimal UsedAmount { get; set; }

        [JsonPropertyName("IssueDateUtc")]
        public string IssueDateUtc { get; set; }

        [JsonPropertyName("ExpirationDateUtc")]
        public string ExpirationDateUtc { get; set; }

        [JsonPropertyName("RecipientEmail")]
        public string RecipientEmail { get; set; }

        [JsonPropertyName("RecipientName")]
        [Browsable(false)]
        public string RecipientName { get; set; }

        [JsonPropertyName("Enabled")]
        [Browsable(false)]
        public bool Enabled { get; set; }
    }
    public class GiftCardDisplayModel
    {
        public string Kartyaszam { get; set; }
        public decimal Osszeg { get; set; }
        public decimal Elhasznalt { get; set; }
        public string Email { get; set; }
        public DateTime Datum { get; set; } // Itt már valódi dátum van!
    }

    public class GiftCardRoot
    {
        [JsonPropertyName("Content")]
        public List<GiftCardDTO> Content { get; set; }
    }

}