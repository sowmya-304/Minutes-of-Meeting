/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MOMC_PROJECT
{
    public class ProjectData
    {
        public string projectName { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public List<string[]> DataGridViewData { get; set; }
        public ProjectData(string projectName, string subject, string description, List<string[]> dataGridViewData)
        {
            this.projectName = projectName;
            Subject = subject;
            Description = description;
            DataGridViewData = dataGridViewData;

        }
    }
}
*/
using System;
using System.Collections.Generic;

namespace MOMC_PROJECT
{
    public class ProjectData
    {
        public string projectName { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }

        // Default constructor
        public ProjectData()
        {
        }

        // Parameterized constructor
        public ProjectData(string projectName, string subject, string description)
        {
            this.projectName = projectName;
            Subject = subject;
            Description = description;
        }
    }

}
