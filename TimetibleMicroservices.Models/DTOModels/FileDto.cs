﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TimetibleMicroservices.Models.DTOModels
{
public    class FileDto
    {
        public byte [] FileData { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
    }
}
