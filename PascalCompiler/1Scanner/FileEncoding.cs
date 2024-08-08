using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PascalCompiler.Scanner
{
   static class FileEncoding
   {
      //https://stackoverflow.com/questions/3825390/effective-way-to-find-any-files-encoding
      //todo: determine if there are better methods for this. Just print a warning if not UTF8.
      public static Encoding? GetFileEncoding(string filename)
      {
         // Read the BOM
         var bom = new byte[4];
         using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
         {
            file.Read(bom, 0, 4);
         }

         // Analyze the BOM
         if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
         if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
         if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return Encoding.UTF32; //UTF-32LE
         if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
         if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
         if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new UTF32Encoding(true, true);  //UTF-32BE

         // We actually have no idea what the encoding is if we reach this point, so
         // you may wish to return null instead of defaulting to ASCII
         return null;
      }
   }
}
