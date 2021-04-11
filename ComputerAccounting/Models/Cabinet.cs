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
        public List<Computer> Computers { get; set; } = new List<Computer>();

        public int GetCabinetNumber() =>
            Convert.ToInt32(string.Join("", Title.Where(ch => char.IsDigit(ch))));

        public bool ContainsNumber() =>
            new Regex(@"^\D*\s?\d+$", RegexOptions.IgnoreCase).IsMatch(Title);

        public override string ToString() =>
            Title;
    }
}
