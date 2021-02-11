using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerAccounting
{
    public class Cabinet : BaseModel
    {
        public int CabinetId { get; set; }
        public static string Icon { get; set; } = "\uE7BE";

        private string _title;
        public string Title
        {
            get => _title;

            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        //public override bool Equals(object otherCabinet)
        //{
        //    return Title == (otherCabinet as Cabinet).Title;
        //}

        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}
    }
}
