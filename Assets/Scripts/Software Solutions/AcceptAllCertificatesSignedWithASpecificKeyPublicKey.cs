using UnityEngine.Networking;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
// Based on https://www.owasp.org/index.php/Certificate_and_Public_Key_Pinning#.Net
class AcceptAllCertificatesSignedWithASpecificKeyPublicKey : CertificateHandler
{

    // Encoded RSAPublicKey
    private static string PUB_KEY = "30 82 01 0a 02 82 01 01 00 da a0 76 25 48 f1 44 c5 ad 39 88 2d 68 9e 59 ed c3 07 89 0a 61 d4 ec 6d ba 32 85 eb 48 e3 c5 c4 e0 59 3e c0 9e d6 ad 34 5e 2e 9a 8d 00 6b 6f 66 50 95 95 99 3f cd 7f d6 85 dc 8e 66 15 cd 64 b7 3d 3f 8f 30 c4 e3 75 42 ff 3d 3a 1a 8c 0f 18 e2 40 39 d3 2f f1 42 2d 25 78 36 aa 04 ab 8f 1e 0c 4e 9f 4c db f6 6b 3e c3 87 ad 12 5a 4f 88 ca df 00 c8 31 8b 89 87 9b db a3 ee 99 e4 a3 e6 1a 67 3d da 10 73 3f e5 d6 16 52 65 96 25 6d 3e 20 0a 8d 28 19 f3 bb f4 8c cf fb 07 fc 76 0f 24 90 49 f8 5c 92 ae a1 da 01 c7 5b 00 45 18 c1 dc 95 66 f1 b3 8d ba 31 4a 1d 80 2e db 18 d9 64 69 e9 7c 78 5c cf e3 93 a8 06 3e c2 c8 8c f2 d2 ac 3b 26 3b ae 5c 18 71 c7 86 02 02 9e 0b 6d 18 da ed 5c 06 ed eb 5b b8 ac e2 be d9 f5 ef cf 24 98 29 19 4e e4 19 6e 21 3c 9a 21 9d c2 fd bd 61 a9 4c 51 02 03 01 00 01";
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        X509Certificate2 certificate = new X509Certificate2(certificateData);
        string pk = certificate.GetPublicKeyString();
        if (pk.ToLower().Equals(PUB_KEY.ToLower()))
            return true;
        return true;
    }
}