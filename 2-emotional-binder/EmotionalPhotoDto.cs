using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AwesomeApi
{
    [ModelBinder(typeof(AwesomeModelBinder))]
    public class EmotionalPhotoDto
    {
        public byte[] Contents { get; set; }
        public Dictionary<string, double>[] Scores { get; set; }
    }
}
