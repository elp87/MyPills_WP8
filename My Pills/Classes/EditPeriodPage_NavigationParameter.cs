using System.Collections.Generic;

namespace My_Pills.Classes
{
    class EditPeriodPage_NavigationParameter
    {
        public string periodName
        {
            get; set;
        }

        public List<Classes.Pill> pills
        {
            get; set;
        }
    }
}
