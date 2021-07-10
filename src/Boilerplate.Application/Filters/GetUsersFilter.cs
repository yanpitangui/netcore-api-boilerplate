namespace Boilerplate.Application.Filters
{
    public class GetUsersFilter : PaginationInfoFilter
    {
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
