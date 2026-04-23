using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinnesLayer.Abstract;
using SignalR.DtoLayer.DiscountDto;
using SignalR.DtoLayer.FeatureDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureController : ControllerBase
    {
        private readonly IFeatureService _featureService;
        private readonly IMapper _mapper;

        public FeatureController(IFeatureService featureService, IMapper mapper)
        {
            _featureService = featureService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult FeatureList()
        {
            var value = _mapper.Map<List<ResultFeatureDto>>(_featureService.TGetListAll());
            return Ok(value);
        }

        [HttpPost]
        public IActionResult CreateFeature(CreateFeatureDto createFeature)
        {
            _featureService.TAdd(new Feature()
            {
                Description1 = createFeature.Description1,
                Description2 = createFeature.Description2,
                Description3 = createFeature.Description3,
                Title1 = createFeature.Title1,
                Title2 = createFeature.Title2,
                Title3 = createFeature.Title3,

            });
            return Ok("Öne çıkarılan oluşturuldu");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFeature(int id)
        {
            var values = _featureService.TGetByID(id);
            _featureService.TDelete(values);
            return Ok("Öne çıkan Silindi");
        }

        [HttpGet("{id}")]
        public IActionResult GetFeature(int id)
        {
            var value = _featureService.TGetByID(id);
            return Ok(value);
        }
        [HttpPut]

        public IActionResult UpdateFeature(UpdateFeatureDto updateFeature)
        {
            _featureService.TUpdate(new Feature()
            {

                FeatureID = updateFeature.FeatureID,
                Description1 = updateFeature.Description1,
                Description2 = updateFeature.Description2,
                Description3 = updateFeature.Description3,
                Title1 = updateFeature.Title1,
                Title2 = updateFeature.Title2,
                Title3 = updateFeature.Title3,
            });
            return Ok("Öne çıkanlar Güncellendi");
        }




    }
}
