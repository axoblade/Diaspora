using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Diaspora.Model
{
    public class Post
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique, MaxLength(100)]
        public string user_email { get; set; }
        
        public int stat { get; set; }

        public override string ToString()
        {
            return this.user_email;
        }
    }
}
