using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day04
{
    class Program
    {
        static void Main(string[] args)
        {
            var rows = System.IO.File.ReadAllLines("input.txt");
            var passports = ParsePassports(rows);
            Console.WriteLine($"Part 1: {passports.Count(p => p.ValidSimple)}");
            Console.WriteLine($"Part 2: {passports.Count(p => p.Valid)}");
        }

        static List<Passport> ParsePassports(IEnumerable<string> rows)
        {
            List<Passport> result = new List<Passport>();
            var current = new Passport();
            foreach (var row in rows)
            {
                if(string.IsNullOrWhiteSpace(row))
                {
                    result.Add(current);
                    current = new Passport();
                    continue;
                }

                var fields = row.Trim().Split(' ');
                foreach (var field in fields)
                {
                    var f = field.Split(':');
                    switch (f[0])
                    {
                        case "byr":
                            current.BirthDate = f[1];
                            break;
                        case "iyr":
                            current.IssueYear = f[1];
                            break;
                        case "eyr":
                            current.ExpirationYear = f[1];
                            break;
                        case "hgt":
                            current.Height = f[1];
                            break;
                        case "ecl":
                            current.EyeColour = f[1];
                            break;
                        case "hcl":
                            current.HairColour = f[1];
                            break;
                        case "pid":
                            current.PassportId = f[1];
                            break;
                        case "cid":
                            current.CountryId = f[1];
                            break;
                    }
                }
            }

            return result;
        }
    }

    class Passport
    {
        public string BirthDate { get; set; }
        public string IssueYear { get; set; }
        public string ExpirationYear { get; set; }
        public string Height { get; set; }
        public string EyeColour { get; set; }
        public string HairColour { get; set; }
        public string PassportId { get; set; }
        public string CountryId { get; set; }

        public bool ValidSimple => 
            !string.IsNullOrWhiteSpace(BirthDate) &&
            !string.IsNullOrWhiteSpace(IssueYear) &&
            !string.IsNullOrWhiteSpace(ExpirationYear) &&
            !string.IsNullOrWhiteSpace(Height) &&
            !string.IsNullOrWhiteSpace(EyeColour) &&
            !string.IsNullOrWhiteSpace(HairColour) &&
            !string.IsNullOrWhiteSpace(PassportId);

        public bool Valid => 
            !string.IsNullOrWhiteSpace(BirthDate) && YearBetween(BirthDate, 1920, 2002) &&
            !string.IsNullOrWhiteSpace(IssueYear) && YearBetween(IssueYear, 2010, 2020) &&
            !string.IsNullOrWhiteSpace(ExpirationYear) && YearBetween(ExpirationYear, 2020, 2030) &&
            !string.IsNullOrWhiteSpace(Height) && IsHeight(Height) &&
            !string.IsNullOrWhiteSpace(EyeColour) && IsEyeColour(EyeColour) &&
            !string.IsNullOrWhiteSpace(HairColour) && IsHexColour(HairColour) &&
            !string.IsNullOrWhiteSpace(PassportId) && IsPassportId(PassportId);

        private bool YearBetween(string value, int start, int end)
        {
            Regex rx = new Regex("^[0-9]{4}$");
            return rx.IsMatch(value) && int.TryParse(value, out int o) && o >= start && o <= end;
        }

        private bool IsHeight(string value)
        {
            if(int.TryParse(value.Substring(0, value.Length - 2), out int hght))
            {
                if(new Regex("^[0-9]{1,20}cm").IsMatch(value) && hght >= 150 && hght <= 193)
                    return true;
                if(new Regex("^[0-9]{1,20}in").IsMatch(value) && hght >= 59 && hght <= 76)
                    return true;
            }

            return false;
        }

        private bool IsEyeColour(string value)
        {
            var allowed = new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
            return allowed.Contains(value);
        }

        private bool IsHexColour(string value)
        {
            return new Regex("^#[0-9a-f]{6}$").IsMatch(value);
        }

        private bool IsPassportId(string value)
        {
            return new Regex("^[0-9]{9}$").IsMatch(value);
        }
    }
}
