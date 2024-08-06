﻿using Finances.Core.Enums;
using Finances.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finances.Core.Requests.Transactions
{
    public class CreateTransactionRequest : Request
    {
        [Required(ErrorMessage = "Titulo Inválido")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipo Inválido")]
        public ETransactionType Type { get; set; } = ETransactionType.Withdraw;

        [Required(ErrorMessage = "Valor Inválido")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Categoria Inválida")]
        public long CategoryId { get; set; }

        [Required(ErrorMessage = "Data Inválida")]
        public DateTime? PaidOrReceivedAt { get; set; }

    }
}
