using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Model;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DenemeController : ControllerBase
    {
        private Context _dbContext;
        private IProductRepository _productRepository;

        public DenemeController(Context dbContext, IProductRepository productRepository)
        {
            _dbContext = dbContext;
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            /* Fake Data */
            return Ok(_productRepository.Get().ToList());

            /* Veri Tabanı */
            //var denemeValues = _dbContext.Denemes.ToList();

            //if (_dbContext.Denemes == null)
            //    return NotFound();

            //return Ok(denemeValues);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var value = _productRepository.Get().ToArray();
            return Ok(value[id]);
            /*Veri Tabanı*/
            //var value = _dbContext.Denemes.Find(id);

            //if (_dbContext.Denemes == null || value == null)
            //    return NotFound();

            //return Ok(value);
        }
        [HttpPost]
        public IActionResult Post(Deneme deneme)
        {
            _dbContext.Denemes.Add(deneme);
            _dbContext.SaveChanges();
            return Ok("Başarıyla Eklendi");
        }

        [HttpPut]
        public IActionResult Put(Deneme deneme)
        {
            try
            {
                var value = _dbContext.Denemes.FirstOrDefault(x => x.id == deneme.id);

                if (value == null)
                    return NotFound();

                value.Name = deneme.Name;
                value.Surname = deneme.Surname;
                value.Status = deneme.Status;

                _dbContext.Entry(value).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error has occurred");
            }

            return Ok(_dbContext.Denemes.ToList());
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var value = _dbContext.Denemes.FirstOrDefault(x => x.id == id);
                if (value == null)
                    return NotFound();
                
                _dbContext.Entry(value).State = EntityState.Deleted;
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error has occurred");
            }

            return Ok(_dbContext.Denemes.ToList());
        }
    }
}
