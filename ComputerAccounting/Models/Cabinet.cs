using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ComputerAccounting
{
    public class Cabinet : BaseModel
    {
        public int CabinetId { get; set; }
        public static string Icon { get; set; } = "\uE7BE";
        public string Title { get; set; }

        //public override bool Equals(object otherCabinet)
        //{
        //    return Title == (otherCabinet as Cabinet).Title;
        //}

        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}

        public int GetCabinetNumber() =>
            Convert.ToInt32(string.Join("", Title.Where(ch => char.IsDigit(ch))));

        public bool ContainsNumber()
        {
            Regex numberRegex = new Regex(@"^\D*\s?\d*$", RegexOptions.IgnoreCase);
            return numberRegex.IsMatch(Title);
        }
    }
}
