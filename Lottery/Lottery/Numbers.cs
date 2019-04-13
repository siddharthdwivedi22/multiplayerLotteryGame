using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery
{
    public class Numbers
    {
        private string pname, no1, no2, no3, no4, no5, dno;

    public string SetName
        {
            get { return pname; }
            set { pname = value; }
        }
        public string SetNo1
        {
            get { return no1; }
            set { no1 = value; }
        }
        public string SetNo2
        {
            get { return no2; }
            set { no2 = value; }
        }
        public string SetNo3
        {
            get { return no3; }
            set { no3 = value; }
        }
        public string SetNo4
        {
            get { return no4; }
            set { no4 = value; }
        }
        public string SetNo5
        {
            get { return no5; }
            set { no5 = value; }
        }
        public string SetDno
        {
            get { return dno; }
            set { dno = value; }
        }
        public Numbers(string pn, string n1, string n2, string n3, string n4, string n5, string ldno)
        {
            pname = pn;
            no1 = n1;
            no2 = n2;
            no3 = n3;
            no4 = n4;
            no5 = n5;
            ldno = dno;
        }
    }
}
