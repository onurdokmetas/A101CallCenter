using A101CallCenter.DAL;
using A101CallCenter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace A101CallCenter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        readonly IUnitOfWork uow;
        readonly ILogger<ApiController> logger;

        public ApiController(IUnitOfWork Uow, ILogger<ApiController> Logger)
        {
            uow = Uow;
            logger = Logger;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await uow.CustomerRepository.Add(customer);
                    await uow.CompleteTransactions();
                    return StatusCode(201);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    return StatusCode(500);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("createlist")]
        public async Task<IActionResult> CreateList(List<Customer> customerList)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await uow.CustomerRepository.AddRange(customerList);
                    await uow.CompleteTransactions();
                    return StatusCode(201);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    return StatusCode(500);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await uow.CustomerRepository.GetById(id));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await uow.CustomerRepository.GetAll());
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("update/{id}")]
        public async Task<IActionResult> Update(int id, Customer newCustomer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Customer customer = await uow.CustomerRepository.GetById(id);
                    customer.Name = newCustomer.Name;
                    customer.Email = newCustomer.Email;
                    customer.City = newCustomer.City;
                    customer.District = newCustomer.District;
                    customer.Address = newCustomer.Address;
                    customer.Age = newCustomer.Age;
                    customer.MonthlyIncome = newCustomer.MonthlyIncome;
                    customer.Review = newCustomer.Review;
                    await uow.CompleteTransactions();
                    return Ok();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    return StatusCode(500);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                uow.CustomerRepository.Remove(await uow.CustomerRepository.GetById(id));
                await uow.CompleteTransactions();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("deletelist")]
        public async Task<IActionResult> DeleteList(List<int> list)
        {
            try
            {
                IEnumerable<Customer> customerDeleteList = await uow.CustomerRepository.Find(x => list.Contains(x.Id));
                uow.CustomerRepository.RemoveRange(customerDeleteList);
                await uow.CompleteTransactions();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }
    }
}
