using UnityEngine.Networking;

namespace UnityLib.Web
{
    public class WebPassCertif : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}