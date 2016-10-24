using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using KTrackCommonUtility;

namespace BOXPOC_JWT.Common
{
    //DebuggerStepThrough]
    public class KTrackHttpClient : HttpClient
    {
        private static readonly string BaseUrl = clsCommonPaths.GetWebAPIBaseURL();

        public KTrackHttpClient()
        {
            // if tokens are available add it to headers
            if (HttpContext.Current.Session["Global_Client_ID"] != null)
            {
                DefaultRequestHeaders.TryAddWithoutValidation("clientId", HttpContext.Current.Session["Global_Client_ID"].ToString());
            }
            if (HttpContext.Current.Session["authToken"] != null)
            {
                DefaultRequestHeaders.TryAddWithoutValidation("Token", HttpContext.Current.Session["authToken"].ToString());
            }
        }

        public T Get<T>(string url, object parameters = null)
        {
            var fullUrl = BaseUrl + url + GetParameters(parameters);
            var response = GetAsync(fullUrl).Result;
            var data = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<ErrorMessageModelView>(data);
                throw new KTrackBaseException(string.Empty, string.Empty, error.Message, (CommonUtility.KTrackHttpStatusCode)response.StatusCode);
            }

            return JsonConvert.DeserializeObject<T>(data);
        }

        public IEnumerable<T> GetAll<T>(string url, object parameters = null)
        {
            return Get<IEnumerable<T>>(url, parameters);
        }

        public IEnumerable<dynamic> GetAll(string url, object parameters = null)
        {
            var fullUrl = BaseUrl + url + GetParameters(parameters);
            var response = GetAsync(fullUrl).Result;
            var data = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<ErrorMessageModelView>(data);
                throw new KTrackBaseException(string.Empty, string.Empty, error.Message, (CommonUtility.KTrackHttpStatusCode)response.StatusCode);
            }

            return JArray.Parse(data);
        }

        public T Post<T>(string url, object parameters = null)
        {
            var fullUrl = BaseUrl + url;
            var content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json");
            var response = PostAsync(fullUrl, content).Result;
            var data = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new KTrackBaseException(string.Empty, string.Empty, "Not Found", (CommonUtility.KTrackHttpStatusCode)response.StatusCode);
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var error = JsonConvert.DeserializeObject<ErrorMessageModelView>(data);
                throw new KTrackBaseException(string.Empty, string.Empty, error.Message, (CommonUtility.KTrackHttpStatusCode)response.StatusCode);
            }

            return JsonConvert.DeserializeObject<T>(data);
        }

        public void Post(string url, object parameters = null)
        {
            var fullUrl = BaseUrl + url;
            var content = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json");
            var response = PostAsync(fullUrl, content).Result;
            var data = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent)
            {
                var error = JsonConvert.DeserializeObject<ErrorMessageModelView>(data);
                throw new KTrackBaseException(string.Empty, string.Empty, error.Message, (CommonUtility.KTrackHttpStatusCode)response.StatusCode);
            }
        }

        public void Delete(string url, object parameters = null)
        {
            var fullUrl = BaseUrl + url + GetParameters(parameters);
            var response = DeleteAsync(fullUrl).Result;
            var data = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent)
            {
                var error = JsonConvert.DeserializeObject<ErrorMessageModelView>(data);
                throw new KTrackBaseException(string.Empty, string.Empty, error.Message, (CommonUtility.KTrackHttpStatusCode)response.StatusCode);
            }
        }

        private static string GetParameters(object parameters)
        {
            if (parameters == null)
            {
                return string.Empty;
            }

            return "?" + string.Join("&",  parameters.GetType().GetProperties().Select(p => p.Name + "=" + p.GetValue(parameters).ToString()).ToArray());
        }

    }
}