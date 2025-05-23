namespace Backend_Service.Domain.DTOs;

public class PagedResult<T>
{
    public T[] Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}