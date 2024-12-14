namespace BankSimulator.Models
{
    internal class Address(Guid userId, int zipCode, string country, string city, string street, int houseNumber)
    {
        public Guid UserId { get; set; } = userId;
        public Guid AddressId { get; set; } = Guid.NewGuid();
        public int ZipCode { get; init; } = zipCode;
        public string Country { get; init; } = country;
        public string City { get; init; } = city;
        public string Street { get; init; } = street;
        public int HouseNumber { get; init; } = houseNumber;
    }
}
