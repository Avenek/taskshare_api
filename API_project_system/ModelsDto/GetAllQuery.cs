namespace API_project_system.ModelsDto
{
    public class GetAllQuery
    {
        public string SearchPhrase { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
