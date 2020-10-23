using System;
using System.Collections.Generic;
using System.Text;

namespace SpecFlowExercise.Pages
{
    public class DressQuickViewInfo
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }//Properties
        public string Condition { get; set; }

        public int Quantity { get; set; }

        public string Size { get; set; }

        public List<string> AvailableColours { get; set; }
    }
}
