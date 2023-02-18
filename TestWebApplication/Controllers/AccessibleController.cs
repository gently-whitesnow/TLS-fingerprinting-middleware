using Microsoft.AspNetCore.Mvc;

namespace TestWebApplication.Controllers;

public class AccessibleController : Controller
{
    [HttpPost("/touch")]
    public IActionResult TriggerTargetAction()
    {
        string fingerprint = HttpContext.Items["TlsFingerprint"] as string;
        foreach (var (key, value) in HttpContext.Request.Headers)
        {
            Console.WriteLine($"{key}  {value}");
        }

        return Ok(fingerprint ?? "nothing");
    }
}