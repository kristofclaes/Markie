using System;
using System.Collections.Generic;
using System.Linq;
using Markie.ViewModels;
using Nancy;

namespace Markie.Modules
{
    public class PostsModule : NancyModule
    {
        public PostsModule() : base("/admin/posts")
        {
            // Returns 10 most recent drafts and 10 most recent posts
            Get["/"] = parameters =>
                {
                    var drafts = new List<PostsIndexViewModel.PostInformation>();
                    var posts = new List<PostsIndexViewModel.PostInformation>();

                    for (int i = 1; i < 11; i++)
                    {
                        var draftPost = new PostsIndexViewModel.PostInformation
                            { Date = DateTime.Now.AddDays(0 - i), PostId = i, Title = string.Format("Draft post {0:00}", i) };

                        var publishedPost = new PostsIndexViewModel.PostInformation
                            { Date = DateTime.Now.AddDays(0 - i), PostId = i, Title = string.Format("Published post {0:00}", i) };

                        drafts.Add(draftPost);
                        posts.Add(publishedPost);
                    }

                    return View["Index.cshtml", new PostsIndexViewModel { Drafts = drafts, Posts = posts }];
                };
        }
    }
}