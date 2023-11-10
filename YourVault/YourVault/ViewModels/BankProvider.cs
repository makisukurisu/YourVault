using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourVault.ViewModels
{
    public class BankProvider
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }

        public BankProvider(int id, string name, string website)
        {
            ID = id;
            Name = name;
            Website = website;
        }
    }
}
