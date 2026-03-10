using Microsoft.AspNetCore.Mvc;
using CrowdLens.Models;

[ApiController]
[Route("api/[controller]")] // This makes the route: api/posts (strips Controller from PostsController.cs)
public class PostsController : ControllerBase // PostsController is the basis for the route api/posts
{
    // Mock data (like a temporary collection in Laravel)
    private static List<Post> _posts = new()
    {
        new Post { Id = 1, Title = "First Post", Content = "Hello CrowdLens!" }
    };

    // GET: api/posts (Like index() in Laravel)
    [HttpGet]
    public ActionResult<IEnumerable<Post>> GetPosts()
    {
        return Ok(_posts);
    }

    // POST: api/posts (Like store() in Laravel)
    [HttpGet("{id}")]
    public ActionResult<Post> GetPost(int id)
    {
        var post = _posts.FirstOrDefault(p => p.Id == id);
        return post == null ? NotFound() : Ok(post);
    }
}