using Microsoft.AspNetCore.Mvc;
using TextTranslator.Service.Contracts;
using TextTranslator.Service.Model;

namespace TextTranslator.Api.Controllers;

[ApiController]
[Route("[controller]/Translate")]
public class TranslatorController(ITranslatorService translatorService) : ControllerBase
{
    [HttpPost()]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status202Accepted)]
    public IActionResult TranslateText([FromBody] string originalText) 
        => Accepted(translatorService.TranslateJob(originalText));

    [HttpGet("{workId}/Result")]
    [ProducesResponseType(typeof(TranslationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTranslationResult(Guid workId)
    {
        var translationResult = await translatorService.GetTranslationResult(workId);

        return translationResult is not null ? Ok(translationResult) : NotFound();
    }
}
