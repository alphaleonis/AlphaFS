#addin nuget:?package=RestSharp
using RestSharp;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;

/// <summary>Invokes a REST api.</summary>
T RestInvoke<T>(string baseUrl, string resource, Method method) where T : new()
{
    var client = new RestClient(baseUrl);
    var request = new RestRequest(resource, method);
    Verbose($"{method} {baseUrl}/{resource}");
    var result = client.Execute<T>(request);
    if (result.StatusCode != HttpStatusCode.OK || !result.IsSuccessful)
    {
        throw new Exception($"REST call to {baseUrl}/{resource} failed with status {result.StatusCode}. Error message: {result.ErrorMessage}");
    }
    return result.Data;    
}