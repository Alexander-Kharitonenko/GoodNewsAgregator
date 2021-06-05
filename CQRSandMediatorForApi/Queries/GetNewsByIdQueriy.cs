﻿using DTO_Models_For_GoodNewsGenerator;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSandMediatorForApi.Queries
{
    public class GetNewsByIdQueriy : IRequest<NewsModelDTO>
    {
        public Guid Id { get; set; }
    }
}
