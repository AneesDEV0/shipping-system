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
    public class CitiesController : Controller
    {
        private readonly ICityService _ICity;
        private readonly ISenderService _ICountry;
        public CitiesController(ICityService ICity, ISenderService iCountry)
        {
            _ICity = ICity;
            _ICountry = iCountry;
        }
        public IActionResult Index()
        {
            var data = _ICity.GetAllCitites();
            return View(data);
        }

        public IActionResult Edit(Guid? Id)
        {
            TempData["MessageType"] = null;
            var data=new Shared.Dtos.CityDto();
            LoadCountries();
            if (Id != null)
            {
                data = _ICity.GetById((Guid)Id);
            }
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(CityDto data)
        {
            TempData["MessageType"] = null;
            if (!ModelState.IsValid)
            {
                LoadCountries();
                return View("Edit", data);
            }

            try
            {
                if (data.Id == Guid.Empty)
                    _ICity.Add(data);
                else
                    _ICity.Update(data);
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
                _ICity.ChangeStatus(Id, 0);
                TempData["MessageType"] = MessageTypes.DeleteSuccess;
            }
            catch (Exception ex)
            {
                TempData["MessageType"] = MessageTypes.DeleteFailed;
            }

            return RedirectToAction("Index");
        }

        void LoadCountries()
        {
            var countries = _ICountry.GetAll();
            ViewBag.Countries = countries;
        }
    }
}
