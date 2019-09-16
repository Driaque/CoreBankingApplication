using Core.BlogTest;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repository
{
    public class BlogRepository : IBlogRepository
    {
        // NHibernate object
        private readonly ISession _session;

        public BlogRepository(ISession session)
        {
            _session = session;
        }

        public IList<Post> Posts(int pageNo, int pageSize)
        {
            var posts = _session.Query<Post>()
                                  .Where(p => p.Published)
                                  .OrderByDescending(p => p.PostedOn)
                                  .Skip(pageNo * pageSize)
                                  .Take(pageSize)
                                  .Fetch(p => p.Category)
                                  .ToList();

            var postIds = posts.Select(p => p.Id).ToList();

            return _session.Query<Post>()
                  .Where(p => postIds.Contains(p.Id))
                  .OrderByDescending(p => p.PostedOn)
                  .FetchMany(p => p.Tags)
                  .ToList();
        }

        public int TotalPosts()
        {
            return _session.Query<Post>().Where(p => p.Published).Count();
        }
        public IList<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize)
        {
            var posts = _session.Query<Post>()
                                .Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
                                .OrderByDescending(p => p.PostedOn)
                                .Skip(pageNo * pageSize)
                                .Take(pageSize)
                                .Fetch(p => p.Category)
                                .ToList();

            var postIds = posts.Select(p => p.Id).ToList();

            return _session.Query<Post>()
                          .Where(p => postIds.Contains(p.Id))
                          .OrderByDescending(p => p.PostedOn)
                          .FetchMany(p => p.Tags)
                          .ToList();
        }

        public int TotalPostsForCategory(string categorySlug)
        {
            return _session.Query<Post>()
                        .Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
                        .Count();
        }

        public Category Category(string categorySlug)
        {
            return _session.Query<Category>()
                        .FirstOrDefault(t => t.UrlSlug.Equals(categorySlug));
        }
        public IList<Post> PostsForTag(string tagSlug, int pageNo, int pageSize)
        {
            var posts = _session.Query<Post>()
                              .Where(p => p.Published && p.Tags.Any(t => t.UrlSlug.Equals(tagSlug)))
                              .OrderByDescending(p => p.PostedOn)
                              .Skip(pageNo * pageSize)
                              .Take(pageSize)
                              .Fetch(p => p.Category)
                              .ToList();

            var postIds = posts.Select(p => p.Id).ToList();

            return _session.Query<Post>()
                          .Where(p => postIds.Contains(p.Id))
                          .OrderByDescending(p => p.PostedOn)
                          .FetchMany(p => p.Tags)
                          .ToList();
        }

        public int TotalPostsForTag(string tagSlug)
        {
            return _session.Query<Post>()
                        .Where(p => p.Published && p.Tags.Any(t => t.UrlSlug.Equals(tagSlug)))
                        .Count();
        }

        public Tag Tag(string tagSlug)
        {
            return _session.Query<Tag>()
                        .FirstOrDefault(t => t.UrlSlug.Equals(tagSlug));
        }

        public IList<Post> PostsForSearch(string search, int pageNo, int pageSize)
        {
            var posts = _session.Query<Post>()
                                  .Where(p => p.Published && (p.Title.Contains(search) || p.Category.Name.Equals(search) || p.Tags.Any(t => t.Name.Equals(search))))
                                  .OrderByDescending(p => p.PostedOn)
                                  .Skip(pageNo * pageSize)
                                  .Take(pageSize)
                                  .Fetch(p => p.Category)
                                  .ToList();

            var postIds = posts.Select(p => p.Id).ToList();

            return _session.Query<Post>()
                  .Where(p => postIds.Contains(p.Id))
                  .OrderByDescending(p => p.PostedOn)
                  .FetchMany(p => p.Tags)
                  .ToList();
        }

        public int TotalPostsForSearch(string search)
        {
            return _session.Query<Post>()
                    .Where(p => p.Published && (p.Title.Contains(search) || p.Category.Name.Equals(search) || p.Tags.Any(t => t.Name.Equals(search))))
                    .Count();
        }
        public Post Post(int year, int month, string titleSlug)
        {
            var query = _session.Query<Post>()
                                .Where(p => p.PostedOn.Year == year && p.PostedOn.Month == month && p.UrlSlug.Equals(titleSlug))
                                .Fetch(p => p.Category);

            query.FetchMany(p => p.Tags).ToFuture();

            return query.ToFuture().Single();
        }
        public IList<Category> Categories()
        {
            return _session.Query<Category>().OrderBy(p => p.Name).ToList();
        }
        public IList<Tag> Tags()
        {
            return _session.Query<Tag>().OrderBy(p => p.Name).ToList();
        }

    }
}
