using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Som_Models.Models;
using Som_Models.VW_Models;
using Som_Service.Interface;
using Som_Service.Service;
namespace Som_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KistiCtrl : ControllerBase
    {
        private readonly IKistiService _kistiservice;

        public KistiCtrl(IKistiService kistiservice)
        {
            _kistiservice = kistiservice;
        }
        [HttpGet("crtype")]
        [Authorize]
        public async Task<IActionResult> GetCrType()
        {
            var mem = await _kistiservice.GetCrData();

            if (mem == null)
                return NotFound("No Cr found.");

            return Ok(mem);
        }
        [HttpGet("kistytype")]
        [Authorize]
        public async Task<IActionResult> GetKistiType(int compId)
        {
            var mem = await _kistiservice.GetKistiTypes(compId);

            if (mem == null)
                return NotFound("No Kisti Type found.");

            return Ok(mem);
        }
        [HttpGet("kistytypebyid")]
        [Authorize]
        public async Task<IActionResult> GetKistiTypeById(int id)
        {
            var mem = await _kistiservice.GetKistiTypesById(id);

            if (mem == null)
                return NotFound("No Kisti Type found.");

            return Ok(mem);
        }
        [HttpGet("kistytypebyproject")]
        [Authorize]
        public async Task<IActionResult> GetKistiTypeByProject(int compId,int projectId)
        {
            var mem = await _kistiservice.GetKistiTypesByProject(compId,projectId);

            if (mem == null)
                return NotFound("No Project Type found.");

            return Ok(mem);
        }
        [HttpPost("savekistitype")]
        [Authorize]
        public async Task<IActionResult> SaveKistiType(KistiTypes k)
        {
            var res = await _kistiservice.SaveKistiType(k);

            // Always return string, null hole "Failed" return
            return Ok(res ?? "Failed to save Kisti Type");
        }
    }
}
