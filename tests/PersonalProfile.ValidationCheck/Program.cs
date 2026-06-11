using PersonalProfile.Api.Validation;

var validator = new TaiwanNationalIdAttribute();
var cases = new[]
{
    (Value: "A123456789", Expected: true),
    (Value: "A123456788", Expected: false),
    (Value: "Z223456789", Expected: false),
    (Value: "AA23456789", Expected: false),
    (Value: "", Expected: false)
};

foreach (var testCase in cases)
{
    var actual = validator.IsValid(testCase.Value);
    if (actual != testCase.Expected)
    {
        Console.Error.WriteLine($"Failed: {testCase.Value}, expected {testCase.Expected}, got {actual}");
        return 1;
    }
}

Console.WriteLine("Taiwan national ID validation checks passed.");
return 0;
