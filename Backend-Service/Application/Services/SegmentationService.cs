using AutoMapper;
using Backend_Service.Application.Interfaces;
using Backend_Service.Domain.DTOs;
using Backend_Service.Domain.Entities;
using Backend_Service.Infrastructure.Interfaces;

namespace Backend_Service.Application.Services;

public class SegmentationService : ISegmentationService
{
    private readonly ISegmentationRepository _segmentationRepository;
    private readonly IMapper _mapper;

    public SegmentationService(ISegmentationRepository segmentationRepository, IMapper mapper)
    {
        _segmentationRepository = segmentationRepository;
        _mapper = mapper;
    }

    public async Task<SegmentationDto> GetSegmentationById(Guid segmentationId)
    {
        var segmentation = await _segmentationRepository.GetSegmentationById(segmentationId);
        var segmentationDto = _mapper.Map<SegmentationDto>(segmentation);
        return segmentationDto;
    }

    public async Task<SegmentationDto[]> GetSegmentationsByLabel(Guid labelId)
    {
        var segmentations = await _segmentationRepository.GetSegmentationsByLabel(labelId);
        var segmentationDtos = _mapper.Map<SegmentationDto[]>(segmentations);
        return segmentationDtos;
    }

    public async Task<SegmentationDto[]> GetSegmentationsByImage(Guid imageId)
    {
        var segmentations = await _segmentationRepository.GetSegmentationsByImage(imageId);
        var segmentationDtos = _mapper.Map<SegmentationDto[]>(segmentations);
        return segmentationDtos;
    }

    public async Task<SegmentationDto> CreateSegmentation(SegmentationDto segmentation)
    {
        var segmentationToCreate = _mapper.Map<Segmentation>(segmentation);
        var createdSegmentation = await _segmentationRepository.CreateSegmentation(segmentationToCreate);
        var createdSegmentationDto = _mapper.Map<SegmentationDto>(createdSegmentation);
        return createdSegmentationDto;
    }

    public async Task<SegmentationDto> UpdateSegmentation(SegmentationDto segmentation)
    {
        var segmentationToUpdate = _mapper.Map<Segmentation>(segmentation);
        var updatedSegmentation = await _segmentationRepository.UpdateSegmentation(segmentationToUpdate);
        var updatedSegmentationDto = _mapper.Map<SegmentationDto>(updatedSegmentation);
        return updatedSegmentationDto;
    }

    public Task DeleteSegmentation(Guid segmentationId)
    {
        _segmentationRepository.DeleteSegmentation(segmentationId);
        return Task.CompletedTask;
    }
}