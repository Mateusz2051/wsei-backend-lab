using System;
using System.Collections.Generic;
using ApplicationCore.Commons;

namespace ApplicationCore.Models;

public class Pesel : ValueObject
{
    public string Value { get; }

    public Pesel(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("PESEL cannot be empty.");
        }

        value = value.Trim();

        if (value.Length != 11)
        {
            throw new ArgumentException("PESEL must be exactly 11 digits.");
        }

        int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
        int sum = 0;

        for (int i = 0; i < 10; i++)
        {
            if (!char.IsDigit(value[i]))
            {
                throw new ArgumentException("PESEL must contain only digits.");
            }
            sum += (value[i] - '0') * weights[i];
        }

        if (!char.IsDigit(value[10]))
        {
            throw new ArgumentException("PESEL must contain only digits.");
        }

        int controlDigit = value[10] - '0';
        int calculatedControl = (10 - (sum % 10)) % 10;

        if (controlDigit != calculatedControl)
        {
            throw new ArgumentException("PESEL checksum is invalid.");
        }

        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
