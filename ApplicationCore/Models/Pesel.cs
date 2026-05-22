using System;
using System.Collections.Generic;
using ApplicationCore.Commons;

namespace ApplicationCore.Models;

public class Pesel : ValueObject
{
    public string Value { get; }
    public DateTime BirthDate { get; }
    public Gender Gender { get; }
    public int ControlDigit { get; }
    public int CalculatedControlDigit { get; }

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
        ControlDigit = controlDigit;
        CalculatedControlDigit = calculatedControl;
        BirthDate = DecodeBirthDate(value);
        Gender = DecodeGender(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    private static DateTime DecodeBirthDate(string value)
    {
        int year = int.Parse(value.Substring(0, 2));
        int month = int.Parse(value.Substring(2, 2));
        int day = int.Parse(value.Substring(4, 2));

        int century;

        if (month >= 81)
        {
            century = 1800;
            month -= 80;
        }
        else if (month >= 61)
        {
            century = 2200;
            month -= 60;
        }
        else if (month >= 41)
        {
            century = 2100;
            month -= 40;
        }
        else if (month >= 21)
        {
            century = 2000;
            month -= 20;
        }
        else
        {
            century = 1900;
        }

        return new DateTime(century + year, month, day);
    }

    private static Gender DecodeGender(string value)
    {
        int genderDigit = value[9] - '0';
        return genderDigit % 2 == 1 ? Gender.Male : Gender.Female;
    }
}
