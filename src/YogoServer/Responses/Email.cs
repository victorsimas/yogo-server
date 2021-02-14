using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using YogoServer.Models;

namespace YogoServer.Responses
{
   public class Email
   {
      [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
      public HeadEmail HeadEmail { get; set; }

      public List<string> Body { get; set; }

      public static List<string> DefineMessageEmailBody(string[] headAndBodySegment, bool optmize)
      {
         List<string> body = new List<string>();

         for (int lineIndex = 1; lineIndex < headAndBodySegment.Length; lineIndex++)
         {
            foreach (string line in headAndBodySegment[lineIndex].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            {
               string lineTreated = line;

               if (optmize)
               {
                  string pattern = @"[a-zA-Z0-9.?!:=\[\]\(\)\/\\]{15,}";
                  lineTreated = Regex.Replace(line, pattern, string.Empty);
               }
               if (!string.IsNullOrEmpty(lineTreated))
                  body.Add(lineTreated);
            }
         }

         return body;
      }

      public static HeadEmail DefineMessageEmailHead(string readFirstIndex)
      {
         if (readFirstIndex is not null)
         {
            HeadEmail headEmail = new HeadEmail();

            string[] segment = readFirstIndex.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            List<string> segmentGeneric = segment.OfType<string>().ToList();

            headEmail.From = segmentGeneric?.FirstOrDefault(x => x.Contains(nameof(headEmail.From), StringComparison.OrdinalIgnoreCase))?.Split(':')?.LastOrDefault();
            headEmail.Title = segmentGeneric?.FirstOrDefault(x => x.Contains(nameof(headEmail.Title), StringComparison.OrdinalIgnoreCase))?.Split(':')?.LastOrDefault();
            headEmail.Date = segmentGeneric?.FirstOrDefault(x => x.Contains(nameof(headEmail.Date), StringComparison.OrdinalIgnoreCase))?.Split(':')?.LastOrDefault();

            return headEmail;
         }
         
         return null;
      }
    }
}
