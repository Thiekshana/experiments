using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_IRAS_Project.classes
{
    public class Property /*: IEquatable<Property>*/
    {
        [Display (Name ="Home Address")]
        public string Address { get; set; }
        [Display(Name = "Level-Units")]
        public List<string> Units { get; set; }
        [Display(Name = "House Pets")]
        public List<string> Pets { get; set; }


        //public bool Equals(Property other)
        //{
        //    return this.Address.Equals(other.Address) && this.Level.Equals(other.Level) && this.Unit.Equals(other.Unit);
        //}

        public Property(string add, List<string> uns, List<string> pets)
        {
            Address = add;
            Units = uns;
            Pets = pets;
        }
    }
}
