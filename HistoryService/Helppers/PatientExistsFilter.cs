using HistoryService.Data;
using HistoryService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace HistoryService.Helppers
{
    public class PatientExistsFilter : Attribute, IAsyncResourceFilter
    {
        private readonly AppDbContext _context;

        public PatientExistsFilter(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var patientId = context.HttpContext.Request.RouteValues["patientId"]?.ToString();
            if (patientId == null) return;
            var exists = await _context.Set<Patient>().AnyAsync(p => p.Id == patientId);
            if (!exists) context.Result = new NotFoundResult();
            else await next();
        }
    }
}
