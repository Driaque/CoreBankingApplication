using Core.BlogTest;
using System.Collections.Generic;

namespace Data.Repository
{
    public interface IBlogRepository
    {

        IList<Post> Posts(int pageNo, int pageSize);
        int TotalPosts();
        IList<Post> PostsForTag(string tagSlug, int pageNo, int pageSize);
        int TotalPostsForTag(string tagSlug);
        Tag Tag(string tagSlug);
        IList<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize);
        int TotalPostsForCategory(string categorySlug);
        Category Category(string categorySlug);
        IList<Post> PostsForSearch(string search, int pageNo, int pageSize);
        int TotalPostsForSearch(string search);
        Post Post(int year, int month, string titleSlug);
        IList<Category> Categories();
        IList<Tag> Tags();
    }
}
