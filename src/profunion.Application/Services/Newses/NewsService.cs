using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using profunion.Applications.Interface.INews;
using profunion.Domain.Models.EventModels;
using profunion.Domain.Models.NewsModels;
using profunion.Domain.Persistance;
using profunion.Infrastructure.Data;
using profunion.Shared.Common.Interfaces;
using profunion.Shared.Common.Service;
using profunion.Shared.Dto.Events;
using profunion.Shared.Dto.News;
using profunion.Shared.Dto.Uploads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace profunion.Applications.Services.Newses
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IPagination _pagination;
        private readonly IUpdateMethods _update;
        private readonly ISortNews _sortNews;
        private readonly ApplicationDbContext _context;

        public NewsService(INewsRepository newsRepository, IFileRepository fileRepository, IMapper mapper, IConfiguration configuration,IPagination pagination,IUpdateMethods update,ISortNews sortNews, ApplicationDbContext context)
        {
            _newsRepository = newsRepository;
            _fileRepository = fileRepository;
            _mapper = mapper;
            _configuration = configuration;
            _pagination = pagination;
            _update = update;
            _sortNews = sortNews;
            _context = context;
        }

        public async Task<(IEnumerable<GetNewsDto> newses, int TotalPage)> GetNewses(int page, NewsQueryDto query, SortStateNews sort)
        {
            int pageSize = 12;

            var newses = await GetFullNewsData();

            if (!string.IsNullOrEmpty(query.search))
            {
                newses = await Search<GetNewsDto>.SearchEntities(newses, query.search);
            }

            if (sort != SortStateNews.Current)
            {
                newses = _sortNews.SortObject(newses, sort);
            }

            if (query.created_at_start != null || query.created_at_end != null)
            {
                newses = newses.Where(n =>
                    (!string.IsNullOrEmpty(query.created_at_start) ? n.createdAt.Date >= DateTime.Parse(query.created_at_start).Date : true) &&
                    (!string.IsNullOrEmpty(query.created_at_end) ? n.createdAt.Date <= DateTime.Parse(query.created_at_end).Date : true) 
                ).ToList();
            }

           var paginationItem = await _pagination.Paginate(newses.ToList(), page);

            newses = paginationItem.Items;
            int totalPages = paginationItem.TotalPages;

            return (newses, totalPages);
        }

        public async Task<GetNewsDto> GetNews(string newsId)
        {
            var newses = await GetFullNewsData();

            var news = newses.FirstOrDefault(e => e.newsId == newsId);

            return news;
        }

        public async Task<bool> CreateNews(CreateNewsDto newsCreate, CancellationToken cancellation)
        {
            var NewsGet = await _newsRepository.GetAllAsync();

            if (NewsGet == null)
                throw new ArgumentNullException();

            var newsMap = _mapper.Map<News>(newsCreate);

            if (newsCreate.imagesId?.Any() == true)
            {
                newsMap.NewsUploads = newsCreate.imagesId
                    .Select(c => new NewsUploads { fileId = c, newsId = newsMap.newsId })
                    .ToList();
            }

            if (!await _newsRepository.CreateAsync(newsMap, cancellation))
            {
                throw new InvalidOperationException("Что то пошло не так при создании");
            }

            return true;
        }

        public async Task<bool> UpdateNews(string newsId, UpdateNewsDto updateNews)
        {
            await _update.UpdateEntity<News, UpdateNewsDto, string>(newsId, updateNews);

            if (updateNews.imagesId != null && updateNews.imagesId.Any())
            {
               /* var newsUploads = _context.NewsUploads.Where(nu => nu.newsId == newsId).ToList();
                _context.NewsUploads.RemoveRange(newsUploads);*/
                await _fileRepository.DeleteNewsFile(newsId);

                foreach (var uploadId in updateNews.imagesId)
                {
                    _context.NewsUploads.Add(new NewsUploads
                    {
                        newsId = newsId,
                        fileId = uploadId
                    });
                }
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteNews(string newsId, CancellationToken cancellation)
        {
            var newsDelete = await _newsRepository.GetByIdAsync(newsId);

            if (newsDelete == null)
                throw new ArgumentNullException();

            await _fileRepository.DeleteNewsFile(newsId);

            await _context.SaveChangesAsync(cancellation);

            if (!await _newsRepository.DeleteAsync(newsDelete))
            {
                throw new ArgumentException("Ошибка удаления ивента");
            }

            return true;

        }

        private async Task<IEnumerable<GetNewsDto>> GetFullNewsData()
        {
            var baseUrl = _configuration["NewsUrl"];
            var events = await _context.News
                .Include(n => n.NewsUploads)
                    .ThenInclude(eu => eu.Uploads)
                 .Select(n => new GetNewsDto
                 {
                     newsId = n.newsId,
                     title = n.title,
                     description = n.description,
                     content = n.content,
                     views = n.views,
                     createdAt = n.createdAt,
                     updatedAt = n.updatedAt,
                     images = n.NewsUploads.Select(nu => new GetUploadsDto
                     {
                         id = nu.fileId,
                         name = $"{_context.Uploads.FirstOrDefault(u => u.id == nu.fileId).fileName}",
                         Url = $"{baseUrl}{_context.Uploads.FirstOrDefault(u => u.id == nu.fileId).fileName}"
                     }).ToList(),
                 }).ToListAsync();

            return events;
        }

        public async Task<bool> IncrementationViews(string newsId)
        {
            var news = await _newsRepository.GetByIdAsync(newsId);

            news.views += 1;

            await _newsRepository.UpdateAsync(news);

            return true;
        }
    }
}
