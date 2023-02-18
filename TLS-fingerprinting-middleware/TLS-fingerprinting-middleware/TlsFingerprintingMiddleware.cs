using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
        // Get the X-SSL-CERT header value from the request
        string clientCertHeader = context.Request.Headers["X-SSL-CERT"];

        // Convert the client certificate from PEM format to X509Certificate2 object
        X509Certificate2 clientCert = new X509Certificate2(Encoding.ASCII.GetBytes(clientCertHeader));

        // Get the client certificate's public key and calculate its SHA256 hash
        RSACryptoServiceProvider rsa = (RSACryptoServiceProvider) clientCert.PublicKey.Key;
        byte[] hash = SHA256.Create().ComputeHash(rsa.ExportSubjectPublicKeyInfo());

        // Convert the hash to a hexadecimal string
        string fingerprint = BitConverter.ToString(hash).Replace("-", "").ToLower();

        // Store the fingerprint in the HttpContext
        context.Items["tls-fingerprint"] = fingerprint;


        await _next(context);
    }
}