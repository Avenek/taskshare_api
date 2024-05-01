using AutoMapper;
using API_project_system.ModelsDto;

namespace API_project_system.Services
{
    public interface IPaginationService
    {
        PageResults<T> PreparePaginationResults<T, T2>(GetAllQuery queryParameters, IQueryable<T2> query, IMapper mapper);
    }
    public class PaginationService : IPaginationService
    {
        public PageResults<T> PreparePaginationResults<T, T2>(GetAllQuery queryParameters, IQueryable<T2> query, IMapper mapper)
        {
            int resultsToSkip = queryParameters.PageSize * (queryParameters.PageNumber - 1);
            int resultCount = query.Count();
            var folders = query.Skip(resultsToSkip).Take(queryParameters.PageSize);
            var foldersDto = folders.Select(f => mapper.Map<T>(f)).ToList();

            var result = new PageResults<T>(foldersDto, resultCount, queryParameters.PageSize, queryParameters.PageNumber);

            return result;
        }
    }
}
