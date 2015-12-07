using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace DotNetLiberty.Http.Samples
{
    public class JsonPlaceholderPostsTest
    {
        private readonly HttpClient _client;

        [DataContract]
        public class Post
        {
            [DataMember(Name = "userId")]
            public int UserId { get; set; }

            [DataMember(Name = "id")]
            public int Id { get; set; }

            [DataMember(Name = "title")]
            public string Title { get; set; }

            [DataMember(Name = "body")]
            public string Body { get; set; }
        }

        public JsonPlaceholderPostsTest()
        {
            _client = new HttpClient()
                .WithBaseUri(new Uri("http://jsonplaceholder.typicode.com/posts/"))
                // We need to include a header to tell the API we want application/json
                .AcceptJson();
        }

        [Fact]
        public async void get_many()
        {
            var posts = (await _client.GetManyAsync<Post>()).ToList();

            Assert.Equal(100, posts.Count);
        }

        [Fact]
        public async void get_single()
        {
            var post = await _client.GetSingleAsync<Post>(1);

            Assert.NotNull(post);
            Assert.Equal(
                "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
                post.Title);
            Assert.Equal(
                "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto",
                post.Body);
            Assert.Equal(1, post.UserId);
            Assert.Equal(1, post.Id);
        }

        [Fact]
        public async void post()
        {
            var created = await _client.PostAsync(new Post
            {
                Id = 299,
                UserId = 305,
                Title = "Sample Title",
                Body = "Sample Body"
            });

            Assert.Equal(299, created.Id);
            Assert.Equal(305, created.UserId);
            Assert.Equal("Sample Title", created.Title);
            Assert.Equal("Sample Body", created.Body);
        }

        [Fact]
        public async void put()
        {
            var updated = await _client.PutAsync(1, new Post
            {
                Id = 1,
                UserId = 305,
                Title = "Updated Title",
                Body = "Updated Body"
            });

            Assert.Equal(1, updated.Id);
            Assert.Equal(305, updated.UserId);
            Assert.Equal("Updated Title", updated.Title);
            Assert.Equal("Updated Body", updated.Body);
        }

        [Fact]
        public async void delete()
        {
            await _client.DeleteAsync(1);
        }
    }
}
