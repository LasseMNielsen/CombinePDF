using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace combinePDF.Objects
{
    class listboxPdfObjects
    {
        public string fullname { get; set; }
        public string friendlyName { get; set; }

        public static List<listboxPdfObjects> createList(List<string>inputList)
        {
            var returnList = new List<listboxPdfObjects>();

            foreach (var item in inputList)
            {
                if (File.Exists(item))
                {
                    var obj = new listboxPdfObjects();
                    obj.friendlyName = item.Split('\\').Last();
                    obj.fullname = item;
                    returnList.Add(obj);
                }
            }

            return returnList.OrderBy(x=> x.friendlyName).ToList();
        }
    }
}
