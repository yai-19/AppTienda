using AppTienda.Core.Application.Dtos.Expense;
using AppTienda.Core.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AppTienda.App.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        public async Task<IActionResult> Index()
        {
            var expenses = await _expenseService.GetAllAsync();
            return View(expenses);
        }

        public IActionResult Create()
        {
            return View("Save", new SaveExpenseDto { ExpenseDate = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveExpenseDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", dto);
            }

            await _expenseService.AddAsync(dto);
            TempData["Success"] = "Gasto registrado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var expense = await _expenseService.GetSaveByIdAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            return View("Save", expense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SaveExpenseDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", dto);
            }

            await _expenseService.UpdateAsync(dto);
            TempData["Success"] = "Gasto actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _expenseService.DeleteAsync(id);
            TempData["Success"] = "Gasto eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
