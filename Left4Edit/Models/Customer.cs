using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Left4Edit.Models
{
    public class Customer
    {
        public Customer()
        {
            Nodes = new List<Node>();
            Credentials = new List<Credential>();
            Contacts = new List<Contact>();
        }

        public Int32 ID { get; set; }
        public String Name { get; set; }
        public String Symbol { get; set; }
        public virtual IList<Node> Nodes { get; set; }
        public virtual IList<Credential> Credentials { get; set; }
        public virtual IList<Contact> Contacts { get; set; }
    }
}