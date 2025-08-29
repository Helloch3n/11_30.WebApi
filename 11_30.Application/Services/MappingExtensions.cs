using _11_30.Application.Dtos;
using _11_30.Domain.Entities.QuestionBank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Application.Services
{
    public static class MappingExtensions
    {
        public static QuestionBankDto ToDto(this QuestionBank questionBank)
        {
            return new QuestionBankDto
            {
                Id = questionBank.Id,
                Name = questionBank.Name,
                IsOnGoing = questionBank.IsOnGoing
            };
        }
    }
}
