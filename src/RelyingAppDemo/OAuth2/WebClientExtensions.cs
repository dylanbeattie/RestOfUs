using System;
using System.Net;
using System.Text;

namespace RelyingAppDemo.OAuth2 {
    public static class WebClientExtensions {
        public static void AuthorizeBasic(this WebClient client, string username, string password) {
            string auth = String.Format("{0}:{1}", username, password);
            string value = String.Format("Basic {0}", Convert.ToBase64String(Encoding.ASCII.GetBytes(auth)));
            client.Headers.Add("Authorization", value);
        }

        public static void AuthorizeBearer(this WebClient client, string token) {
            client.Headers.Add("Authorization", "Bearer " + token);
        }
    }
}