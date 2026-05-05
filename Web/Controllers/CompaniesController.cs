using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendLab01.Controllers;

[ApiController]
[Route("/api/companies")]
public class CompaniesController(ICompanyService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await service.GetAllCompanies());
    }
}
