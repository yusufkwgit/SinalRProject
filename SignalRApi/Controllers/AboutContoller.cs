using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinnesLayer.Abstract;
using SignalR.DtoLayer.AboutDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutContoller : ControllerBase
    {
        private readonly IAboutService _aboutService;

        public AboutContoller(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        [HttpGet]
        public IActionResult AboutList()
        {
            var value = _aboutService.TGetListAll();
            return Ok(value);
        }

        [HttpPost]
        public IActionResult CreateAbout(CreateAboutDto createAboutDto)
        {
            About about = new About()
            {
                ImageUrl = createAboutDto.ImageUrl,
                Title = createAboutDto.Title,
                Description = createAboutDto.Description
            };
            _aboutService.TAdd(about);
            return Ok("Hakkımda Oluşturuldu");
        }

        [HttpDelete]
        public IActionResult DeleteAbout(int id)
        {
            var values = _aboutService.TGetByID(id);
            _aboutService.TDelete(values);
            return Ok("Hakkımda Silindi");
        }

        [HttpPut]
        public IActionResult UpdateAbout(UpdateAboutDto updateAboutDto)
        {
            About about = new About()
             {
                 ImageUrl = updateAboutDto.ImageUrl,
                 Title = updateAboutDto.Title,
                 Description = updateAboutDto.Description
             };
            _aboutService.TUpdate(about);
            return Ok("Hakkımda Güncellendi");
        }

        [HttpGet("GetAbout")]

        public IActionResult GetAbout(int id)
        {
            var value = _aboutService.TGetByID(id);
            return Ok(value);
        }
    }
}