using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularTestBackend.Models
{
    public class FileDetailsVM
    {
        public int FileId { get; set; }
        public string UserName { get; set; }
        public string DocFile { get; set; }
        public string Image { get; set; }
        public string ImageName { get; set; }
    }
}