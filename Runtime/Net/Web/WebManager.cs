using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;

namespace UnityLib.Web
{
    public class WebManager
    {
        private static CertificateHandler m_certificate;

        public static CertificateHandler CertificateHandler
        {
            get
            {
                if (m_certificate == null)
                {
                    m_certificate = new WebPassCertif();
                }
                return m_certificate;
            }
            set
            {
                m_certificate = value;
            }
        }

        public static Action<AsyncOperation> ErrrorHandle { get; set; }

        public static UnityWebRequestAsyncOperation Send(string url)
        {
            Debug.Assert(url != null);
            var webRequest = UnityWebRequest.Get(url);

            webRequest.certificateHandler = CertificateHandler;
            var operation = webRequest.SendWebRequest();
            if (ErrrorHandle != null)
            {
                operation.completed += ErrrorHandle;
            }
            return operation;
        }


    }
}