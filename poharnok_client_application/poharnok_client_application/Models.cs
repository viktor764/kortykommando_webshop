using System.Collections.Generic;
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
        public bool Selected { get; set; } = true;
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
        public long StoreId { get; set; }
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal UsedAmount { get; set; }

        // Stringként vesszük át az API-tól, hogy ne dögöljön meg a konvertálásnál
        public string IssueDateUtc { get; set; }
        public string ExpirationDateUtc { get; set; }

        public string RecipientEmail { get; set; }
        public string RecipientName { get; set; }
        public bool Enabled { get; set; }
    }

    public class GiftCardRoot
    {
        [JsonPropertyName("Content")]
        public List<GiftCardDTO> Content { get; set; }
    }

}