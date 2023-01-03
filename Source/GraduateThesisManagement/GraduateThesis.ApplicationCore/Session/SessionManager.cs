using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

#nullable disable

namespace GraduateThesis.ApplicationCore.Session;

public class SessionManager
{
    private HttpContext _httpContext;

    public SessionManager(HttpContext httpContext)
    {
        _httpContext = httpContext;
    }

    public TSessionModel GetSession<TSessionModel>(string key)
    {
        string json = _httpContext.Session.GetString(key);
        return JsonConvert.DeserializeObject<TSessionModel>(json);
    }

    public void SetSession<TSessionModel>(string key, TSessionModel value)
    {
        string json = JsonConvert.SerializeObject(value);
        _httpContext.Session.SetString(key, json);
    }
}
