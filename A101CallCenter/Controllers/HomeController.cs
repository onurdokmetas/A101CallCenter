using A101CallCenter.DAL;
using A101CallCenter.Models;
using A101CallCenter.Pagination;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Diagnostics;

namespace A101CallCenter.Controllers
{
    public class HomeController : Controller
    {
        readonly ILogger<HomeController> logger;
        readonly IUnitOfWork uow;
        readonly IToastNotification toastNotification;

        public HomeController(ILogger<HomeController> Logger, IUnitOfWork Uow, IToastNotification ToastNotification)
        {
            logger = Logger;
            uow = Uow;
            toastNotification = ToastNotification;
        }

        public async Task<IActionResult> Index(string sortOrder, string sortType, bool sortCommand, string searchString, int? pageNumber)
        {
            IEnumerable<Customer> customers; 

            ViewBag.SearchParam = searchString;

            if (!string.IsNullOrEmpty(searchString))
            {
                customers = await uow.CustomerRepository.Find(x => x.Name.Contains(searchString) || x.Email.Contains(searchString) || x.City.Contains(searchString)
                 || x.District.Contains(searchString) || x.Address.Contains(searchString) || x.Review.Contains(searchString));
            }
            else
            {
                customers = await uow.CustomerRepository.GetAll();
            }

            if (!string.IsNullOrEmpty(sortType))
            {
                if (sortCommand)
                {
                    if (string.IsNullOrEmpty(sortOrder))
                    {
                        sortOrder = "asc";
                    }
                    else if (sortOrder == "asc")
                    {
                        sortOrder = "desc";
                    }
                    else
                    {
                        sortOrder = "asc";
                    }
                }
                
                ViewBag.SortType = sortType;
                ViewBag.SortOrder = sortOrder;

                switch (sortType)
                {
                    case "name":
                        if(sortOrder == "asc")
                        {
                            customers = customers.OrderBy(x => x.Name);
                        }
                        else
                        {
                            customers.OrderByDescending(x => x.Name);
                        }
                        break;
                    case "age":
                        if(sortOrder == "asc")
                        {
                            customers = customers.OrderBy(x => x.Age);
                        }
                        else
                        {
                            customers = customers.OrderByDescending(x => x.Age);
                        }
                        break;
                    case "monthlyIncome":
                        if (sortOrder == "asc")
                        {
                            customers = customers.OrderBy(x => x.MonthlyIncome);
                        }
                        else
                        {
                            customers = customers.OrderByDescending(x => x.MonthlyIncome);
                        }
                        break;
                    case "city":
                        if (sortOrder == "asc")
                        {
                            customers = customers.OrderBy(x => x.City);
                        }
                        else
                        {
                            customers = customers.OrderByDescending(x => x.City);
                        }
                        break;
                    case "district":
                        if (sortOrder == "asc")
                        {
                            customers = customers.OrderBy(x => x.District);
                        }
                        else
                        {
                            customers = customers.OrderByDescending(x => x.District);
                        }
                        break;
                }
            }
            int pageSize = 3;
            return View(PaginatedList<Customer>.CreateAsync(customers, pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Delete(int id, int? pageNumber)
        {
            try
            {
                uow.CustomerRepository.Remove(await uow.CustomerRepository.GetById(id));
                await uow.CompleteTransactions();
                toastNotification.AddInfoToastMessage("Record deleted!");
            }
            catch (Exception ex)
            {
                toastNotification.AddErrorToastMessage("An error occured.");
                logger.LogError(ex.Message);
            }
            return RedirectToAction("Index", "Home", new { pageNumber = pageNumber });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Customer());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            try
            {
                await uow.CustomerRepository.Add(customer);
                await uow.CompleteTransactions();
                toastNotification.AddSuccessToastMessage("Customer record created!");
            }
            catch(Exception ex)
            {
                toastNotification.AddErrorToastMessage("An error occured.");
                logger.LogError(ex.Message);
            }
            return RedirectToAction("Create", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, int? pageNumber)
        {
            ViewBag.PageIndex = pageNumber;
            return View(await uow.CustomerRepository.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Customer customer, int? pageNumber)
        {
            try
            {
                Customer editCustomer = await uow.CustomerRepository.GetById(customer.Id);
                editCustomer.Address = customer.Address;
                editCustomer.City = customer.City;
                editCustomer.Review = customer.Review;
                editCustomer.Age = customer.Age;
                editCustomer.Email = customer.Email;
                editCustomer.District = customer.District;
                editCustomer.Name = customer.Name;
                editCustomer.MonthlyIncome = customer.MonthlyIncome;
                await uow.CompleteTransactions();
                toastNotification.AddSuccessToastMessage("Changes saved!");
            }
            catch (Exception ex)
            {
                toastNotification.AddErrorToastMessage("An error occured.");
                logger.LogError(ex.Message);
            }
            
            return RedirectToAction("Index", "Home", new { pageNumber = pageNumber });
        }

        //template code
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}