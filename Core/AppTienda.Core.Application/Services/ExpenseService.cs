using AutoMapper;
using AppTienda.Core.Application.Dtos.Expense;
using AppTienda.Core.Application.Interfaces;
using AppTienda.Core.Domain.Entities;
using AppTienda.Core.Domain.Interfaces;

namespace AppTienda.Core.Application.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;

        public ExpenseService(IExpenseRepository expenseRepository, IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }

        public async Task<List<ExpenseDto>> GetAllAsync()
        {
            var expenses = await _expenseRepository.GetAllAsync();
            return _mapper.Map<List<ExpenseDto>>(expenses.OrderByDescending(e => e.ExpenseDate).ToList());
        }

        public async Task<ExpenseDto?> GetByIdAsync(int id)
        {
            var entity = await _expenseRepository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<ExpenseDto>(entity);
        }

        public async Task<SaveExpenseDto?> GetSaveByIdAsync(int id)
        {
            var entity = await _expenseRepository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<SaveExpenseDto>(entity);
        }

        public async Task<SaveExpenseDto> AddAsync(SaveExpenseDto dto)
        {
            var entity = _mapper.Map<Expense>(dto);
            var result = await _expenseRepository.AddAsync(entity);
            return _mapper.Map<SaveExpenseDto>(result);
        }

        public async Task UpdateAsync(SaveExpenseDto dto)
        {
            var entity = await _expenseRepository.GetByIdAsync(dto.Id);
            if (entity != null)
            {
                entity.ExpenseDate = dto.ExpenseDate;
                entity.Category = dto.Category;
                entity.Description = dto.Description;
                entity.Amount = dto.Amount;
                entity.Notes = dto.Notes;
                await _expenseRepository.UpdateAsync(entity);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _expenseRepository.GetByIdAsync(id);
            if (entity != null)
            {
                await _expenseRepository.DeleteAsync(entity);
            }
        }

        public async Task<List<ExpenseDto>> GetExpensesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var expenses = await _expenseRepository.GetExpensesByDateRangeAsync(startDate, endDate);
            return _mapper.Map<List<ExpenseDto>>(expenses.OrderByDescending(e => e.ExpenseDate).ToList());
        }
    }
}
