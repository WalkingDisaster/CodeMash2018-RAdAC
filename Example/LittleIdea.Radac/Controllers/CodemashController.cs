using System;
using LittleIdea.Radac.Models.Codemash;
using LittleIdea.Radac.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LittleIdea.Radac.Controllers
{
    [Route("")]
    [Authorize]
    public class CodemashController : Controller
    {
        [Route("")]
        [Authorize(Policy = PolicyStore.AnyEmployee)]
        public IActionResult Index()
        {
            return View(new IndexViewModel());
        }

        [Route("patient/{patientId}/appointment/{appointmentId}")]
        [Authorize(Roles = Roles.Doctor)]
        [HttpGet]
        public IActionResult PatientVisit(Guid patientId, Guid appointmentId)
        {
            return View(new PatientVisitViewModel
            {
                PatientId = patientId,
                AppointmentId = appointmentId
            });
        }

        [Route("patient/{patientId}/appointment/{appointmentId}/unmask")]
        [Authorize(Roles = Roles.Doctor)]
        [Authorize(Policy = PolicyStore.UnmaskPhi)]
        [Authorize(Policy = PolicyStore.DataRisk)]
        [HttpGet]
        public ActionResult Unmask(Guid patientId, Guid appointmentId)
        {
            var risk = HttpContext.Items["risk"];
            return Ok(new
            {
                status = "success",
                result = $"Unmasked secure data::Risk Score = {risk}"
            });
        }
    }
}