using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TLS_fingerprinting_middleware;

public class TlsFingerprintingMiddleware
{
    private readonly RequestDelegate _next;

    public TlsFingerprintingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.IsHttps)
        {
            // Get the client certificate from the SslStream
            X509Certificate2 serverCertificate = context.Connection?.ClientCertificate as X509Certificate2;

            // Get the SHA-256 fingerprint of the certificate
            byte[] rawData = serverCertificate.GetCertHash();
            string fingerprint = BitConverter.ToString(rawData).Replace("-", ":").ToLower();

            // Store the fingerprint in the HttpContext
            context.Items["tls-fingerprint"] = fingerprint;
        }

        await _next(context);
    }
}