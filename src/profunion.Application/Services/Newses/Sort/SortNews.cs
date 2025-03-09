using profunion.Applications.Interface.INews;
using profunion.Shared.Dto.News;


namespace profunion.Applications.Services.Newses.Sort
{
    public class SortNews : ISortNews
    {
        public IEnumerable<GetNewsDto> SortObject(IEnumerable<GetNewsDto> news, SortStateNews? sort)
        {
            switch (sort)
            {
                case SortStateNews.AlphabeticAsc:
                    news = news.OrderBy(n => n.title);
                    break;
                case SortStateNews.AlphabeticDesc:
                    news = news.OrderByDescending(n => n.title);
                    break;
                case SortStateNews.ViewsAsc:
                    news = news.OrderBy(n => n.views);
                    break;
                case SortStateNews.ViewsDesc:
                    news = news.OrderByDescending(n => n.views);
                    break;
                case SortStateNews.DateAsc:
                    news = news.OrderBy(n => n.createdAt);
                    break;
                case SortStateNews.DateDesc:
                    news = news.OrderByDescending(n => n.createdAt);
                    break;
                default:
                    news = news.OrderBy(n => n.title);
                    break;
            }

            return news.ToList();
        }
    }
}
