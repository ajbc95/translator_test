using Microsoft.AspNetCore.Mvc;
using TextTranslator.Service.Contracts;
using TextTranslator.Service.Model;

namespace TextTranslator.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TranslatorController(ITranslatorService translatorService) : ControllerBase
{
    [HttpPost("Translate")]
    [ProducesResponseType(typeof(TranslationResult), StatusCodes.Status202Accepted)]
    public IActionResult TranslateText([FromBody] string originalText)
    {
        var workId = Guid.NewGuid().ToString();

        var result = translatorService.TranslateAsync(originalText);
        result.Wait();

        return Accepted(result.Result);
    }

    [HttpGet("Result/{workId}")]
    [ProducesResponseType(typeof(TranslationResult), StatusCodes.Status200OK)]
    public IActionResult GetTranslationResult(string workId)
    {
        // TODO: Retrieve the translation result associated with workId
        var translatedText = "Translated text for workId: " + workId; // Placeholder
        
        return !string.IsNullOrWhiteSpace(translatedText) ? Ok(translatedText) : NoContent();
    }
}
