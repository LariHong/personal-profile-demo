namespace PersonalProfile.Api.Services;

public sealed class DuplicateNationalIdException()
    : InvalidOperationException("身分證字號已存在。")
{
}
