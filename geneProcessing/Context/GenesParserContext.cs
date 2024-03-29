﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UploadFilesServer.Models;

namespace UploadFilesServer.Context
{
    public class GenesParserContext: DbContext
    {
        public GenesParserContext(DbContextOptions options)
            :base(options)
        {
        }

        public DbSet<GeneResult> GeneResults { get; set; }
    }
}
