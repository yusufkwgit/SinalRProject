using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinnesLayer.Abstract;
using SignalR.DtoLayer.ContactDto;
using SignalR.DtoLayer.DiscountDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly IMapper _mapper;

        public DiscountController(IDiscountService discountService, IMapper mapper)
        {
            _discountService = discountService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult DiscountList()
        {
            var value = _mapper.Map<List<ResultDiscountDto>>(_discountService.TGetListAll());
            return Ok(value);
        }

        [HttpPost]
        public IActionResult CreateDiscount(CreateDiscountDto createDiscount)
        {
            _discountService.TAdd(new Discount()
            {
                Title = createDiscount.Title,
                Amount = createDiscount.Amount,
                Description = createDiscount.Description,
                ImageUrl = createDiscount.ImageUrl,
                Status = createDiscount.Status

            });
            return Ok("İndirim Oluşturuldu");
        }

        [HttpDelete]
        public IActionResult DeleteDiscount(int id)
        {
            var values = _discountService.TGetByID(id);
            _discountService.TDelete(values);
            return Ok("İndirim Silindi");
        }

        [HttpGet("GetDiscount")]
        public IActionResult GetDiscount(int id)
        {
            var value = _discountService.TGetByID(id);
            return Ok(value);
        }
        [HttpPut]

        public IActionResult UpdateDiscount(UpdateDiscountDto updateDiscount)
        {
            _discountService.TUpdate(new Discount()
            {
                DiscountID = updateDiscount.DiscountID,
                Title = updateDiscount.Title,
                Amount = updateDiscount.Amount,
                Description = updateDiscount.Description,
                ImageUrl = updateDiscount.ImageUrl,
                Status = updateDiscount.Status,
            });
            return Ok("İndirim Güncellendi");
        }


    }
}
