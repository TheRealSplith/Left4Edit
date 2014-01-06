using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Left4Edit.Models
{
    public class Node
    {
        public Node()
        {
            Credentials = new List<Credential>();
        }

        public Int32 ID { get; set; }
        public String Name { get; set; }
        public String Address { get; set; }
        public String Comment { get; set; }
        public virtual IList<Credential> Credentials { get; set; }
        public Int32? CustomerID { get; set; }
        [JsonIgnore]
        public virtual Customer MyCustomer { get; set; }
    }
}