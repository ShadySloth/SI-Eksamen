using AutoMapper;
using Backend_Service.Domain.DTOs;
using Backend_Service.Domain.Entities;

namespace Backend_Service.Application.Mappers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        //Image
        CreateMap<Image, ImageDto>();
        CreateMap<ImageDto, Image>();
        
        //Label
        CreateMap<Label, LabelDto>();
        CreateMap<LabelDto, Label>();
        
        //Segmentation
        CreateMap<Segmentation, SegmentationDto>();
        CreateMap<SegmentationDto, Segmentation>();
    }
}