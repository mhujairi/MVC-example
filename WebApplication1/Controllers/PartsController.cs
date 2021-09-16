using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;

using WebApplication1.Business;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PartsController : Controller
    {
        private readonly IPartsProvider partsProvider;

        public PartsController(IPartsProvider partsProvider)
        {
            if (partsProvider is null)
            {
                throw new ArgumentNullException(nameof(partsProvider));
            }

            this.partsProvider = partsProvider;
        }

        // GET: PartsController
        public async Task<ActionResult> Index()
        {
            return View();
        }

        // GET: PartsController
        public async Task<ActionResult> List()
        {
            return PartialView("List", await partsProvider.GetAllAsync());
        }

        // GET: PartsController/Create
        public async Task<ActionResult> Create()
        {
            return PartialView("Create", new Part());
        }

        // POST: PartsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Part part)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await partsProvider.AddAsync(part);
                    return await List();
                }

                return PartialView("Create");
            }
            catch (ArgumentException ex)
            {
                AddedArgumentExceptionError(ex);
                return PartialView("Create");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("Create");
            }
        }

        private static Part GetPart(IFormCollection collection)
        {
            return new Part()
            {
                PartNumberCommonized = collection[nameof(Part.PartNumberCommonized)],
                PartH = Convert.ToDouble(collection[nameof(Part.PartH)]),
                PartL = Convert.ToDouble(collection[nameof(Part.PartL)]),
                PartW = Convert.ToDouble(collection[nameof(Part.PartW)]),
                PartIntroDate = Convert.ToDateTime(collection[nameof(Part.PartIntroDate)]),
            };
        }

        // GET: PartsController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            return PartialView("Edit", await partsProvider.GetAsync(id));
        }

        // POST: PartsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, Part part)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await partsProvider.UpdateAsync(id, part);
                    return await List();
                }

                return PartialView("Edit");
            }
            catch (ArgumentException ex)
            {
                AddedArgumentExceptionError(ex);
                return PartialView("Edit");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("Edit");
            }
        }

        private void AddedArgumentExceptionError(ArgumentException ex)
        {
            ModelState.AddModelError(ex.ParamName, ex.Message.Substring(0, ex.Message.IndexOf(" (Parameter ")));
        }

        // GET: PartsController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            return PartialView("Delete", await partsProvider.GetAsync(id));
        }

        // POST: PartsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Part part)
        {

            try
            {
                await partsProvider.DeleteAsync(part.PartNumberCommonized);
                return await List();
            }
            catch (ArgumentException ex)
            {
                AddedArgumentExceptionError(ex);
                return PartialView("Delete");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("Delete");
            }
        }
    }
}
