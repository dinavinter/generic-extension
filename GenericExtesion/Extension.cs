using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace GenericExtesion
{
    public class Extension
    {
        public string ExtensionId;
    }

    public class ExtensionModel
    {
        public string[] HttpCalls { get; set; }
        public int DelayMs { get; set; }
        
        [Required]
        public dynamic  Result { get; set; }
    }

}