﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.Requests.Transactions
{
    public class GetTransactionByIdRequest : Request
    {
        public long Id { get; set; }
        public object? StartDate { get; set; }
        public object? EndDate { get; set; }
    }
}
