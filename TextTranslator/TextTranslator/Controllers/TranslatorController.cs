using Microsoft.AspNetCore.Mvc;
using TextTranslator.Service.Contracts;
using TextTranslator.Service.Model;

namespace TextTranslator.Api.Controllers;

[ApiController]
[Route("[controller]/Translate")]
public class TranslatorController(ITranslatorService translatorService) : ControllerBase
{
    /// <summary>
    /// Starts a text translation job asynchronously
    /// </summary>
    /// <param name="originalText">The text to be translated</param>
    /// <returns>A unique identifier (GUID) of the started translation job</returns>
    /// <response code="202">Translation job started successfully</response>
    [HttpPost()]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status202Accepted)]
    public IActionResult TranslateText([FromBody] string? originalText) 
        => Accepted(translatorService.TranslateJob(originalText));

    /// <summary>
    /// Gets the result of a completed translation job
    /// </summary>
    /// <param name="workId">The unique identifier of the translation job</param>
    /// <returns>The translation result if available</returns>
    /// <response code="200">Translation completed and result available</response>
    /// <response code="404">Translation job not found or not yet completed</response>
    [HttpGet("{workId}/Result")]
    [ProducesResponseType(typeof(TranslationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTranslationResult(Guid workId)
    {
        var translationResult = await translatorService.GetTranslationResult(workId);

        return translationResult is not null ? Ok(translationResult) : NotFound();
    }
}
