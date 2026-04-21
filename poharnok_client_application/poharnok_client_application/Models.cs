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
        public bool Selected { get; set; } = false; //alapértelmezetten nincsenek kijelölve a rendelések
        public long Azonosito { get; set; }
        public string Email { get; set; }
        public string Nev { get; set; }
        public decimal Osszeg { get; set; }
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
    }

    public class AddressDTO
    {
        public string City { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}