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
        private int BufferSize => 16 * 1024;

        public async Task<string> DownloadFile(string uriString, string saveFileName, string userName, string password)
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
                    var savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, saveFileName);
                    using (var fileStream = new FileStream(savePath, FileMode.Create))
                    {
                        byte[] buffer = new byte[BufferSize];
                        int bytesRead;
                        do
                        {
                            bytesRead = contentStream.Read(buffer, 0, BufferSize);
                            fileStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead > 0);

                    }

                    return savePath;
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
        }
    }
}
