using System;
using System.Collections.Generic;

namespace Markie.ViewModels
{
    public class PostsIndexViewModel
    {
        public IEnumerable<PostInformation> Drafts { get; set; }
        public IEnumerable<PostInformation> Posts { get; set; }

        public PostsIndexViewModel()
        {
            Drafts = new List<PostInformation>();
            Posts = new List<PostInformation>();
        }

        public class PostInformation
        {
            public int PostId { get; set; }
            public DateTime Date { get; set; }
            public string Title { get; set; }
        }
    }
}