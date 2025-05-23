﻿using ASC.Model.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASC.Model.Model
{
    public class MasterDataKey : BaseEntity
    {
        public MasterDataKey() { }
        public MasterDataKey(string key)
        {
            this.RowKey = Guid.NewGuid().ToString();
            this.PartitionKey = key;
        }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } 
        public string Name { get; set; }
    }
}
