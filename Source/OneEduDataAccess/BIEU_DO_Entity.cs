using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BieuDo.Entities
{
   public class BIEU_DO_Entity
    {
       public BIEU_DO_Entity(string name, double value, bool isExplode, string year)
        {
            _name = name;
            _value = value;
            _isExplode = isExplode;
            _year = year;
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private double _value;
        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }
        private bool _isExplode;
        public bool IsisExplode
        {
            get { return _isExplode; }
            set { _isExplode = value; }
        }
        private string _year;
        public string Year
        {
            get { return _year; }
            set { _year = value; }
        }
    }
}
