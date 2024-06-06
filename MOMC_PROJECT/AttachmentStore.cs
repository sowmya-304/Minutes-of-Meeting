using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOMC_PROJECT
{
    public class AttachmentStore
    {
        public static List<string> Attachments { get; set; }

        public  AttachmentStore()
        {

           Attachments = new List<string>();
        }
        public AttachmentStore(List<string> attachments)
        {
            Attachments = attachments;
           
        }
    }
}
