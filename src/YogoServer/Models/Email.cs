using System.Collections.Generic;

namespace YogoServer.Models
{
   public class Email
   {
        public HeadEmail HeadEmail { get; set; }

        public Dictionary<string, List<string>> Body { get; set; }
   }
}