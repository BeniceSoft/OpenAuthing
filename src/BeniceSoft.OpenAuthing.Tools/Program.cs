using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using BeniceSoft.OpenAuthing.Tools.Options;
using CommandLine;

namespace BeniceSoft.OpenAuthing.Tools;

public class Program
{
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<OtherOptions, CertificateOptions>(args)
            .MapResult(
                (OtherOptions options) => PrintOtherAndReturnExitCode(options),
                (CertificateOptions options) => RunCertificateAndReturnExitCode(options),
                PrintErrorsAndReturnExitCode
            );
    }

    static int PrintErrorsAndReturnExitCode(IEnumerable<Error> errors)
    {
        Console.WriteLine("Occurred error:");

        foreach (var error in errors)
        {
            Console.WriteLine($"\t- {error}");
        }

        return 1;
    }

    static int PrintOtherAndReturnExitCode(OtherOptions options)
    {
        return 0;
    }

    static int RunCertificateAndReturnExitCode(CertificateOptions options)
    {
        GenerateCertificatePfxFile(new X500DistinguishedName("CN=Fabrikam Encryption Certificate"),X509KeyUsageFlags.KeyEncipherment, "encryption-certificate.pfx");
        Console.WriteLine("encryption-certificate.pfx generated!");

        GenerateCertificatePfxFile(new X500DistinguishedName("CN=Fabrikam Signing Certificate"), X509KeyUsageFlags.DigitalSignature,"signing-certificate.pfx");
        Console.WriteLine("signing-certificate.pfx generated!");

        return 0;
    }

    static void GenerateCertificatePfxFile(X500DistinguishedName subjectName, X509KeyUsageFlags keyUsage, string path)
    {
        using var algorithm = RSA.Create(keySizeInBits: 2048);

        var request = new CertificateRequest(subjectName, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        request.CertificateExtensions.Add(new X509KeyUsageExtension(keyUsage, critical: true));

        var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));

        File.WriteAllBytes(path, certificate.Export(X509ContentType.Pfx, string.Empty));
    }
}