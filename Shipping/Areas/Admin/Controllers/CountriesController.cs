using BusinessLayer.Contracts;
using Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ShippingApp.Helpers;
namespace ShippingApp.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class CountriesController : Controller
    {
        private readonly ISenderService _ICountry;
        public CountriesController(ISenderService ICountry)
        {
            _ICountry= ICountry;
        }
        public IActionResult Index()
        {
            var data = _ICountry.GetAll();
            return View(data);
        }

        public IActionResult Edit(Guid? Id)
        {
            TempData["MessageType"] = null;
            var data=new Shared.Dtos.CountryDto(); 
            if (Id != null)
            {
                data = _ICountry.GetById((Guid)Id);
            }
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(CountryDto data)
        {
            TempData["MessageType"] = null;
            if (!ModelState.IsValid)
                return View("Edit", data);
            try
            {
                if (data.Id == Guid.Empty)
                    _ICountry.Add(data);
                else
                    _ICountry.Update(data);
                TempData["MessageType"] = MessageTypes.SaveSucess;
            }
            catch (Exception ex)
            {
                TempData["MessageType"] = MessageTypes.SaveFailed;
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid Id)
        {
            TempData["MessageType"] = null;
            try
            {
                _ICountry.ChangeStatus(Id, 0);
                TempData["MessageType"] = MessageTypes.DeleteSuccess;
            }
            catch (Exception ex)
            {
                TempData["MessageType"] = MessageTypes.DeleteFailed;
            }

            return RedirectToAction("Index");
        }
    }
}
