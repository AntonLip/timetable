using System;
using System.Collections.Generic;
using System.Text;

namespace TimetibleMicroservices.Models
{
    public class ResultDto<TModel>
    {
        public TModel Value { get; set; }

        public List<string> Errors { get; set; }
    }
}
