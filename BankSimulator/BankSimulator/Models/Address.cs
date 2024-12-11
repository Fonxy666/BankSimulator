namespace BankSimulator.Models
{
    internal class Address(int zipCode, string country, string city, string street, int houseNumber)
    {
        public int ZipCode { get; init; } = zipCode;
        public string Country { get; init; } = country;
        public string City { get; init; } = city;
        public string Street { get; init; } = street;
        public int HouseNumber { get; init; } = houseNumber;

        public override string ToString()
        {
            return $"ZipCode: {ZipCode}\n" +
                   $"Country: {Country}\n" +
                   $"City: {City}\n" +
                   $"Street: {Street}\n" +
                   $"HouseNumber: {HouseNumber}";
        }
    }
}
