using AbsenDulu.BE.Interfaces.IServices;
namespace AbsenDulu.BE.Services.URI;
public class URiService:IUriService
{
    private readonly string _baseUri;
    public URiService(string baseurl)
    {
        _baseUri=baseurl;
    }
    // public Uri GetPageUri(PaginationFilter filter, string route)
    // {
    //     // var _enpointUri = new Uri(string.Concat(_baseUri, route));
    //     // var modifiedUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
    //     // modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
    //     // return new Uri(modifiedUri);
    // }
}