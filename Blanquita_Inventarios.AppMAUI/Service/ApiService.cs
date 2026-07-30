using Blanquita_Inventarios.Entities;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Blanquita_Inventarios.AppMAUI.Service
{
    public class ApiService
    {
        // 🔑 UN SOLO HttpClient (REGLA DE ORO)
        private static readonly HttpClient _client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30) // ⏱️ evita espera infinita
        };

        public async Task<DBResponse<T>> PostObj<T>(
            string urlBase,
            string prefix,
            string controller,
            object model)
        {
            try
            {
                var requestJson = JsonConvert.SerializeObject(model);
                var content = new StringContent(
                    requestJson,
                    Encoding.UTF8,
                    "application/json");

                var url = $"{urlBase}{prefix}{controller}";

                var response = await _client.PostAsync(url, content);

                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new DBResponse<T>
                    {
                        ExecutionOK = false,
                        Message = string.IsNullOrWhiteSpace(answer)
                            ? response.ReasonPhrase
                            : answer
                    };
                }

                var obj = JsonConvert.DeserializeObject<DBResponse<T>>(answer);
                return obj;
            }
            catch (TaskCanceledException)
            {
                // ⏱️ TIMEOUT REAL
                return new DBResponse<T>
                {
                    ExecutionOK = false,
                    Message = "La solicitud tardó demasiado tiempo (timeout)."
                };
            }
            catch (Exception ex)
            {
                return new DBResponse<T>
                {
                    ExecutionOK = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<DBResponse<T>> Get<T>(
            string urlBase,
            string prefix,
            string controller,
            int id)
        {
            try
            {
                var url = $"{urlBase}{prefix}{controller}/{id}";

                var response = await _client.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new DBResponse<T>
                    {
                        ExecutionOK = false,
                        Message = string.IsNullOrWhiteSpace(answer)
                            ? response.ReasonPhrase
                            : answer
                    };
                }

                return JsonConvert.DeserializeObject<DBResponse<T>>(answer);
            }
            catch (TaskCanceledException)
            {
                return new DBResponse<T>
                {
                    ExecutionOK = false,
                    Message = "La solicitud tardó demasiado tiempo (timeout)."
                };
            }
            catch (Exception ex)
            {
                return new DBResponse<T>
                {
                    ExecutionOK = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<DBResponse<T>> GetData<T>(
            string urlBase,
            string prefix,
            string controller)
        {
            try
            {
                var url = $"{urlBase}{prefix}{controller}";

                var response = await _client.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new DBResponse<T>
                    {
                        ExecutionOK = false,
                        Message = string.IsNullOrWhiteSpace(answer)
                            ? response.ReasonPhrase
                            : answer
                    };
                }

                return JsonConvert.DeserializeObject<DBResponse<T>>(answer);
            }
            catch (TaskCanceledException)
            {
                return new DBResponse<T>
                {
                    ExecutionOK = false,
                    Message = "La solicitud tardó demasiado tiempo (timeout)."
                };
            }
            catch (Exception ex)
            {
                return new DBResponse<T>
                {
                    ExecutionOK = false,
                    Message = ex.Message
                };
            }
        }
    }
}
