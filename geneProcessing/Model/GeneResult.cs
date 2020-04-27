using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UploadFilesServer.Models
{
    public class GeneResult
    {
        public int Id { get; set; }
        public string GeneName { get; set; }
        public int Result { get; set; }
        public string Text { get; set; }

    }
}
