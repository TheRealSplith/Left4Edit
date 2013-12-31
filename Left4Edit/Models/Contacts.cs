using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Left4Edit.Models
{
    public class Contact
    {
        public Int32 ID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Phone { get; set; }
        public String Fax { get; set; }
        public String Email { get; set; }
        public Int32? CustomerID { get; set; }
        [JsonIgnore]
        public virtual Customer MyCustomer { get; set; }
    }
}