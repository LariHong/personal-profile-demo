using Microsoft.AspNetCore.Mvc;
using PersonalProfile.Api.Dtos;
using PersonalProfile.Api.Services;

namespace PersonalProfile.Api.Controllers;

[ApiController]
[Route("api/profiles")]
public sealed class ProfilesController(IPersonalProfileService profiles) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PersonalProfileDto>>> Search(
        [FromQuery] string? keyword,
        CancellationToken cancellationToken)
    {
        return Ok(await profiles.SearchAsync(keyword, cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonalProfileDto>> Get(int id, CancellationToken cancellationToken)
    {
        var profile = await profiles.GetAsync(id, cancellationToken);
        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpPost]
    public async Task<ActionResult<PersonalProfileDto>> Create(
        [FromBody] UpsertPersonalProfileRequest request,
        CancellationToken cancellationToken)
    {
        var profile = await profiles.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = profile.Id }, profile);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PersonalProfileDto>> Update(
        int id,
        [FromBody] UpsertPersonalProfileRequest request,
        CancellationToken cancellationToken)
    {
        var profile = await profiles.UpdateAsync(id, request, cancellationToken);
        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await profiles.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
