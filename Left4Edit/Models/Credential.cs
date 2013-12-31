using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Left4Edit.Models
{
    public class Credential
    {
        public Int32 ID { get; set; }
        public String Domain { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public Int32? CustomerID { get; set; }
        [JsonIgnore]
        public virtual Customer MyCustomer { get; set; }
        public Int32? NodeID { get; set; }
        [JsonIgnore]
        public virtual Node MyNode { get; set; }
    }
}