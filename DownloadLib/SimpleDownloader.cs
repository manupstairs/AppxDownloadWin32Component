using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DownloadLib
{
    public class SimpleDownloader
    {
        public async Task<Stream> RequestHttpContentAsync(string uriString, string userName, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(uriString);
                client.DefaultRequestHeaders.Accept.Clear();
                var authorization = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{userName}:{password}"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorization);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(uriString);
                if (response.IsSuccessStatusCode)
                {
                    HttpContent content = response.Content;
                    var contentStream = await content.ReadAsStreamAsync();
                    return contentStream;
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
        }
    }
}
