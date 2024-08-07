﻿using Caretta.Core.Dto.CategoryDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Business.Queries
{
    public class GetAllCategoriesQuery : IRequest<IEnumerable<CategoryGetDto>> { }
}