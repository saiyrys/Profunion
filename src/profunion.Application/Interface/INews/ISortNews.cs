using profunion.Shared.Dto.News;

namespace profunion.Applications.Interface.INews
{
    public interface ISortNews
    {
        IEnumerable<GetNewsDto> SortObject(IEnumerable<GetNewsDto> news, SortStateNews? sort);
    }
}
