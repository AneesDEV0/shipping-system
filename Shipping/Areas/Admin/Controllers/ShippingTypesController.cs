using BusinessLayer.Contracts;
using Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingApp.Helpers;

namespace ShippingApp.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class ShippingTypesController : Controller
    {
        IShippingTypeService _shippingTypeService;
        public ShippingTypesController(IShippingTypeService shippingTypeService)
        {
            _shippingTypeService = shippingTypeService;
        }

        public IActionResult Index()
        {
            var shippingTypes = _shippingTypeService.GetAll();
            return View(shippingTypes);
        }
        public IActionResult Edit(Guid? id)
        {
            TempData["MessageType"] = null;

            var data = new ShippingTypeDto();

            if (id != null)
                data = _shippingTypeService.GetById((Guid)id);

            return View(data);
        }
 

        public async Task<IActionResult> Save(ShippingTypeDto data)
        {
            TempData["MessageType"] = null;

            try
            {
                if (!ModelState.IsValid)
                    return View("Edit", data);


                if (data.Id == Guid.Empty)
                    _shippingTypeService.Add(data);
                else
                    _shippingTypeService.Update(data);

                TempData["MessageType"] = MessageTypes.SaveSucess;


            }
            catch (Exception ex)
            {
                TempData["MessageType"] = MessageTypes.SaveFailed;
                return View("Edit", data);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid Id)
        {
            TempData["MessageType"] = null;
            try
            {
               _shippingTypeService.ChangeStatus(Id ,0);
                TempData["MessageType"] = MessageTypes.DeleteSuccess;
            }
            catch(Exception ex)
            {
                TempData["MessageType"] = MessageTypes.DeleteFailed;
            }
            return RedirectToAction("Index");


        }
    }
}
