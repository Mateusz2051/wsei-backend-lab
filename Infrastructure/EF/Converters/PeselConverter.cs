using ApplicationCore.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.EF.Converters;

public class PeselConverter : ValueConverter<Pesel, string>
{
    public PeselConverter() : base(
        pesel => pesel.Value,
        value => new Pesel(value))
    {
    }
}
