using profunion.Domain.Models.NewsModels;
using profunion.Shared.Dto.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Interface.INews
{
    public interface INewsService
    {
        Task<(IEnumerable<GetNewsDto> newses, int TotalPage)> GetNewses(int page, NewsQueryDto query, SortStateNews sort);

        Task<GetNewsDto> GetNews(string newsId);

        Task<bool> CreateNews(CreateNewsDto createNews, CancellationToken cancellation);

        Task<bool> UpdateNews(string newsId, UpdateNewsDto updateNews);

        Task<bool> DeleteNews(string newsId, CancellationToken cancellation);

    }
}
