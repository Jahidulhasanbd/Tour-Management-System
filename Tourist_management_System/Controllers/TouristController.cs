using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tourist_management_System.Models;
using Tourist_management_System.Models.ViewModels;

namespace Tourist_management_System.Controllers
{
    public class TouristController : Controller
    {
        private readonly TravelDbContext _context;
        private readonly IWebHostEnvironment _he;
        public TouristController(TravelDbContext context, IWebHostEnvironment he)
        {
            this._context = context;
            this._he = he;
        }
        public async Task<IActionResult> Index()
        {

            return View(await _context.Tourists.Include(x => x.BookingEntries).ThenInclude(y => y.Spot).ToListAsync());
        }
        public IActionResult AddNewSpots(int? id)
        {
            ViewBag.spot = new SelectList(_context.Spots, "SpotId", "SpotName", id.ToString() ?? "");
            return PartialView("_addNewSpots");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( TouristVM touristVM, int[] spotId)
        {
            if (ModelState.IsValid)
            {
                Tourist tourist = new Tourist()
                {
                    TouristName = touristVM.TouristName,
                    BirthDate = touristVM.BirthDate,
                    Age = touristVM.Age,
                    MaritalStatus = touristVM.MaritalStatus
                };
                //Image
                var file = touristVM.PictureFile;
                string webroot = _he.WebRootPath;
                string folder = "Images";
                string imgFileName = Path.GetFileName(touristVM.PictureFile.FileName);
                string fileToSave = Path.Combine(webroot, folder, imgFileName);

                if (file != null)
                {
                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        touristVM.PictureFile.CopyTo(stream);
                        tourist.Picture = "/" + folder + "/" + imgFileName;
                    }
                }

                foreach (var item in spotId)
                {
                    BookingEntry bookingEntry = new BookingEntry()
                    {
                        Tourist = tourist,
                        TouristId = tourist.TouristId,
                        SpotId = item
                    };
                    _context.BookingEntries.Add(bookingEntry);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            var tourist = await _context.Tourists.FirstOrDefaultAsync(x => x.TouristId == id);

            TouristVM touristVM = new TouristVM()
            {
                TouristId = tourist.TouristId,
                TouristName = tourist.TouristName,
                BirthDate = tourist.BirthDate,
                Age = tourist.Age,
                Picture = tourist.Picture,
                MaritalStatus = tourist.MaritalStatus
            };
            var existSpot= _context.BookingEntries.Where(x => x.TouristId == id).ToList();
            foreach (var item in existSpot)
            {
                touristVM.SpotList.Add(item.SpotId);
            }
            return View(touristVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TouristVM touristVM, int[] spotId)
        {
            if (ModelState.IsValid)
            {
                Tourist tourist = new Tourist()
                {
                    TouristId = touristVM.TouristId,
                    TouristName = touristVM.TouristName,
                    BirthDate = touristVM.BirthDate,
                    Age = touristVM.Age,
                    Picture = touristVM.Picture,
                    MaritalStatus = touristVM.MaritalStatus
                };
                var file = touristVM.PictureFile;
                if (file != null)
                {
                    string webroot = _he.WebRootPath;
                    string folder = "Images";
                    string imgFileName = Path.GetFileName(touristVM.PictureFile.FileName);
                    string fileToSave = Path.Combine(webroot, folder, imgFileName);
                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        touristVM.PictureFile.CopyTo(stream);
                        tourist.Picture = "/" + folder + "/" + imgFileName;
                    }
                }

                var existSpot = _context.BookingEntries.Where(x => x.TouristId == tourist.TouristId).ToList();
                foreach (var item in existSpot)
                {
                    _context.BookingEntries.Remove(item);
                }
                foreach (var item in spotId)
                {
                    BookingEntry bookingEntry = new BookingEntry()
                    {

                        TouristId = tourist.TouristId,
                        SpotId = item
                    };
                    _context.BookingEntries.Add(bookingEntry);
                }
                _context.Update(tourist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            Tourist tourist = _context.Tourists.First(x => x.TouristId == id);

            var bookingEntries = _context.BookingEntries.Where(x => x.TouristId == id).ToList();

            TouristVM touristVM = new TouristVM()
            {
                TouristId = tourist.TouristId,
                TouristName = tourist.TouristName,
                BirthDate = tourist.BirthDate,
                Age = tourist.Age,
                Picture = tourist.Picture,
                MaritalStatus = tourist.MaritalStatus

            };
            if (bookingEntries.Count() > 0)
            {
                foreach (var item in bookingEntries)
                {
                    touristVM.SpotList.Add(item.SpotId);
                }
            }

            return View(touristVM);
        }


        [HttpPost]
        [ActionName("Delete")]

        public IActionResult Delete(int id)
        {
            Tourist tourist = _context.Tourists.Find(id);

            if (tourist == null)
            {
                return NotFound();
            }
            _context.Entry(tourist).State = EntityState.Deleted;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}



