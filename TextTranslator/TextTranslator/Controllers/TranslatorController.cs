using Microsoft.AspNetCore.Mvc;

namespace TextTranslator.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TranslatorController : ControllerBase
{
    [HttpPost("Translate")]
    [ProducesResponseType(typeof(string), StatusCodes.Status202Accepted)]
    public IActionResult TranslateText([FromBody] string originalText)
    {
        var workId = Guid.NewGuid().ToString();

        // TODO: Add translation logic here and store the result associated with workId

        return Accepted(workId);
    }

    [HttpGet("Result/{workId}")]
    public IActionResult GetTranslationResult(string workId)
    {
        // TODO: Retrieve the translation result associated with workId
        var translatedText = "Translated text for workId: " + workId; // Placeholder
        
        return !string.IsNullOrWhiteSpace(translatedText) ? Ok(translatedText) : NoContent();
    }
}
